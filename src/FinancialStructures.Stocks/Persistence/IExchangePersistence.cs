using System.IO.Abstractions;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Persistence;
using Effanville.FinancialStructures.Stocks.Implementation;

namespace Effanville.FinancialStructures.Stocks.Persistence
{
    public sealed class ExchangePersistence : IPersistence<IStockExchange>
    {
        public IStockExchange Load(PersistenceOptions options, IReportLogger reportLogger = null)
        {
            StockExchange stockExchange = new StockExchange();
            if (!Load(stockExchange, options, reportLogger))
            {
                return null;
            }

            return stockExchange;
        }

        public bool Load(IStockExchange stockExchange, PersistenceOptions options, IReportLogger reportLogger = null)
            => options switch
            {
                XmlFilePersistenceOptions xmlOptions => new XmlExchangePersistence().Load(stockExchange, xmlOptions,
                    reportLogger),
                SqlitePersistenceOptions sqliteOptions => new SqliteExchangePersistence().Load(stockExchange,
                    sqliteOptions, reportLogger),
                _ => false
            };

        public bool Save(IStockExchange stockExchange, PersistenceOptions options, IReportLogger reportLogger = null)
            => options switch
            {
                XmlFilePersistenceOptions xmlOptions => new XmlExchangePersistence().Save(stockExchange, xmlOptions,
                    reportLogger),
                SqlitePersistenceOptions binaryOptions =>
                    new SqliteExchangePersistence().Save(stockExchange, binaryOptions, reportLogger),
                _ => false
            };

        public static PersistenceOptions CreateOptions(string filePath, IFileSystem fileSystem)
        {
            string extension = fileSystem.Path.GetExtension(filePath);
            return extension switch
            {
                ".db" => new SqlitePersistenceOptions(filePath, fileSystem),
                ".bin" => new BinaryFilePersistenceOptions(filePath, fileSystem),
                _ => new XmlFilePersistenceOptions(filePath, fileSystem)
            };
        }
    }
}