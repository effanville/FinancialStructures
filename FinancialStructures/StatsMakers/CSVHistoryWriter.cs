using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinancialStructures.StatisticStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.StatsMakers
{
    public static class CSVHistoryWriter
    {
        public static async void WriteToCSV(List<PortfolioDaySnapshot> historyStatistics, string filePath, IReportLogger reportLogger = null)
        {
            try
            {
                StreamWriter historyWriter = new StreamWriter(filePath);
                if (!historyStatistics.Any())
                {
                    reportLogger?.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, "Not enough history points to export.");
                    return;
                }
                historyWriter.WriteLine(historyStatistics[0].Headers());
                foreach (PortfolioDaySnapshot statistic in historyStatistics)
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
