using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Common.Structure.FileAccess;
using Common.Structure.Reporting;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;

namespace FinancialStructures.DataExporters.History
{
    /// <summary>
    /// Class to store the historic evolution of a <see cref="IPortfolio"/>
    /// </summary>
    public sealed class PortfolioHistory
    {
        /// <summary>
        /// Records of the history.
        /// </summary>
        public List<PortfolioDaySnapshot> Snapshots
        {
            get;
            set;
        }

        /// <summary>
        /// Creates the portfolio history.
        /// </summary>
        /// <param name="portfolio">The portfolio to create history for.</param>
        public PortfolioHistory(IPortfolio portfolio)
        {
            GenerateHistoryStats(portfolio, new PortfolioHistorySettings());
        }

        /// <summary>
        /// Creates the portfolio history.
        /// </summary>
        /// <param name="portfolio">The portfolio to create history for.</param>
        /// <param name="settings">The settings for the history.</param>
        public PortfolioHistory(IPortfolio portfolio, PortfolioHistorySettings settings)
        {
            GenerateHistoryStats(portfolio, settings);
        }

        private void GenerateHistoryStats(IPortfolio portfolio, PortfolioHistorySettings settings)
        {
            List<PortfolioDaySnapshot> outputs = new List<PortfolioDaySnapshot>();
            if (!settings.SnapshotIncrement.Equals(0))
            {
                DateTime calculationDate = portfolio.FirstValueDate(Totals.All);

                while (calculationDate < DateTime.Today)
                {
                    PortfolioDaySnapshot calcuationDateStatistics = new PortfolioDaySnapshot(calculationDate, portfolio, settings.GenerateSecurityRates, settings.GenerateSectorRates);
                    outputs.Add(calcuationDateStatistics);
                    calculationDate = calculationDate.AddDays(settings.SnapshotIncrement);
                }
                if (calculationDate == DateTime.Today)
                {
                    PortfolioDaySnapshot calcuationDateStatistics = new PortfolioDaySnapshot(calculationDate, portfolio, settings.GenerateSecurityRates, settings.GenerateSectorRates);
                    outputs.Add(calcuationDateStatistics);
                }
            }

            Snapshots = outputs;
        }

        /// <summary>
        /// Exports the history to a file.
        /// </summary>
        public void ExportToFile(string filePath)
        {
            ExportToFile(filePath, new FileSystem());
        }

        /// <summary>
        /// Exports the history to a file.
        /// </summary>
        public void ExportToFile(string filePath, IFileSystem fileSystem, IReportLogger reportLogger = null)
        {
            if (!Snapshots.Any())
            {
                _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, "Not enough history points to export.");
                return;
            }

            try
            {
                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    List<List<string>> valuesToWrite = new List<List<string>>();
                    foreach (PortfolioDaySnapshot statistic in Snapshots)
                    {
                        valuesToWrite.Add(statistic.ExportValues());
                    }
                    fileWriter.WriteTableFromEnumerable(ExportType.Csv, Snapshots[0].ExportHeaders(), valuesToWrite, false);
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
