using System.IO.Abstractions;

using FinancialStructures.Stocks.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace FinancialStructures.Stocks.Persistence.Database
{
    public class StockExchangeDbContext : DbContext
    {
        public string DbPath { get; }
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<InstrumentData> InstrumentData { get; set; }
        public DbSet<InstrumentPriceData> InstrumentPrices { get; set; }

        public StockExchangeDbContext(IFileSystem fileSystem, string filePath)
        {
            DbPath = fileSystem.Path.GetFullPath(filePath);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<DataSource>()
                .HasIndex(b => b.Name)
                .IsUnique();
            _ = modelBuilder.Entity<Instrument>()
                .HasIndex(b => b.Ric)
                .IsUnique();
            _ = modelBuilder.Entity<Exchange>()
                .HasIndex(b => b.ExchangeIdentifier)
                .IsUnique();
            _ = modelBuilder.Entity<InstrumentData>()
                .HasIndex(b => new { b.InstrumentId, b.SnapshotTime })
                .IsUnique();
        }
    }
}