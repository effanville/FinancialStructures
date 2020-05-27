using System;
using System.IO;
using System.Linq;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;
using StructureCommon.Reporting;

namespace FinancialStructures.StatsMakers
{
    public static class CSVHistoryWriter
    {
        public static async void WriteHistoryToCSV(IPortfolio portfolio, string filePath, int daysGap, IReportLogger reportLogger = null)
        {
            try
            {
                StreamWriter historyWriter = new StreamWriter(filePath);
                System.Collections.Generic.List<StatisticStructures.PortfolioDaySnapshot> historyStatistics = await portfolio.GenerateHistoryStats(daysGap).ConfigureAwait(false);
                if (!historyStatistics.Any())
                {
                    reportLogger?.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, "Not enough history points to export.");
                    return;
                }
                historyWriter.WriteLine(historyStatistics[0].Headers());
                foreach (StatisticStructures.PortfolioDaySnapshot statistic in historyStatistics)
                {
                    historyWriter.WriteLine(statistic.ToString());
                }
                historyWriter.Close();
                reportLogger?.LogUseful(ReportType.Report, ReportLocation.StatisticsPage, $"Successfully exported history to {filePath}.");
            }
            catch (Exception exception)
            {
                reportLogger?.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, exception.Message);

            }
        }
    }
}
