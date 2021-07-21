using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinancialStructures.DataStructures;
using Common.Structure.Reporting;
using System.IO.Abstractions;
using Common.Structure.FileAccess;

namespace FinancialStructures.DataExporters
{
    /// <summary>
    /// Writer of history data to csv file.
    /// </summary>
    public static class CSVHistoryWriter
    {
        /// <summary>
        /// Exports the history to a CSV file.
        /// </summary>
        /// <param name="historyStatistics">The statistics to export.</param>
        /// <param name="filePath">The path to export to.</param>
        /// <param name="reportLogger">Callback to log any issues.</param>
        public static void WriteToCSV(List<PortfolioDaySnapshot> historyStatistics, string filePath, IFileSystem fileSystem, IReportLogger reportLogger = null)
        {
            try
            {
                if (!historyStatistics.Any())
                {
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, "Not enough history points to export.");
                    return;
                }

                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    List<List<string>> valuesToWrite = new List<List<string>>();
                    foreach (PortfolioDaySnapshot statistic in historyStatistics)
                    {
                        valuesToWrite.Add(statistic.ExportValues());
                    }
                    fileWriter.WriteTableFromEnumerable(ExportType.Csv, historyStatistics[0].ExportHeaders(), valuesToWrite, false);
                }

                _ = reportLogger?.LogUseful(ReportType.Information, ReportLocation.StatisticsPage, $"Successfully exported history to {filePath}.");
            }
            catch (Exception exception)
            {
                _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, exception.Message);

            }
        }
    }
}
