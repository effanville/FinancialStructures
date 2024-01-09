using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using Common.Structure.Reporting;

namespace FinancialStructures.Stocks.HistoricalRepository
{
    public sealed class HistoricalMarkets
    {
        public List<HistoricalExchange> Exchanges { get; } = new();
        
        public static HistoricalMarkets Create(string stockFilePath, IFileSystem fileSystem, IReportLogger logger = null)
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

            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(), $"Configured StockExchanges from file {stockFilePath}.");
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
            
            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(), $"Added {numberChanges} exchanges into database.");
            return historicalMarkets;
        }
    }
}