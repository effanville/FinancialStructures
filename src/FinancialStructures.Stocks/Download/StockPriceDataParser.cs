using System;
using System.Linq;
using System.Threading.Tasks;

using Effanville.Common.Structure.Reporting;

using FinancialStructures.Stocks.HistoricalRepository;
using FinancialStructures.Stocks.Implementation;

namespace FinancialStructures.Stocks.Download
{
    public static class StockPriceDataParser
    {
        public async static Task<int> Populate(HistoricalMarkets context, DateTime startDate, DateTime endDate,
            IReportLogger logger = null)
        {
            int numberChanges = 0;
            foreach (var exchange in context.Exchanges)
            {
                foreach (HistoricalStock historicalStock in exchange.Stocks)
                {
                    string url = historicalStock.Name.Last().Value.Url;
                    var downloader = StockPriceDownloaderFactory.Retrieve(url);
                    IStock tempDataHolder = null;
                    string code = downloader.GetFinancialCode(url);
                    int numRetries = 0;
                    while ((tempDataHolder == null || tempDataHolder.Valuations.Count == 0) && numRetries < 10)
                    {
                        if (await downloader.TryGetFullPriceHistory(code, startDate, endDate, TimeSpan.FromDays(1),
                                value => tempDataHolder = value, logger))
                        {
                            break;
                        }

                        numRetries++;
                    }

                    if (numRetries == 10)
                    {
                        continue;
                    }
                    
                    if(tempDataHolder== null)
                    {
                        continue;
                    }

                    foreach (var valuation in tempDataHolder.Valuations)
                    {
                        var start = valuation.Start.Add(exchange.ExchangeOpen.ToTimeSpan());
                        var end = valuation.Start.Add(exchange.ExchangeClose.ToTimeSpan());
                        if (historicalStock.Valuations
                            .Any(x => x.Start == start
                                      && x.End == end))
                        {
                            continue;
                        }

                        historicalStock.Valuations.Add(new StockDay()
                        {
                            Start = start,
                            Duration = end - start,
                            Open = Convert.ToDecimal(valuation.Open),
                            High = Convert.ToDecimal(valuation.High),
                            Low = Convert.ToDecimal(valuation.Low),
                            Close = Convert.ToDecimal(valuation.Close),
                            Volume = Convert.ToDecimal(valuation.Volume),
                        });
                        numberChanges++;
                    }
                }
            }

            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(),
                $"Added {numberChanges} into database.");
            return numberChanges;
        }
    }
}