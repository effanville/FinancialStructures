using System.IO.Abstractions;

using Effanville.FinancialStructures.Stocks.Persistence.Database.Setup;
using Microsoft.Extensions.Logging;

namespace Effanville.FinancialStructures.Stocks.Persistence.Database
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public DatabaseFactory(ILoggerFactory loggerFactory) => _loggerFactory = loggerFactory;

        public StockExchangeDbContext Create(IFileSystem fileSystem, string filePath)
            => new StockExchangeDbContext(fileSystem, filePath);

        public StockExchangeDbContext CreateDbContext(string[] args)
            => Create(new FileSystem(), "SampleDB.db");

        public DatabaseBuilder GetDbBuilder(IFileSystem fileSystem, string filePath)
        {
            StockExchangeDbContext dbContext = new StockExchangeDbContext(fileSystem, filePath);
            return new DatabaseBuilder(_loggerFactory.CreateLogger<DatabaseBuilder>(), dbContext);
        }
    }
}