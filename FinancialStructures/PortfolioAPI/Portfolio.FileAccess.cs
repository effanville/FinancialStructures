using FileSupport;
using FinancialStructures.FinanceStructures;
using FinancialStructures.Database;
using SavingClasses;
using System;
using System.Collections.Generic;
using System.IO;

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
        public static List<Sector> LoadPortfolio(this Portfolio portfolio, string filePath, Action<string, string, string> reportLogger)
        {
            if (File.Exists(filePath))
            {
                var database = XmlFileAccess.ReadFromXmlFile<AllData>(filePath);
                portfolio.CopyData(database.MyFunds);
                return database.myBenchMarks;
            }

            reportLogger("Report", "Loading", "Loaded Empty New Database.");
            portfolio.CopyData(new Portfolio());
            return new List<Sector>();
        }

        /// <summary>
        /// Save database to xml file.
        /// </summary>
        /// <param name="portfolio">The database to save.</param>
        /// <param name="benchMarks">The associated sectors to save.</param>
        /// <param name="filePath">The path to save to.</param>
        /// <param name="reportLogger">Callback to report information.</param>
        public static void SavePortfolio(this Portfolio portfolio, List<Sector> benchMarks, string filePath, Action<string, string, string> reportLogger)
        {
            var toSave = new AllData(portfolio, benchMarks);
            if (filePath != null)
            {
                XmlFileAccess.WriteToXmlFile(filePath, toSave);
                reportLogger("Report", "Saving", $"Saved Database at {filePath}");
            }
        }
    }
}
