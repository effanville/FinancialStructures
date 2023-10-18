using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common.Structure.Reporting;
using Common.Structure.WebAccess;

using FinancialStructures.Stocks.Persistence.Database.Interactions;
using FinancialStructures.Stocks.Persistence.Models;

using HtmlAgilityPack;

namespace FinancialStructures.Stocks.Persistence.Database.Setup
{
    internal class InstrumentDataLoader
    {
        private static async Task<Dictionary<string, string>> GetExtraData(Instrument instrument, IReportLogger logger = null)
        {
            string urlData = await WebDownloader.DownloadFromURLasync(instrument.Url, logger);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(urlData);
            var quoteSummaryElement = htmlDocument.GetElementbyId("quote-summary");

            var summaryColumn1 = quoteSummaryElement.ChildNodes.First();
            var summaryColumn2 = quoteSummaryElement.ChildNodes.Last();
            var tableValues = summaryColumn1.ChildNodes.First().ChildNodes.First().ChildNodes;
            var table2Values = summaryColumn2.ChildNodes.First().ChildNodes.First().ChildNodes;

            var dict = new Dictionary<string, string>();
            foreach (var child in tableValues.Union(table2Values))
            {
                string key = child.ChildNodes.First().InnerText;
                string value = child.ChildNodes.Last().InnerText;
                dict[key] = value;
            }

            return dict;
        }

        public static async Task<int> InsertInstrumentData(StockExchangeDbContext context, string indexName, IList<string> instrumentData, IReportLogger logger = null)
        {
            var snapshotTime = DateTime.Now;
            var instrumentDataList = new List<InstrumentData>();
            foreach (string instrumentStrings in instrumentData)
            {
                string[] instValues = instrumentStrings.Split(' ');
                var instrument = context.Instruments.SingleOrDefault(inst => inst.Company == instValues[0]);

                if (instrument == null)
                {
                    continue;
                }

                var values = await GetExtraData(instrument, logger);
                if (instValues.Length > 7)
                {
                    string marketCapString = instValues[^4];
                    double marketCap = double.Parse(marketCapString) * 1000000;
                    InstrumentData data = new InstrumentData()
                    {
                        InstrumentId = instrument.Id,
                        Index = indexName,
                        SnapshotTime = snapshotTime,
                        MarketCap = marketCap
                    };
                    if (values.TryGetValue("PE ratio (TTM)", out string val))
                    {
                        if (double.TryParse(val, out double peRatio))
                        {
                            data.PERatio = peRatio;
                        }
                    }
                    if (values.TryGetValue("Beta (5Y monthly)", out val))
                    {
                        if (double.TryParse(val, out double beta))
                        {
                            data.Beta_5YMonth = beta;
                        }
                    }
                    if (values.TryGetValue("EPS (TTM)", out val))
                    {
                        if (double.TryParse(val, out double eps))
                        {
                            data.EPS = eps;
                        }
                    }
                    if (values.TryGetValue("Market cap", out val))
                    {
                        int multiplier = 1;
                        if (val.EndsWith('B'))
                        {
                            multiplier = 1000000000;
                        }
                        if (val.EndsWith('M'))
                        {
                            multiplier = 1000000;
                        }

                        if (multiplier != 1)
                        {
                            val = val.Substring(0, val.Length - 1);
                        }
                        if (double.TryParse(val, out double newMarketCap))
                        {
                            newMarketCap *= multiplier;
                            if (marketCap != newMarketCap)
                            {
                                logger?.Warning("dataLoader", $"Instrument {instrument.Name}. Received {marketCap} and {newMarketCap} for market cap.");
                            }
                        }
                    }
                    if (values.TryGetValue("Forward dividend &amp; yield", out val))
                    {
                        int dividendEnd = val.IndexOf('(');
                        string dividend = val.Substring(0, dividendEnd);
                        if (double.TryParse(dividend, out double price))
                        {
                            data.ForwardDividend = price;
                        }

                        string yield = val.Substring(dividendEnd + 1).Trim(')').Trim('%');
                        if (double.TryParse(yield, out price))
                        {
                            data.ForwardYield = price / 100;
                        }
                    }
                    if (values.TryGetValue("Avg. volume", out val))
                    {
                        if (double.TryParse(val, out double avVol))
                        {
                            data.AverageVolume = avVol;
                        }
                    }

                    instrumentDataList.Add(data);
                }
            }

            context.InstrumentData.AddRange(instrumentDataList);
            int numberChanges = context.SaveChanges();

            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(), $"Added {numberChanges} into database.");
            return numberChanges;
        }

