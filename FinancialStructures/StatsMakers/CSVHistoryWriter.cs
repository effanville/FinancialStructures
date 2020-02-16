using FinancialStructures.Database;
using FinancialStructures.ReportingStructures;
using System;
using System.IO;
using System.Linq;

namespace FinancialStructures.StatsMakers
{
    public static class CSVHistoryWriter
    {
        public async static void WriteHistoryToCSV(Portfolio portfolio, Action<ErrorReports> reportCallback, string filePath, int daysGap)
        {
            var reports = new ErrorReports();
            try
            {
                StreamWriter historyWriter = new StreamWriter(filePath);
                var historyStatistics = await portfolio.GenerateHistoryStats(daysGap).ConfigureAwait(false);
                if (!historyStatistics.Any())
                {
                    reports.AddError("Not enough history points to export.", Location.StatisticsPage);
                    return;
                }
                historyWriter.WriteLine(historyStatistics[0].Headers());
                foreach (var statistic in historyStatistics)
                {
                    historyWriter.WriteLine(statistic.ToString());
                }
                historyWriter.Close();
                reports.AddReport($"Successfully exported history to {filePath}.", Location.StatisticsPage);
            }
            catch (Exception exception)
            {
                reports.AddError(exception.Message, Location.StatisticsPage);

            }
            finally
            {
                reportCallback(reports);
            }
        }
    }
}
