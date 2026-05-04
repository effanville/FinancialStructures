using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Effanville.Common.Structure.Reporting;
using Effanville.Common.Structure.WebAccess;
using Effanville.FinancialStructures.Stocks.Download;
using Effanville.FinancialStructures.Stocks.HistoricalRepository;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Effanville.FinancialStructures.Stocks.Tests.HistoricalRepository
{
    [TestFixture]
    public class HistoricalMarketsBuilderTests
    {
        [Test]
        public async Task SetupDbFromWeb()
        {
            var logger = new LogReporter(null, true);
            var fileSystem = new FileSystem();
            var loggerFactory = new LoggerFactory();
            var fundamentalDownloader = new FundamentalDataDownloader(loggerFactory, Substitute.For<ILogger<FundamentalDataDownloader>>());
            IStockDownloaderFactory priceDownloaderFactory = new StockPriceDownloaderFactory(loggerFactory, new WebDownloader(null));
            var historicalMarketsBuilder = new HistoricalMarketsBuilder(
                priceDownloaderFactory,
                new StockDataParser(fundamentalDownloader, Substitute.For<ILogger<StockDataParser>>()),
                new InstrumentDownloader(Substitute.For<ILogger<InstrumentDownloader>>()),
                Substitute.For<ILogger<HistoricalMarketsBuilder>>(),
                loggerFactory)
                .WithExchangesFromFile("ExampleConfigFiles/Exchanges.csv", fileSystem);
            _ = await historicalMarketsBuilder.WithIndexInstruments("FTSE-100");
            _ = await historicalMarketsBuilder.WithInstrumentPriceData(
                new DateTime(2025, 1, 1),
                DateTime.Today);
            var markets = historicalMarketsBuilder.GetInstance();
            foreach (HistoricalExchange exchange in markets.Exchanges)
            {
                if (exchange.ExchangeIdentifier != "LSE")
                {
                    continue;
                }

                foreach (HistoricalStock instrument in exchange.Stocks)
                {
                    Assert.That(instrument.Fundamentals.Count, Is.Not.Zero);
                    Assert.That(instrument.Name.Count, Is.Not.Zero);
                    Assert.That(instrument.Valuations.Count, Is.Not.Zero, $"Instrument {instrument.Name.Last().Value.Ric} has no valuations.");
                }
            }
        }

        [Test]
        public async Task UpdateDbFromWeb()
        {
            // setup db with test data
            var logger = new LogReporter(null, true);
            var fileSystem = new FileSystem();
            var loggerFactory = new LoggerFactory();
            var fundamentalDownloader = new FundamentalDataDownloader(loggerFactory, Substitute.For<ILogger<FundamentalDataDownloader>>());
            IStockDownloaderFactory priceDownloaderFactory = new StockPriceDownloaderFactory(null, new WebDownloader(null));
            var historicalMarketsBuilder = new HistoricalMarketsBuilder(
                priceDownloaderFactory,
                new StockDataParser(fundamentalDownloader, Substitute.For<ILogger<StockDataParser>>()),
                new InstrumentDownloader(Substitute.For<ILogger<InstrumentDownloader>>()),
                Substitute.For<ILogger<HistoricalMarketsBuilder>>(),
                loggerFactory)
                .WithExchangesFromFile("ExampleConfigFiles/Exchanges.csv", fileSystem);
            _ = await historicalMarketsBuilder.WithIndexInstruments("FTSE-100");
            _ = await historicalMarketsBuilder.WithInstrumentPriceData(
                new DateTime(2020, 1, 1),
                new DateTime(2021, 1, 1));

            var markets = historicalMarketsBuilder.GetInstance();
            Dictionary<string, int> valuationRecords = new Dictionary<string, int>();
            foreach (HistoricalExchange exchange in markets.Exchanges)
            {
                if (exchange.ExchangeIdentifier != "LSE")
                {
                    continue;
                }

                foreach (var instrument in exchange.Stocks)
                {
                    valuationRecords.Add(instrument.Name.Last().Value.Ric, instrument.Valuations.Count);
                }
            }

            // now add more data

            _ = await historicalMarketsBuilder.UpdateIndexInstruments("FTSE-100");
            _ = await historicalMarketsBuilder.WithInstrumentPriceData(
                new DateTime(2020, 1, 1),
                DateTime.Today);

            foreach (HistoricalExchange exchange in markets.Exchanges)
            {
                if (exchange.ExchangeIdentifier != "LSE")
                {
                    continue;
                }

                foreach (var instrument in exchange.Stocks)
                {
                    Assert.That(instrument.Fundamentals.Count, Is.Not.Zero);
                    Assert.That(instrument.Name.Count, Is.Not.Zero);
                    Assert.That(instrument.Valuations.Count, Is.Not.Zero);
                    valuationRecords.TryGetValue(instrument.Name.Last().Value.Ric, out int previous);
                    Assert.That(
                        instrument.Valuations.Count,
                        Is.GreaterThan(previous),
                        $"Instrument {instrument.Name.Last().Value.Ric} has not added data.");
                }
            }
        }
    }
}