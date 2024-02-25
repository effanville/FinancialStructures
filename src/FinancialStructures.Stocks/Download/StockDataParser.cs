using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Effanville.Common.Structure.Reporting;

using FinancialStructures.Stocks.HistoricalRepository;
using FinancialStructures.Stocks.Implementation;

namespace FinancialStructures.Stocks.Download
{
    public static class StockDataParser
    {
        public static int ConfigureInstruments(
            HistoricalMarkets context,
            string indexName,
            IEnumerable<string> instrumentData,
            out List<HistoricalStock> removedInstruments,
            IReportLogger logger = null)
        {
            DateTime validFrom = DateTime.Now;
            List<HistoricalStock> currentIndexInstruments = new();
            foreach (var exchange in context.Exchanges)
            {
                var historicalStocks = exchange.Stocks
                    .Where(st => string.Equals(indexName, st.Fundamentals.Last().Value.Index));
                currentIndexInstruments.AddRange(historicalStocks);
            }

            removedInstruments = new List<HistoricalStock>(currentIndexInstruments);
            int numberAlterations = 0;
            foreach (string line in instrumentData)
            {
                string[] inputs = line.Split(',');
                if (inputs.Length == 7)
                {
                    HistoricalExchange exchange = context.Exchanges.Single(ex => ex.ExchangeIdentifier == inputs[4]);
                    if (exchange.TryGetStock(inputs[0], inputs[2], out HistoricalStock inst))
                    {
                        inst.UpdateName(validFrom, line, ',');
                        _ = removedInstruments.Remove(inst);
                        continue;
                    }

                    exchange.Stocks.Add(HistoricalStock.FromString(validFrom, line, ','));
                    numberAlterations++;
                }
                else
                {
                    string[] tabSepInputs = line.Split('\t');
                    if (tabSepInputs.Length == 7)
                    {
                        string ric = $"{tabSepInputs[0].Trim('.')}.L";
                        string name = tabSepInputs[0].Replace(".L", "");
                        var exchange2 = context.Exchanges.Single(ex => ex.ExchangeIdentifier == "LSE");
                        if (exchange2.TryGetStock(ric, name, out HistoricalStock inst2))
                        {
                            inst2.UpdateName(validFrom, line, '\t');
                            _ = removedInstruments.Remove(inst2);
                            continue;
                        }

                        exchange2.Stocks.Add(HistoricalStock.FromString(validFrom, line, '\t'));
                        numberAlterations++;
                    }
                    else
                    {
                        string[] spaceSepInputs = line.Split(" ");
                        if (spaceSepInputs.Length <= 7)
                        {
                            continue;
                        }

                        string ric = $"{spaceSepInputs[0].Trim('.').Replace('.', '-')}.L";
                        string[] nameArray = spaceSepInputs.Skip(1).SkipLast(5).ToArray();
                        string name = string.Join(" ", nameArray);
                        var exchange2 = context.Exchanges.Single(ex => ex.ExchangeIdentifier == "LSE");
                        if (exchange2.TryGetStock(ric, name, out HistoricalStock inst2))
                        {
                            inst2.UpdateName(validFrom, line, ' ');
                            _ = removedInstruments.Remove(inst2);
                            continue;
                        }

                        exchange2.Stocks.Add(HistoricalStock.FromString(validFrom, line, ' '));
                        numberAlterations++;
                    }
                }
            }

            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(),
                $"Made {numberAlterations} alterations.");
            return numberAlterations;
        }

