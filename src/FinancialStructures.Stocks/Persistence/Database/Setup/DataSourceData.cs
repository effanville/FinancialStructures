using Effanville.FinancialStructures.Stocks.Persistence.Database.Models;

namespace Effanville.FinancialStructures.Stocks.Persistence.Database.Setup
{
    internal static class DataSourceData
    {
        public static int Configure(StockExchangeDbContext context)
        {
            var dataSource = new DataSource() { Name = "Yahoo", BaseUrl = "https://uk.finance.yahoo.com/quote/" };
            context.DataSources.AddIfNotExists(
                dataSource,
                otherEntity => dataSource.Name == otherEntity.Name);
            int numberChanges = context.SaveChanges();
            return numberChanges;
        }
    }
}