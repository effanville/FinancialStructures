using System.IO.Abstractions;
using System.Linq;

using Effanville.Common.Structure.FileAccess;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.Persistence.Xml;

namespace Effanville.FinancialStructures.Persistence
{
    public sealed class XmlPortfolioPersistence : IPersistence<IPortfolio>
    {
        private readonly IReportLogger _logger;

        public XmlPortfolioPersistence(IReportLogger logger) => _logger = logger;

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
        {
            if (options is not XmlFilePersistenceOptions xmlOptions)
            {
                _logger?.Info(nameof(XmlPortfolioPersistence), "Options for loading from Xml file not of correct type.");
                return false;
            }

            IFileSystem fileSystem = xmlOptions.FileSystem;
            string filePath = xmlOptions.FilePath;
            if (!fileSystem.File.Exists(filePath))
            {
                _logger?.Info(nameof(XmlPortfolioPersistence), "Loaded Empty New StockExchange.");
                return false;
            }

            if (portfolio is not Portfolio portfolioImpl)
            {
                return false;
            }

            if (LoadV2(portfolioImpl, options))
            {
                return true;
            }

            return LoadV1(portfolioImpl, options);
        }

        private bool LoadV2(Portfolio portfolio, PersistenceOptions options)
        {
            IFileSystem fileSystem = options.FileSystem;
            string filePath = options.FilePath;

            Xml.V2.AllData database = XmlFileAccess.ReadFromXmlFile<Xml.V2.AllData>(fileSystem, filePath, out string error);
            if (database != null)
            {
                portfolio.Clear();
                database.MyFunds.Set(portfolio);

                portfolio.WireDataChangedEvents();
                portfolio.Name = fileSystem.Path.GetFileNameWithoutExtension(filePath);
                portfolio.Saving();
                _logger?.Info(nameof(XmlPortfolioPersistence), $"Loaded new database from {filePath}");
            }
            else
            {
                if (options.Version == "2.0.0.0")
                {
                    _logger?.Error(nameof(XmlPortfolioPersistence), $" Failed to load new database with version {options.Version} from {filePath}. {error}.");
                }
                else
                {
                    _logger?.Debug(nameof(XmlPortfolioPersistence), $" Failed to load new database with version {options.Version} from {filePath}. {error}.");
                }

                return false;
            }

            foreach (Security sec in portfolio.Funds)
            {
                sec.EnsureOnLoadDataConsistency();
            }
            foreach (Security sec in portfolio.Pensions)
            {
                sec.EnsureOnLoadDataConsistency();
            }

            portfolio.OnNewPortfolio(this, new PortfolioEventArgs(true));
            return true;
        }

        private bool LoadV1(Portfolio portfolio, PersistenceOptions options)
        {
            IFileSystem fileSystem = options.FileSystem;
            string filePath = options.FilePath;

            AllData database = XmlFileAccess.ReadFromXmlFile<AllData>(fileSystem, filePath, out string error);
            if (database != null)
            {
                portfolio.Clear();
                database.MyFunds.Set(portfolio);

                if (!database.MyFunds.BenchMarks.Any())
                {
                    foreach (XmlSector benchmark in database.myBenchMarks)
                    {
                        portfolio.AddBenchMark(new Sector(benchmark.Names, benchmark.Values));
                    }
                }

                portfolio.WireDataChangedEvents();
                portfolio.Name = fileSystem.Path.GetFileNameWithoutExtension(filePath);
                portfolio.Saving();
                _logger?.Info(nameof(XmlPortfolioPersistence), $"Loaded new database from {filePath}");
            }
            else
            {
                _logger?.Error(nameof(XmlPortfolioPersistence), $" Failed to load new database from {filePath}. {error}.");
                return false;
            }

            foreach (Security sec in portfolio.Funds)
            {
                sec.EnsureOnLoadDataConsistency();
            }
            foreach (Security sec in portfolio.Pensions)
            {
                sec.EnsureOnLoadDataConsistency();
            }

            portfolio.OnNewPortfolio(this, new PortfolioEventArgs(true));
            return true;
        }

        public bool Save(IPortfolio portfolio, PersistenceOptions options)
        {
            if (options is not XmlFilePersistenceOptions xmlOptions)
            {
                _logger?.Info(nameof(XmlPortfolioPersistence), "Options for loading from Xml file not of correct type");
                return false;
            }

            if (portfolio is not Portfolio portfolioImpl)
            {
                _logger?.Error(nameof(XmlPortfolioPersistence), "Attempted to save a Portfolio that was not of the correct type");
                return false;
            }

            if (options.Version == "1.0.0.0")
            {
                return SaveV1(portfolioImpl, options);
            }
            else if (options.Version == "2.0.0.0")
            {
                return SaveV2(portfolioImpl, options);
            }

            _logger?.Error(nameof(XmlPortfolioPersistence), "Incorrect save version used");
            return false;
        }

        private bool SaveV2(Portfolio portfolio, PersistenceOptions options)
        {
            IFileSystem fileSystem = options.FileSystem;
            string filePath = options.FilePath;

            Xml.V2.AllData toSave = new Xml.V2.AllData(portfolio);

            XmlFileAccess.WriteToXmlFile(fileSystem, filePath, toSave, out string error);
            if (error != null)
            {
                _logger?.Info(nameof(XmlPortfolioPersistence), $"Failed to save database: {error}");
                return false;
            }

            portfolio.Saving();
            _logger?.Info(nameof(XmlPortfolioPersistence), $"Saved Database at {filePath}");
            return true;
        }

        private bool SaveV1(Portfolio portfolio, PersistenceOptions options)
        {
            IFileSystem fileSystem = options.FileSystem;
            string filePath = options.FilePath;

            AllData toSave = new AllData(portfolio, null);

            XmlFileAccess.WriteToXmlFile(fileSystem, filePath, toSave, out string error);
            if (error != null)
            {
                _logger?.Info(nameof(XmlPortfolioPersistence), $"Failed to save database: {error}");
                return false;
            }

            portfolio.Saving();
            _logger?.Info(nameof(XmlPortfolioPersistence), $"Saved Database at {filePath}");
            return true;
        }
    }
}