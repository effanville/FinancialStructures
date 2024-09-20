using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Stocks.HistoricalRepository;

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
            var historicalMarketsBuilder = new HistoricalMarketsBuilder()
                .WithExchangesFromFile("ExampleConfigFiles/Exchanges.csv", fileSystem, logger);
            _ = await historicalMarketsBuilder.WithIndexInstruments("FTSE-100", logger);
            _ = await historicalMarketsBuilder.WithInstrumentPriceData(
                new DateTime(2020, 1, 1),
                DateTime.Today,
                logger);
            var markets = historicalMarketsBuilder.GetInstance();
            foreach (HistoricalExchange exchange in markets.Exchanges)
            {
                if (exchange.ExchangeIdentifier != "LSE")
                {
                    continue;
                }

                foreach (HistoricalStock instrument in exchange.Stocks)
                {
                    Assert.NotZero(instrument.Fundamentals.Count);
                    Assert.NotZero(instrument.Name.Count);
                    Assert.NotZero(instrument.Valuations.Count, $"Instrument {instrument.Name.Last().Value.Ric} has no valuations.");
                }
            }
        }

        [Test]
        public async Task UpdateDbFromWeb()
        {
            // setup db with test data
            var logger = new LogReporter(null, true);
            var fileSystem = new FileSystem();
            var historicalMarketsBuilder = new HistoricalMarketsBuilder()
                .WithExchangesFromFile("ExampleConfigFiles/Exchanges.csv", fileSystem, logger);
            _ = await historicalMarketsBuilder.WithIndexInstruments("FTSE-100", logger);
            _ = await historicalMarketsBuilder.WithInstrumentPriceData(
                new DateTime(2020, 1, 1),
                new DateTime(2021, 1, 1),
                logger);

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

            _ = await historicalMarketsBuilder.UpdateIndexInstruments("FTSE-100", logger);
            _ = await historicalMarketsBuilder.WithInstrumentPriceData(
                new DateTime(2020, 1, 1),
                DateTime.Today,
                logger);

            foreach (HistoricalExchange exchange in markets.Exchanges)
            {
                if (exchange.ExchangeIdentifier != "LSE")
                {
                    continue;
                }

                foreach (var instrument in exchange.Stocks)
                {
                    Assert.NotZero(instrument.Fundamentals.Count);
                    Assert.NotZero(instrument.Name.Count);
                    Assert.NotZero(instrument.Valuations.Count);
                    valuationRecords.TryGetValue(instrument.Name.Last().Value.Ric, out int previous);
                    Assert.Greater(
                        instrument.Valuations.Count,
                        previous,
                        $"Instrument {instrument.Name.Last().Value.Ric} has not added data.");
                }
            }
        }
    }
}