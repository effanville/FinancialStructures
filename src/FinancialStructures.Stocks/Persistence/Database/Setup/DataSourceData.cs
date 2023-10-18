using System.Collections.Generic;
using System.Linq;

using Common.Structure.Reporting;

using FinancialStructures.Stocks.Persistence.Models;

namespace FinancialStructures.Stocks.Persistence.Database.Setup
{
    internal static class DataSourceData
    {
        public static void Configure(StockExchangeDbContext context, IReportLogger logger = null)
        {
            if (!context.DataSources.Any(v => v.Name == "Yahoo"))
            {
                var dataSources = new List<DataSource>
                {
                    new DataSource() { Name = "Yahoo", BaseUrl = "https://uk.finance.yahoo.com/quote/" }
                };
                context.DataSources.AddRange(dataSources);
                int numberChanges = context.SaveChanges();
                logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(), $"Added {numberChanges} into database.");
            }
        }
    }
}