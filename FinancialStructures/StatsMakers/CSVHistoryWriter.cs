using FinancialStructures.Database;
using System;
using System.IO;
using System.Linq;

namespace FinancialStructures.StatsMakers
{
    public static class CSVHistoryWriter
    {
        public async static void WriteHistoryToCSV(Portfolio portfolio, Action<string, string, string> reportLogger, string filePath, int daysGap)
        {
            try
            {
                StreamWriter historyWriter = new StreamWriter(filePath);
                var historyStatistics = await portfolio.GenerateHistoryStats(daysGap).ConfigureAwait(false);
                if (!historyStatistics.Any())
                {
                    reportLogger("Error", "StatisticsPage", "Not enough history points to export.");
                    return;
                }
                historyWriter.WriteLine(historyStatistics[0].Headers());
                foreach (var statistic in historyStatistics)
                {
                    historyWriter.WriteLine(statistic.ToString());
                }
                historyWriter.Close();
                reportLogger("Report", "StatisticsPage", $"Successfully exported history to {filePath}.");
            }
            catch (Exception exception)
            {
                reportLogger("Error", "StatisticsPage", exception.Message);

            }
        }
    }
}
