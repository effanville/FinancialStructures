using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Reporting;
using System.IO;

namespace FinancialStructures.StatsMakers
{
    public static class InvestmentsExporter
    {
        public static void Export(IPortfolio portfolio, string filePath, IReportLogger reportLogger = null)
        {
            StreamWriter statsWriter = new StreamWriter(filePath);
            // write in column headers
            statsWriter.WriteLine("Securities Investments");
            statsWriter.WriteLine("Date, Company, Name, Investment Amount");
            foreach (DayValue_Named stats in portfolio.AllSecuritiesInvestments())
            {
                string securitiesData = stats.Day.ToShortDateString() + ", " + stats.Names.Company + ", " + stats.Names.Name + ", " + stats.Value.ToString();
                statsWriter.WriteLine(securitiesData);
            }

            reportLogger?.Log(ReportSeverity.Critical, ReportType.Report, ReportLocation.Saving, $"Created Investment list page at {filePath}.");
            statsWriter.Close();
        }
    }
}
