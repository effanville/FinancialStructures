using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

using Common.Structure.Reporting;
using Common.Structure.ReportWriting;

using FinancialStructures.Database.Extensions.Values;

namespace FinancialStructures.Database.Export.History
{
    /// <summary>
    /// Class to store the historic evolution of a <see cref="IPortfolio"/>
    /// </summary>
    public sealed partial class PortfolioHistory
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
            Settings settings = new Settings();
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
        public PortfolioHistory(IPortfolio portfolio, Settings settings)
        {
            if (ShouldMultiThread(portfolio, settings, forceMultiThreading: true))
            {
                GenerateHistoryStatsMulti(portfolio, settings);
            }
            else
            {
                GenerateHistoryStats(portfolio, settings);
            }
        }

        private static bool ShouldMultiThread(IPortfolio portfolio, Settings settings, bool forceMultiThreading = false)
        {
            if (forceMultiThreading)
            {
                return true;
            }
            if (settings[Account.Security].GenerateRates)
            {
                return true;
            }

            if (!(settings[Account.Security].GenerateRates || settings[Account.Benchmark].GenerateRates) && portfolio.FundsThreadSafe.Count >= 15)
            {
                return true;
            }

            return false;
        }

        private static List<DateTime> PrepareTimes(IPortfolio portfolio, Settings settings)
        {
            List<DateTime> times = new List<DateTime>();
            if (!settings.SnapshotIncrement.Equals(0))
            {
                DateTime calculationDate = settings.EarliestDate != default ? settings.EarliestDate : portfolio.FirstValueDate(Totals.All);
                DateTime lastDate = settings.LastDate != default ? settings.LastDate : portfolio.LatestDate(Totals.All);
                while (calculationDate < lastDate)
                {
                    times.Add(calculationDate);
                    calculationDate = calculationDate.AddDays(settings.SnapshotIncrement);
                }
                if (calculationDate == lastDate)
                {
                    times.Add(calculationDate);
                }
                if (calculationDate == DateTime.MaxValue && !times.Contains(lastDate))
                {
                    times.Add(lastDate);
                }
            }

            return times;
        }

        private void GenerateHistoryStats(IPortfolio portfolio, Settings settings)
        {
            List<PortfolioDaySnapshot> outputs = new List<PortfolioDaySnapshot>();
            foreach (DateTime time in PrepareTimes(portfolio, settings))
            {
                PortfolioDaySnapshot calcuationDateStatistics = new PortfolioDaySnapshot(
                    time,
                    portfolio,
                    settings);
                outputs.Add(calcuationDateStatistics);
            }


            Snapshots = outputs;
        }

        private void GenerateHistoryStatsMulti(IPortfolio portfolio, Settings settings)
        {
            var times = PrepareTimes(portfolio, settings);
            PortfolioDaySnapshot[] snapshots = new PortfolioDaySnapshot[times.Count];
            _ = Parallel.For(fromInclusive: 0, times.Count, timeIndex => GenerateSnapshot(timeIndex));

            void GenerateSnapshot(int index)
            {
                PortfolioDaySnapshot snapshot = new PortfolioDaySnapshot(
                    times[index],
                    portfolio,
                    settings);
                snapshots[index] = snapshot;
            }

            Snapshots = snapshots.ToList();
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
                reportLogger?.Log(ReportType.Error, ReportLocation.StatisticsPage.ToString(), "Not enough history points to export.");
                return;
            }

            try
            {
                List<List<string>> valuesToWrite = new List<List<string>>();
                foreach (PortfolioDaySnapshot statistic in Snapshots)
                {
                    valuesToWrite.Add(statistic.ExportValues());
                }

                ReportBuilder reportBuilder = new ReportBuilder(DocumentType.Csv, new ReportSettings(true, false, false));
                _ = reportBuilder.WriteTableFromEnumerable(Snapshots[0].ExportHeaders(), valuesToWrite, false);

                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(reportBuilder.ToString());
                }

                reportLogger?.Log(ReportType.Information, ReportLocation.StatisticsPage.ToString(), $"Successfully exported history to {filePath}.");
            }
            catch (Exception exception)
            {
                reportLogger?.Log(ReportType.Error, ReportLocation.StatisticsPage.ToString(), exception.Message);
            }
        }
    }
}
