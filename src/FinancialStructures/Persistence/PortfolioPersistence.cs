using System.IO.Abstractions;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;

namespace Effanville.FinancialStructures.Persistence
{
    public sealed class PortfolioPersistence : IPersistence<IPortfolio>
    {
        public IPortfolio Load(PersistenceOptions options, IReportLogger reportLogger = null)
        {
            Portfolio portfolio = new Portfolio();
            if (!Load(portfolio, options, reportLogger))
            {
                return null;
            }

            return portfolio;
        }

        public bool Load(IPortfolio portfolio, PersistenceOptions options, IReportLogger reportLogger = null) 
            => options switch
            {
                XmlFilePersistenceOptions xmlOptions => new XmlPortfolioPersistence().Load(portfolio, xmlOptions,
                    reportLogger),
                BinaryFilePersistenceOptions binaryOptions => new BinaryFilePortfolioPersistence().Load(portfolio,
                    binaryOptions, reportLogger),
                _ => false
            };

        public bool Save(IPortfolio portfolio, PersistenceOptions options, IReportLogger reportLogger = null) 
            => options switch
            {
                XmlFilePersistenceOptions xmlOptions => new XmlPortfolioPersistence().Save(portfolio, xmlOptions,
                    reportLogger),
                BinaryFilePersistenceOptions binaryOptions =>
                    new BinaryFilePortfolioPersistence().Save(portfolio, binaryOptions, reportLogger),
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