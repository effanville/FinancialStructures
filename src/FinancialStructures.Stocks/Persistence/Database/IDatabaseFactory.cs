using System.IO.Abstractions;

using FinancialStructures.Stocks.Persistence.Database.Setup;

using Microsoft.EntityFrameworkCore.Design;

namespace FinancialStructures.Stocks.Persistence.Database
{
    public interface IDatabaseFactory : IDesignTimeDbContextFactory<StockExchangeDbContext>
    {
        StockExchangeDbContext Create(IFileSystem fileSystem, string filePath);

        DatabaseBuilder GetDbBuilder(IFileSystem fileSystem, string filePath);
    }
}