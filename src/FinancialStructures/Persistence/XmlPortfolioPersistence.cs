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
        {
            if (options is not XmlFilePersistenceOptions xmlOptions)
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(),
                    "Options for loading from Xml file not of correct type.");
                return false;
            }

            IFileSystem fileSystem = xmlOptions.FileSystem;
            string filePath = xmlOptions.FilePath;
            if (!fileSystem.File.Exists(filePath))
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(),
                    "Loaded Empty New StockExchange.");
                return false;
            }

            if (portfolio is not Portfolio portfolioImpl)
            {
                return false;
            }

            AllData database = XmlFileAccess.ReadFromXmlFile<AllData>(fileSystem, filePath, out string error);
            if (database != null)
            {
                portfolioImpl.Clear();
                database.MyFunds.Set(portfolioImpl);

                if (!database.MyFunds.BenchMarks.Any())
                {
                    foreach (var benchmark in database.myBenchMarks)
                    {
                        portfolioImpl.AddBenchMark(new Sector(benchmark.Names, benchmark.Values));
                    }
                }

                portfolioImpl.WireDataChangedEvents();
                portfolioImpl.Name = fileSystem.Path.GetFileNameWithoutExtension(filePath);
                portfolioImpl.Saving();
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Loading,
                    $"Loaded new database from {filePath}");
            }
            else
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading,
                    $" Failed to load new database from {filePath}. {error}.");
            }

            foreach (Security sec in portfolio.Funds)
            {
                sec.EnsureOnLoadDataConsistency();
            }

            portfolioImpl.OnNewPortfolio(this, new PortfolioEventArgs( true));
            return true;
        }

        public bool Save(IPortfolio portfolio, PersistenceOptions options, IReportLogger reportLogger = null)
        {
            if (options is not XmlFilePersistenceOptions xmlOptions)
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(),
                    "Options for loading from Xml file not of correct type.");
                return false;
            }

            IFileSystem fileSystem = xmlOptions.FileSystem;
            string filePath = xmlOptions.FilePath;
            if (portfolio is not Portfolio portfolioImpl)
            {
                reportLogger?.Log(ReportType.Error, ReportLocation.Saving.ToString(),
                    "Attempted to save a StockExchange that was not of the correct type.");
                return false;
            }

            AllData toSave = new AllData(portfolioImpl, null);

            XmlFileAccess.WriteToXmlFile(fileSystem, filePath, toSave, out string error);
            if (error != null)
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Saving,
                    $"Failed to save database: {error}");
                return false;
            }

            portfolioImpl.Saving();
            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Saving,
                $"Saved Database at {filePath}");
            return true;
        }
    }
}