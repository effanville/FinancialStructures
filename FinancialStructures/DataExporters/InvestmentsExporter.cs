using System.IO;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using Common.Structure.Reporting;

namespace FinancialStructures.DataExporters
{
    /// <summary>
    /// Contains methods for exporting investment data.
    /// </summary>
    public static class InvestmentsExporter
    {
        /// <summary>
        /// Exports all investments in the portfolio to file.
        /// </summary>
        public static void Export(IPortfolio portfolio, string filePath, IReportLogger reportLogger = null)
        {
            StreamWriter statsWriter = new StreamWriter(filePath);
            // write in column headers
            statsWriter.WriteLine("Securities Investments");
            statsWriter.WriteLine("Date, Company, Name, Investment Amount");
            foreach (var stats in portfolio.TotalInvestments(Totals.Security))
            {
                string securitiesData = stats.Instance.Day.ToShortDateString() + ", " + stats.Label.Company + ", " + stats.Label.Name + ", " + stats.Instance.Value.ToString();
                statsWriter.WriteLine(securitiesData);
            }

            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Saving, $"Created Investment list page at {filePath}.");
            statsWriter.Close();
        }
    }
}