        public static async Task<int> UpdateInstrumentData(
            StockExchangeDbContext context,
            IList<string> instrumentData,
            IList<Instrument> indexRemovedInstruments,
            IReportLogger logger = null)
        {
            var snapshotTime = DateTime.Now;
            var instrumentDataList = new List<InstrumentData>();
            foreach (string instrumentStrings in instrumentData)
            {
                string[] instValues = instrumentStrings.Split(' ');
                var instrument = context.Instruments.SingleOrDefault(inst => inst.Company == instValues[0]);
                var data = context.InstrumentData.GetLatest(instrument.Id);
                if (instrument == null)
                {
                    continue;
                }

                var values = await GetExtraData(instrument, logger);
                if (instValues.Length > 7)
                {

                    string marketCapString = instValues[^4];
                    var dict = new Dictionary<string, double>
                    {
                        ["Market cap"] = double.Parse(marketCapString) * 1000000
                    };
                    if (values.TryGetValue("PE ratio (TTM)", out string val))
                    {
                        if (double.TryParse(val, out double peRatio))
                        {
                            dict["PE ratio (TTM)"] = peRatio;
                        }
                    }
                    if (values.TryGetValue("Beta (5Y monthly)", out val))
                    {
                        if (double.TryParse(val, out double beta))
                        {
                            dict["Beta (5Y monthly)"] = beta;
                        }
                    }
                    if (values.TryGetValue("EPS (TTM)", out val))
                    {
                        if (double.TryParse(val, out double eps))
                        {
                            dict["EPS (TTM)"] = eps;
                        }
                    }
                    if (values.TryGetValue("Market cap", out val))
                    {
                        int multiplier = 1;
                        if (val.EndsWith('B'))
                        {
                            multiplier = 1000000000;
                        }
                        if (val.EndsWith('M'))
                        {
                            multiplier = 1000000;
                        }

                        if (multiplier != 1)
                        {
                            val = val.Substring(0, val.Length - 1);
                        }
                        if (double.TryParse(val, out double newMarketCap))
                        {
                            newMarketCap *= multiplier;
                            if (dict["Market cap"] != newMarketCap)
                            {
                                logger?.Warning("dataLoader", $"Instrument {instrument.Name}. Received {dict["Market cap"]} and {newMarketCap} for market cap.");
                            }
                        }
                    }
                    if (values.TryGetValue("Forward dividend &amp; yield", out val))
                    {
                        int dividendEnd = val.IndexOf('(');
                        string dividend = val.Substring(0, dividendEnd);
                        if (double.TryParse(dividend, out double forwardDividend))
                        {
                            dict["Forward dividend"] = forwardDividend;
                        }

                        string yield = val.Substring(dividendEnd + 1).Trim(')').Trim('%');
                        if (double.TryParse(yield, out double forwardYield))
                        {
                            dict["yield"] = forwardYield / 100;
                        }
                    }
                    if (values.TryGetValue("Avg. volume", out val))
                    {
                        if (double.TryParse(val, out double avVol))
                        {
                            dict["Avg. volume"] = avVol;
                        }
                    }

                    if (Differ(data, dict))
                    {
                        var newData = new InstrumentData(data)
                        {
                            SnapshotTime = snapshotTime,
                        };
                        if (dict.TryGetValue("Market cap", out double marketCap))
                        {
                            newData.MarketCap = marketCap;
                        }
                        if (dict.TryGetValue("PE ratio (TTM)", out double peRatio))
                        {
                            newData.PERatio = peRatio;
                        }
                        if (dict.TryGetValue("Beta (5Y monthly)", out double beta))
                        {
                            newData.Beta_5YMonth = beta;
                        }
                        if (dict.TryGetValue("EPS (TTM)", out double eps))
                        {
                            newData.EPS = eps;
                        }
                        if (dict.TryGetValue("Forward dividend", out double forwardDividend))
                        {
                            newData.ForwardDividend = forwardDividend;
                        }
                        if (dict.TryGetValue("yield", out double yield))
                        {
                            newData.ForwardYield = yield;
                        }
                        if (dict.TryGetValue("Av. Volume", out double avVol))
                        {
                            newData.AverageVolume = avVol;
                        }
                        instrumentDataList.Add(newData);
                    }
                }
            }

            foreach (var instrument in indexRemovedInstruments)
            {
                InstrumentData data = context.InstrumentData.GetLatest(instrument.Id);
                var newData = new InstrumentData(data)
                {
                    SnapshotTime = snapshotTime,
                    Index = null
                };
                instrumentDataList.Add(newData);
            }

            context.InstrumentData.AddRange(instrumentDataList);
            int numberChanges = context.SaveChanges();

            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(), $"Added {numberChanges} into database.");
            return numberChanges;
        }

        public static bool Differ(InstrumentData data, Dictionary<string, double> dict)
        {
            bool valuesDiffer = false;
            if (dict.TryGetValue("Market cap", out double marketCap))
            {
                valuesDiffer = !data.MarketCap.Equals(marketCap);
            }
            if (dict.TryGetValue("PE ratio (TTM)", out double peRatio))
            {
                valuesDiffer = !data.PERatio.Equals(peRatio);
            }
            if (dict.TryGetValue("Beta (5Y monthly)", out double beta))
            {
                valuesDiffer = !data.Beta_5YMonth.Equals(beta);
            }
            if (dict.TryGetValue("EPS (TTM)", out double eps))
            {
                valuesDiffer = !data.EPS.Equals(eps);
            }
            if (dict.TryGetValue("Forward dividend", out double forwardDividend))
            {
                valuesDiffer = !data.ForwardDividend.Equals(forwardDividend);
            }
            if (dict.TryGetValue("yield", out double yield))
            {
                valuesDiffer = !data.ForwardYield.Equals(yield);
            }
            if (dict.TryGetValue("Av. Volume", out double avVol))
            {
                valuesDiffer = !data.AverageVolume.Equals(avVol);
            }
            return valuesDiffer;
        }
    }
}