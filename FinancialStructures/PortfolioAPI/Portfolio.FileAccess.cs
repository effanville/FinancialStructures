using FileSupport;
using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.ReportLogging;
using SavingClasses;
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
        /// <param name="benchMarks">The associated sectors to load into.</param>
        /// <param name="filePath">The path to load from.</param>
        /// <param name="reportLogger">Callback to report information.</param>
        public static void LoadPortfolio(this IPortfolio portfolio, string filePath, LogReporter reportLogger)
        {
            string error = null;
            if (File.Exists(filePath))
            {
                var database = XmlFileAccess.ReadFromXmlFile<AllData>(filePath, out error);
                if (database != null)
                {
                    portfolio.CopyData(database.MyFunds);
                    if (!database.MyFunds.BenchMarks.Any())
                    {
                        portfolio.SetBenchMarks(database.myBenchMarks);
                    }

                    reportLogger.LogDetailed("Critical", "Report", "Loading", $"Loaded new database from {filePath}");
                }
                else
                {
                    reportLogger.LogDetailed("Critical", "Error", "Loading", $" Failed to load new database from {filePath}. {error}.");
                }

                return;
            }

            reportLogger.LogDetailed("Critical", "Report", "Loading", "Loaded Empty New Database.");
            portfolio.CopyData(new Portfolio());
        }

        /// <summary>
        /// Save database to xml file.
        /// </summary>
        /// <param name="portfolio">The database to save.</param>
        /// <param name="benchMarks">The associated sectors to save.</param>
        /// <param name="filePath">The path to save to.</param>
        /// <param name="reportLogger">Callback to report information.</param>
        public static void SavePortfolio(this IPortfolio portfolio, string filePath, LogReporter reportLogger)
        {
            var toSave = new AllData(portfolio, portfolio.BenchMarks);

            if (filePath != null)
            {
                XmlFileAccess.WriteToXmlFile(filePath, toSave, out string error);
                if (error != null)
                {
                    reportLogger.LogDetailed("Critical", "Error", "Saving", $"Failed to save database: {error}");
                    return;
                }

                reportLogger.LogDetailed("Critical", "Report", "Saving", $"Saved Database at {filePath}");
            }
        }
    }
}
