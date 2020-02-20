using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.ReportingStructures;
using System.IO;

namespace FinancialStructures.StatsMakers
{
    public static class InvestmentsExporter
    {
        public static void Export(Portfolio portfolio, string filePath, ErrorReports reports)
        {
            StreamWriter statsWriter = new StreamWriter(filePath);
            // write in column headers
            statsWriter.WriteLine("Securities Investments");
            statsWriter.WriteLine("Date, Company, Name, Investment Amount");
            foreach (DayValue_Named stats in portfolio.AllSecuritiesInvestments())
            {
                string securitiesData = stats.Day.ToShortDateString() + ", " + stats.Company + ", " + stats.Name + ", " + stats.Value.ToString();
                statsWriter.WriteLine(securitiesData);
            }
            reports.AddReport($"Created Investment list page at {filePath}.", Location.Saving);
            statsWriter.Close();
        }
    }
}
