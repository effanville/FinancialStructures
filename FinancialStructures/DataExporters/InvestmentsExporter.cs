using System.IO;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.DataStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.DataExporters
{
    public static class InvestmentsExporter
    {
        public static void Export(IPortfolio portfolio, string filePath, IReportLogger reportLogger = null)
        {
            StreamWriter statsWriter = new StreamWriter(filePath);
            // write in column headers
            statsWriter.WriteLine("Securities Investments");
            statsWriter.WriteLine("Date, Company, Name, Investment Amount");
            foreach (DayValue_Named stats in portfolio.TotalInvestments(Totals.Security))
            {
                string securitiesData = stats.Day.ToShortDateString() + ", " + stats.Names.Company + ", " + stats.Names.Name + ", " + stats.Value.ToString();
                statsWriter.WriteLine(securitiesData);
            }

            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Report, ReportLocation.Saving, $"Created Investment list page at {filePath}.");
            statsWriter.Close();
        }
    }
}
