using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

using Common.Structure.Reporting;

using FinancialStructures.Stocks.Implementation;

namespace FinancialStructures.Stocks.HistoricalRepository
{
    public sealed class HistoricalMarkets
    {
        public List<HistoricalExchange> Exchanges { get; } = new();

        public static HistoricalMarkets Create(string stockFilePath, IFileSystem fileSystem,
            IReportLogger logger = null)
        {
            string[] fileContents = Array.Empty<string>();
            try
            {
                fileContents = fileSystem.File.ReadAllLines(stockFilePath);
            }
            catch (Exception ex)
            {
                logger?.Error("FileRead", $"Failed to read from file located at {stockFilePath}: {ex.Message}.");
            }

            if (fileContents.Length == 0)
            {
                logger?.Error("FileRead", "Nothing in file selected, but expected stock company, name, url data.");
                return null;
            }

            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(),
                $"Configured StockExchanges from file {stockFilePath}.");
            return Configure(fileContents, logger);
        }

        private static HistoricalMarkets Configure(string[] exchangeData, IReportLogger logger = null)
        {
            var historicalMarkets = new HistoricalMarkets();
            int numberChanges = 0;
            foreach (string line in exchangeData)
            {
                var exchange = HistoricalExchange.FromString(line);
                if (exchange != null)
                {
                    historicalMarkets.Exchanges.Add(exchange);
                    numberChanges++;
                }
            }

            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(),
                $"Added {numberChanges} exchanges into database.");
            return historicalMarkets;
        }

        public IStockExchange CreateExchangeSnapshot(DateTime snapshotTime, string exchangeCode)
        {
            var historicalExchange = Exchanges.FirstOrDefault(exc => exc.ExchangeIdentifier == exchangeCode);
            if (historicalExchange == null)
            {
                return null;
            }

            var stockExchange = new StockExchange
            {
                ExchangeIdentifier = historicalExchange.ExchangeIdentifier,
                Name = historicalExchange.Name,
                CountryDateCode = historicalExchange.CountryDateCode,
                TimeZone = historicalExchange.TimeZone,
                ExchangeOpen = historicalExchange.ExchangeOpen,
                ExchangeClose = historicalExchange.ExchangeClose
            };
            foreach (HistoricalStock historicalStock in historicalExchange.Stocks)
            {
                var validFrom = historicalStock.EarliestValidity();
                if (validFrom > snapshotTime)
                {
                    continue;
                }

                var stock = new Stock
                {
                    Name = historicalStock.ValidName(snapshotTime),
                    Fundamentals = historicalStock.ValidFundamentals(snapshotTime)
                };
                foreach (var valuation in historicalStock.Valuations)
                {
                    if (valuation.End >= snapshotTime)
                    {
                        break;
                    }

                    var stockDay = new StockDay(
                        valuation.Start,
                        valuation.Open,
                        valuation.High,
                        valuation.Low,
                        valuation.Close,
                        valuation.Volume,
                        valuation.Duration);
                    stock.Valuations.Add(stockDay);
                }
                stockExchange.Stocks.Add(stock);
            }

            return stockExchange;
        }
    }
}