        public static async Task<int> InsertInstrumentData(
            HistoricalMarkets context,
            string indexName,
            IList<string> instrumentData,
            IReportLogger logger = null)
        {
            var validFrom = DateTime.Now;

            HistoricalExchange exchange = context.Exchanges.Single(ex => ex.ExchangeIdentifier == "LSE");

            int numberAdditions = 0;
            foreach (string instrumentStrings in instrumentData)
            {
                string[] instValues = instrumentStrings.Split(' ');
                if (instValues.Length > 7)
                {
                    string ric = $"{instValues[0].Trim('.').Replace('.', '-')}.L";
                    string[] nameArray = instValues.Skip(1).SkipLast(5).ToArray();
                    string name = string.Join(" ", nameArray);

                    if (!exchange.TryGetStock(ric, name, out HistoricalStock inst))
                    {
                        continue;
                    }

                    var values = await FundamentalDataDownloader.GetExtraData(inst.Name.Last().Value.Url, logger);

                    string marketCapString = instValues[^4];
                    double marketCap = double.Parse(marketCapString) * 1000000;
                    StockFundamentalData data = new StockFundamentalData() { Index = indexName, MarketCap = marketCap };
                    if (values.TryGetDoubleValue("PE ratio (TTM)", out double peRatio))
                    {
                        data.PeRatio = peRatio;
                    }

                    if (values.TryGetDoubleValue("Beta (5Y monthly)", out double beta))
                    {
                        data.Beta5YearMonth = beta;
                    }

                    if (values.TryGetDoubleValue("Avg. volume", out double eps))
                    {
                        data.EPS = eps;
                    }

                    if (values.TryGetValue("Market cap", out string val))
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
                            if (Math.Abs(marketCap - newMarketCap) > 1e-8)
                            {
                                logger?.Warning("dataLoader",
                                    $"Instrument {inst.Name.Last().Value.Ric}. Received {marketCap} and {newMarketCap} for market cap.");
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

                    if (values.TryGetDoubleValue("Avg. volume", out double avVol))
                    {
                        data.AverageVolume = avVol;
                    }

                    numberAdditions++;
                    inst.Fundamentals.Add(validFrom, data);
                }
            }

            return numberAdditions;
        }

        private static bool TryGetDoubleValue(this Dictionary<string, string> map, string key, out double value,
            double defaultValue = double.NaN)
        {
            if (map.TryGetValue(key, out string val))
            {
                if (double.TryParse(val, out double eps))
                {
                    value = eps;
                    return true;
                }
            }

            value = defaultValue;
            return false;
        }

        public static async Task<int> UpdateInstrumentData(
            HistoricalMarkets context,
            string indexName,
            IList<string> instrumentData,
            IList<HistoricalStock> indexRemovedInstruments,
            IReportLogger logger = null)
        {
            var validFrom = DateTime.Now;
            int numberChanges = 0;
            HistoricalExchange exchange = context.Exchanges.Single(ex => ex.ExchangeIdentifier == "LSE");

            foreach (string instrumentStrings in instrumentData)
            {
                string[] instValues = instrumentStrings.Split(' ');
                if (instValues.Length <= 7)
                {
                    continue;
                }

                string ric = $"{instValues[0].Trim('.').Replace('.', '-')}.L";
                string[] nameArray = instValues.Skip(1).SkipLast(5).ToArray();
                string name = string.Join(" ", nameArray);

                if (!exchange.TryGetStock(ric, name, out HistoricalStock inst))
                {
                    continue;
                }

                if (!inst.Fundamentals.Any())
                {
                    numberChanges += await InsertInstrumentData(
                        context,
                        indexName,
                        new[] { instrumentStrings },
                        logger);
                }

                var data = inst.Fundamentals.Last().Value;

                var values = await FundamentalDataDownloader.GetExtraData(
                    inst.Name.Last().Value.Url,
                    logger);
                if (values == null)
                {
                    continue;
                }

                string marketCapString = instValues[^4];
                var dict = new Dictionary<string, double> { ["Market cap"] = double.Parse(marketCapString) * 1000000 };
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
                        if (Math.Abs(dict["Market cap"] - newMarketCap) > 1e-8)
                        {
                            logger?.Warning(
                                "dataLoader",
                                $"Instrument {inst.Name.Last().Value.Ric}. Received {dict["Market cap"]} and {newMarketCap} for market cap.");
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

                if (!data.Differs(dict))
                {
                    continue;
                }

                {
                    var newData = new StockFundamentalData(data);
                    if (dict.TryGetValue("Market cap", out double marketCap))
                    {
                        newData.MarketCap = marketCap;
                    }

                    if (dict.TryGetValue("PE ratio (TTM)", out double peRatio))
                    {
                        newData.PeRatio = peRatio;
                    }

                    if (dict.TryGetValue("Beta (5Y monthly)", out double beta))
                    {
                        newData.Beta5YearMonth = beta;
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

                    inst.Fundamentals.Add(validFrom, newData);
                    numberChanges++;
                }
            }

            if (indexRemovedInstruments == null)
            {
                return numberChanges;
            }

            foreach (var historicalStock in indexRemovedInstruments)
            {
                StockFundamentalData data = historicalStock.Fundamentals.Last().Value;
                var newData = new StockFundamentalData(data) { Index = null };
                historicalStock.Fundamentals.Add(validFrom, newData);
                numberChanges++;
            }

            logger?.Log(
                ReportType.Information,
                ReportLocation.AddingData.ToString(),
                $"Added {numberChanges} into database.");
            return numberChanges;
        }
    }
}