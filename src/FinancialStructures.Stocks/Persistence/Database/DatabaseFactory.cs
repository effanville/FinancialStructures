using System.IO.Abstractions;

using Effanville.FinancialStructures.Stocks.Persistence.Database.Setup;

namespace Effanville.FinancialStructures.Stocks.Persistence.Database
{
    public class DatabaseFactory : IDatabaseFactory
    {
        public StockExchangeDbContext Create(IFileSystem fileSystem, string filePath)
            => new StockExchangeDbContext(fileSystem, filePath);
        
        public StockExchangeDbContext CreateDbContext(string[] args)
            => Create(new FileSystem(), "SampleDB.db");

        public DatabaseBuilder GetDbBuilder(IFileSystem fileSystem, string filePath)
        {
            var dbContext = new StockExchangeDbContext(fileSystem, filePath);
            return new DatabaseBuilder(dbContext);
        }
    }
}