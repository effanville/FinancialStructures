using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Stocks.Persistence.Database.Models;

namespace Effanville.FinancialStructures.Stocks.Persistence.Database.Setup
{
    public static class ExchangeData
    {
        public static void Configure(StockExchangeDbContext context, string stockFilePath, IFileSystem fileSystem, IReportLogger logger = null)
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
                return;
            }

            Configure(context, fileContents);

            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(), $"Configured StockExchanges from file {stockFilePath}.");
        }

        public static void Configure(StockExchangeDbContext context, string[] exchangeData, IReportLogger logger = null)
        {
            var exchanges = new List<Exchange>();
            foreach (string line in exchangeData)
            {
                string[] inputs = line.Split(',');
                if (inputs.Length == 6)
                {
                    exchanges.Add(new Exchange()
                    {
                        ExchangeIdentifier = inputs[0].Trim(),
                        Name = inputs[1].Trim(),
                        TimeZone = inputs[2].Trim(),
                        CountryCode = inputs[3].Trim(),
                        ExchangeOpen = TimeOnly.Parse(inputs[4].Trim()),
                        ExchangeClose = TimeOnly.Parse(inputs[5].Trim())
                    });
                }
            }

            context.Exchanges.AddRange(exchanges);
            int numberChanges = context.SaveChanges();

            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(), $"Added {numberChanges} into database.");
        }
    }
}