using FileSupport;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using SavingClasses;
using System.Collections.Generic;
using System.IO;

namespace FinancialStructures.Database
{
    public static class DatabaseAccess
    {
        public static List<NameData> GetSectorNames(List<Sector> sectors)
        {
            var outputs = new List<NameData>();
            if (sectors != null)
            {
                foreach (Sector thing in sectors)
                {
                    outputs.Add(new NameData(thing.GetName(), string.Empty, string.Empty, thing.GetUrl(), false));
                }
            }
            return outputs;
        }


        public static List<Sector> LoadPortfolio(this Portfolio portfolio, string filePath,  ErrorReports reports)
        {
            if (File.Exists(filePath))
            {
                var database = XmlFileAccess.ReadFromXmlFile<AllData>(filePath);
                portfolio.CopyData(database.MyFunds);
                return database.myBenchMarks;
            }

            reports.AddReport("Loaded Empty New Database.", Location.Loading);
            portfolio.CopyData(new Portfolio());
            return new List<Sector>();
        }

        public static void SavePortfolio(this Portfolio portfolio, List<Sector> benchMarks, string filePath, ErrorReports reports)
        {
            var toSave = new AllData(portfolio, benchMarks);
            if (filePath != null)
            {
                XmlFileAccess.WriteToXmlFile(filePath, toSave);
                reports.AddReport("Saved Database.", Location.Saving);
            }
        }
    }
}
