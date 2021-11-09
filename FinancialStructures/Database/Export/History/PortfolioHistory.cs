using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Common.Structure.FileAccess;
using Common.Structure.Reporting;
using FinancialStructures.Database.Extensions.Values;

namespace FinancialStructures.Database.Export.History
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
            PortfolioHistorySettings settings = new PortfolioHistorySettings();
            if (ShouldMultiThread(portfolio, settings, forceMultiThreading: false))
            {
                GenerateHistoryStatsMulti(portfolio, settings);
            }
            else
            {
                GenerateHistoryStats(portfolio, settings);
            }
        }

        /// <summary>
        /// Creates the portfolio history.
        /// </summary>
        /// <param name="portfolio">The portfolio to create history for.</param>
        /// <param name="settings">The settings for the history.</param>
        public PortfolioHistory(IPortfolio portfolio, PortfolioHistorySettings settings)
        {
            if (ShouldMultiThread(portfolio, settings, forceMultiThreading: false))
            {
                GenerateHistoryStatsMulti(portfolio, settings);
            }
            else
            {
                GenerateHistoryStats(portfolio, settings);
            }
        }

        private static bool ShouldMultiThread(IPortfolio portfolio, PortfolioHistorySettings settings, bool forceMultiThreading = false)
        {
            if (forceMultiThreading)
            {
                return true;
            }
            if (settings.GenerateSecurityRates)
            {
                return true;
            }

            if (!(settings.GenerateSecurityRates || settings.GenerateSectorRates) && portfolio.FundsThreadSafe.Count >= 15)
            {
                return true;
            }

            return false;
        }

        private static List<DateTime> PrepareTimes(IPortfolio portfolio, PortfolioHistorySettings settings)
        {
            List<DateTime> times = new List<DateTime>();
            if (!settings.SnapshotIncrement.Equals(0))
            {
                DateTime calculationDate = portfolio.FirstValueDate(Totals.All);
                while (calculationDate < DateTime.Today)
                {
                    times.Add(calculationDate);
                    calculationDate = calculationDate.AddDays(settings.SnapshotIncrement);
                }
                if (calculationDate == DateTime.Today)
                {
                    times.Add(calculationDate);
                }
                if (calculationDate == DateTime.MaxValue && !times.Contains(DateTime.Today))
                {
                    times.Add(DateTime.Today);
                }
            }
            return times;
        }

        private void GenerateHistoryStats(IPortfolio portfolio, PortfolioHistorySettings settings)
        {
            List<PortfolioDaySnapshot> outputs = new List<PortfolioDaySnapshot>();
            foreach (DateTime time in PrepareTimes(portfolio, settings))
            {
                PortfolioDaySnapshot calcuationDateStatistics = new PortfolioDaySnapshot(time, portfolio, includeSecurityValues: true, includeBankValues: true, includeSectorValues: true, settings.GenerateSecurityRates, settings.GenerateSectorRates);
                outputs.Add(calcuationDateStatistics);
            }


            Snapshots = outputs;
        }

        private void GenerateHistoryStatsMulti(IPortfolio portfolio, PortfolioHistorySettings settings)
        {
            ConcurrentBag<PortfolioDaySnapshot> bag = new ConcurrentBag<PortfolioDaySnapshot>();
            List<Task> tasks = new List<Task>();
            foreach (DateTime time in PrepareTimes(portfolio, settings))
            {
                Task task = Task.Run(() =>
                {
                    PortfolioDaySnapshot snapshot = new PortfolioDaySnapshot(time, portfolio, includeSecurityValues: true, includeBankValues: true, includeSectorValues: true, settings.GenerateSecurityRates, settings.GenerateSectorRates);
                    bag.Add(snapshot);
                });
                tasks.Add(task);
            }

            Task.WhenAll(tasks).Wait();
            Snapshots = bag.ToList();
            Snapshots.Sort();
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
