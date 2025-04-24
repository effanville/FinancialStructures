using System.IO.Abstractions;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Persistence;
using Effanville.FinancialStructures.Stocks.Implementation;

namespace Effanville.FinancialStructures.Stocks.Persistence
{
    public sealed class ExchangePersistence : IPersistence<IStockExchange>
    {
        private readonly XmlExchangePersistence _xmlExchangePersistence;
        private readonly SqliteExchangePersistence _sqliteExchangePersistence;

        public ExchangePersistence(IReportLogger logger)
        {
            _xmlExchangePersistence = new XmlExchangePersistence(logger);
            _sqliteExchangePersistence = new SqliteExchangePersistence(logger);
        }
        public IStockExchange Load(PersistenceOptions options)
        {
            StockExchange stockExchange = new StockExchange();
            if (!Load(stockExchange, options))
            {
                return null;
            }

            return stockExchange;
        }

        public bool Load(IStockExchange stockExchange, PersistenceOptions options)
            => options switch
            {
                XmlFilePersistenceOptions xmlOptions => _xmlExchangePersistence.Load(stockExchange, xmlOptions),
                SqlitePersistenceOptions sqliteOptions => _sqliteExchangePersistence.Load(stockExchange,
                    sqliteOptions),
                _ => false
            };

        public bool Save(IStockExchange stockExchange, PersistenceOptions options)
            => options switch
            {
                XmlFilePersistenceOptions xmlOptions => _xmlExchangePersistence.Save(stockExchange, xmlOptions),
                SqlitePersistenceOptions binaryOptions => _sqliteExchangePersistence.Save(stockExchange, binaryOptions),
                _ => false
            };

        public static PersistenceOptions CreateOptions(string filePath, IFileSystem fileSystem)
        {
            string extension = fileSystem.Path.GetExtension(filePath);
            return extension switch
            {
                ".db" => new SqlitePersistenceOptions(filePath, fileSystem, "1.0.0.0"),
                ".bin" => new BinaryFilePersistenceOptions(filePath, fileSystem, "1.0.0.0"),
                _ => new XmlFilePersistenceOptions(filePath, fileSystem, "1.0.0.0")
            };
        }
    }
}