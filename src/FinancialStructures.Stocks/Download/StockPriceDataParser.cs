using System;
using System.Linq;
using System.Threading.Tasks;

using Effanville.FinancialStructures.Stocks.HistoricalRepository;
using Effanville.FinancialStructures.Stocks.Implementation;
using Microsoft.Extensions.Logging;

namespace Effanville.FinancialStructures.Stocks.Download
{
    public class StockPriceDataParser
    {
        private readonly ILogger<StockPriceDataParser> _logger;
        private readonly IStockDownloaderFactory _stockDownloaderFactory;

        public StockPriceDataParser(IStockDownloaderFactory stockDownloaderFactory, ILogger<StockPriceDataParser> logger)
        {
            _stockDownloaderFactory = stockDownloaderFactory;
            _logger = logger;
        }

        public async Task<int> Populate(HistoricalMarkets context, DateTime startDate, DateTime endDate)
        {
            int numberChanges = 0;
            foreach (HistoricalExchange exchange in context.Exchanges)
            {
                foreach (HistoricalStock historicalStock in exchange.Stocks)
                {
                    string url = historicalStock.Name.Last().Value.Url;
                    IStockDownloader downloader = _stockDownloaderFactory.Retrieve(url);
                    IStock tempDataHolder = new Stock() { Name = new NamingStructures.NameData { Url = url } };
                    int numRetries = 0;
                    while ((tempDataHolder == null || tempDataHolder.Valuations.Count == 0) && numRetries < 10)
                    {
                        if (await downloader.TryGetFullPriceHistory(tempDataHolder, startDate, endDate))
                        {
                            break;
                        }

                        numRetries++;
                    }

                    if (numRetries == 10)
                    {
                        continue;
                    }

                    if (tempDataHolder == null)
                    {
                        continue;
                    }

                    foreach (StockDay valuation in tempDataHolder.Valuations)
                    {
                        DateTime start = valuation.Start.Add(exchange.ExchangeOpen.ToTimeSpan());
                        DateTime end = valuation.Start.Add(exchange.ExchangeClose.ToTimeSpan());
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

            _logger?.LogInformation($"Added {numberChanges} into database.");
            return numberChanges;
        }
    }
}