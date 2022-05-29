using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Structure.Reporting;
using FinancialStructures.Download;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Download
{
    /// <summary>
    /// Contains download routines to update portfolio.
    /// </summary>
    public static class PortfolioDataUpdater
    {
        /// <summary>
        /// Updates specific object.
        /// </summary>
        /// <param name="accountType">The type of the object to update</param>
        /// <param name="portfolio">The database storing the object</param>
        /// <param name="names">The name of the object.</param>
        /// <param name="reportLogger">An optional update logger.</param>
        public static async Task Download(Account accountType, IPortfolio portfolio, TwoName names, IReportLogger reportLogger = null)
        {
            List<Task> downloadTasks = new List<Task>();
            if (accountType == Account.All)
            {
                downloadTasks.AddRange(DownloadPortfolioLatest(portfolio, reportLogger));
            }
            else
            {
                _ = portfolio.TryGetAccount(accountType, names, out IValueList acc);
                downloadTasks.Add(DownloadLatestValue(acc.Names, value => UpdateAndCheck(acc, value, reportLogger), reportLogger));
            }

            await Task.WhenAll(downloadTasks);
            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Downloading, "Downloader Completed");
        }

        private static List<Task> DownloadPortfolioLatest(IPortfolio portfo, IReportLogger reportLogger)
        {
            List<Task> downloadTasks = new List<Task>();
            foreach (ISecurity sec in portfo.FundsThreadSafe)
            {
                if (!string.IsNullOrEmpty(sec.Names.Url))
                {
                    downloadTasks.Add(DownloadLatestValue(sec.Names, value => UpdateAndCheck(sec, value, reportLogger), reportLogger));
                }
                else
                {
                    _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.Downloading, $"No Url set for {sec.Names}");
                }
            }
            foreach (IExchangableValueList acc in portfo.BankAccountsThreadSafe)
            {
                if (!string.IsNullOrEmpty(acc.Names.Url))
                {
                    downloadTasks.Add(DownloadLatestValue(acc.Names, value => UpdateAndCheck(acc, value, reportLogger), reportLogger));
                }
                else
                {
                    _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.Downloading, $"No Url set for {acc.Names}");
                }
            }
            foreach (ICurrency currency in portfo.CurrenciesThreadSafe)
            {
                if (!string.IsNullOrEmpty(currency.Names.Url))
                {
                    downloadTasks.Add(DownloadLatestValue(currency.Names, value => UpdateAndCheck(currency, value, reportLogger), reportLogger));
                }
                else
                {
                    _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.Downloading, $"No Url set for {currency.Names}");
                }
            }
            foreach (IValueList sector in portfo.BenchMarksThreadSafe)
            {
                if (!string.IsNullOrEmpty(sector.Names.Url))
                {
                    downloadTasks.Add(DownloadLatestValue(sector.Names, value => UpdateAndCheck(sector, value, reportLogger), reportLogger));
                }
                else
                {
                    _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.Downloading, $"No Url set for {sector.Names}");
                }
            }
            foreach (IValueList asset in portfo.Assets)
            {
                if (!string.IsNullOrEmpty(asset.Names.Url))
                {
                    downloadTasks.Add(DownloadLatestValue(asset.Names, value => asset.SetData(DateTime.Today, value, reportLogger), reportLogger));
                }
                else
                {
                    _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.Downloading, $"No Url set for {asset.Names}");
                }
            }

            return downloadTasks;
        }

        private static void UpdateAndCheck(IValueList valueList, decimal valueToUpdate, IReportLogger logger)
        {
            decimal latestValue = valueList.LatestValue()?.Value ?? 0.0m;
            valueList.SetData(DateTime.Today, valueToUpdate, logger);

            decimal newLatestValue = valueList.LatestValue()?.Value ?? 0.0m;
            if (newLatestValue == 0.0m || latestValue == 0.0m)
            {
                return;
            }

            decimal scaleFactor = latestValue / newLatestValue;
            if (scaleFactor > 50 || scaleFactor < 0.02m)
            {
                _ = logger.Log(
                    ReportSeverity.Critical,
                    ReportType.Warning,
                    ReportLocation.Downloading,
                    $"Account {valueList.Names} has large change in value from {latestValue} to {newLatestValue}.");
            }
        }

        /// <summary>
        /// Downloads the latest value from the website stored in <paramref name="names"/> url field.
        /// </summary>
        internal static async Task DownloadLatestValue(NameData names, Action<decimal> updateValue, IReportLogger reportLogger = null)
        {
            var downloader = PriceDownloaderFactory.Retrieve(names.Url);
            if (downloader != null && await downloader.TryGetLatestPriceFromUrl(names.Url, updateValue, reportLogger))
            {
            }
            else
            {
                _ = reportLogger?.LogUsefulError(ReportLocation.Downloading, $"{names.Company}-{names.Name}: could not download data from {names.Url}");
            }
        }
    }
}
