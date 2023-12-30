using Common.Structure.Reporting;

using FinancialStructures.Stocks.Persistence.Database.Models;

namespace FinancialStructures.Stocks.Persistence.Database.Setup
{
    internal static class DataSourceData
    {
        public static void Configure(StockExchangeDbContext context, IReportLogger logger = null)
        {
            var dataSource = new DataSource() { Name = "Yahoo", BaseUrl = "https://uk.finance.yahoo.com/quote/" };
            context.DataSources.AddIfNotExists(
                dataSource,
                otherEntity => dataSource.Name == otherEntity.Name);
            int numberChanges = context.SaveChanges();
            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(),
                $"Added {numberChanges} into database.");
        }
    }
}