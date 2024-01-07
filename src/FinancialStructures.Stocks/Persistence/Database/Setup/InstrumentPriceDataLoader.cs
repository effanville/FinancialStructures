using System;
using System.Linq;
using System.Threading.Tasks;

using Common.Structure.Reporting;

using FinancialStructures.Stocks.Download;
using FinancialStructures.Stocks.Persistence.Models;

namespace FinancialStructures.Stocks.Persistence.Database.Setup
{
    internal static class InstrumentPriceDataLoader
    {
        public async static Task Populate(StockExchangeDbContext context, DateTime startDate, DateTime endDate, IReportLogger logger = null)
        {
            DataSource dataSource = context.DataSources.Single(ex => ex.Name == "Yahoo");
            Exchange exchange = context.Exchanges.Single(ex => ex.ExchangeIdentifier == "LSE");
            foreach (var instrument in context.Instruments)
            {
                var downloader = StockPriceDownloaderFactory.Retrieve(instrument.Url);
                IStock tempDataHolder = null;
                string code = downloader.GetFinancialCode(instrument.Url);

                if (await downloader.TryGetFullPriceHistory(code, startDate, endDate, TimeSpan.FromDays(1), value => tempDataHolder = value, logger))
                {
                    foreach (var valuation in tempDataHolder.Valuations)
                    {
                        var start = valuation.Start.Add(exchange.ExchangeOpen.ToTimeSpan());
                        var end = valuation.Start.Add(exchange.ExchangeClose.ToTimeSpan());
                        _ = context.InstrumentPrices.Add(new InstrumentPriceData()
                        {
                            InstrumentId = instrument.Id,
                            DataSourceId = dataSource.Id,
                            StartTime = start,
                            EndTime = end,
                            Open = Convert.ToDouble(valuation.Open),
                            High = Convert.ToDouble(valuation.High),
                            Low = Convert.ToDouble(valuation.Low),
                            Close = Convert.ToDouble(valuation.Close),
                            Volume = Convert.ToDouble(valuation.Volume),
                        });
                    }
                }
            }

            int numberChanges = context.SaveChanges();
            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(), $"Added {numberChanges} into database.");
        }
    }
}