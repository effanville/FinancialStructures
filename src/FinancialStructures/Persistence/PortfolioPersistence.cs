using System.IO.Abstractions;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;

namespace Effanville.FinancialStructures.Persistence
{
    public sealed class PortfolioPersistence : IPersistence<IPortfolio>
    {
        private readonly XmlPortfolioPersistence _xmlPortfolioPersistence;
        private readonly BinaryFilePortfolioPersistence _binaryFilePortfolioPersistence;

        public PortfolioPersistence(IReportLogger logger)
        {
            _xmlPortfolioPersistence = new XmlPortfolioPersistence(logger);
            _binaryFilePortfolioPersistence = new BinaryFilePortfolioPersistence(logger);
        }

        public IPortfolio Load(PersistenceOptions options)
        {
            Portfolio portfolio = new Portfolio();
            if (!Load(portfolio, options))
            {
                return null;
            }

            return portfolio;
        }

        public bool Load(IPortfolio portfolio, PersistenceOptions options)
            => options switch
            {
                XmlFilePersistenceOptions xmlOptions => _xmlPortfolioPersistence.Load(portfolio, xmlOptions),
                BinaryFilePersistenceOptions binaryOptions => _binaryFilePortfolioPersistence.Load(portfolio, binaryOptions),
                _ => false
            };

        public bool Save(IPortfolio portfolio, PersistenceOptions options)
            => options switch
            {
                XmlFilePersistenceOptions xmlOptions => _xmlPortfolioPersistence.Save(portfolio, xmlOptions),
                BinaryFilePersistenceOptions binaryOptions => _binaryFilePortfolioPersistence.Save(portfolio, binaryOptions),
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