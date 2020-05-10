using FinancialStructures.Database;
using FinancialStructures.FileAccess;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.Reporting;
using FinancialStructures.SavingClasses;
using System.IO;
using System.Linq;

namespace FinancialStructures.PortfolioAPI
{
    public static class DatabaseAccess
    {
        /// <summary>
        /// Load database from xml file.
        /// </summary>
        /// <param name="portfolio">The database to load into..</param>
        /// <param name="filePath">The path to load from.</param>
        /// <param name="reportLogger">Callback to report information.</param>
        public static void LoadPortfolio(this IPortfolio portfolio, string filePath, IReportLogger reportLogger)
        {
            if (File.Exists(filePath))
            {
                var database = XmlFileAccess.ReadFromXmlFile<AllData>(filePath, out string error);
                if (database != null)
                {
                    portfolio.CopyData(database.MyFunds);
                    if (!database.MyFunds.BenchMarks.Any())
                    {
                        portfolio.SetBenchMarks(database.myBenchMarks);
                    }

                    portfolio.WireDataChangedEvents();
                    _ = reportLogger.Log(ReportSeverity.Critical, ReportType.Report, ReportLocation.Loading, $"Loaded new database from {filePath}");
                }
                else
                {
                    _ = reportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, $" Failed to load new database from {filePath}. {error}.");
                }

                return;
            }

            _ = reportLogger.Log(ReportSeverity.Critical, ReportType.Report, ReportLocation.Loading, "Loaded Empty New Database.");
            portfolio.CopyData(new Portfolio());
        }

        /// <summary>
        /// Save database to xml file.
        /// </summary>
        /// <param name="portfolio">The database to save.</param>
        /// <param name="filePath">The path to save to.</param>
        /// <param name="reportLogger">Callback to report information.</param>
        public static void SavePortfolio(this IPortfolio portfolio, string filePath, IReportLogger reportLogger)
        {
            var toSave = new AllData(portfolio, portfolio.BenchMarks);

            if (filePath != null)
            {
                XmlFileAccess.WriteToXmlFile(filePath, toSave, out string error);
                if (error != null)
                {
                    _ = reportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Saving, $"Failed to save database: {error}");
                    return;
                }

                _ = reportLogger.Log(ReportSeverity.Critical, ReportType.Report, ReportLocation.Saving, $"Saved Database at {filePath}");
            }
        }
    }
}
