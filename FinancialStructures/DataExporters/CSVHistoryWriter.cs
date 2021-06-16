using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinancialStructures.DataStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.DataExporters
{
    /// <summary>
    /// Writer of history data to csv file.
    /// </summary>
    public static class CSVHistoryWriter
    {
        /// <summary>
        /// Exports the histor to a CSV file.
        /// </summary>
        /// <param name="historyStatistics">The statistics to export.</param>
        /// <param name="filePath">The path to export to.</param>
        /// <param name="reportLogger">Callback to log any issues.</param>
        public static void WriteToCSV(List<PortfolioDaySnapshot> historyStatistics, string filePath, IReportLogger reportLogger = null)
        {
            try
            {
                StreamWriter historyWriter = new StreamWriter(filePath);
                if (!historyStatistics.Any())
                {
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, "Not enough history points to export.");
                    return;
                }
                historyWriter.WriteLine(historyStatistics[0].Headers());
                foreach (PortfolioDaySnapshot statistic in historyStatistics)
                {
                    historyWriter.WriteLine(statistic.ToString());
                }
                historyWriter.Close();
                _ = reportLogger?.LogUseful(ReportType.Information, ReportLocation.StatisticsPage, $"Successfully exported history to {filePath}.");
            }
            catch (Exception exception)
            {
                _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, exception.Message);

            }
        }
    }
}
