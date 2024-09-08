using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Download;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Download
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
            List<DownloadResult> results = new List<DownloadResult>();
            List<Task> downloadTasks = new List<Task>();
            if (accountType == Account.All)
            {
                downloadTasks.AddRange(DownloadPortfolioLatest(portfolio, reportLogger, results));
            }
            else
            {
                _ = portfolio.TryGetAccount(accountType, names, out IValueList acc);
                NameData nameData = acc.Names.Copy();
                if (string.IsNullOrWhiteSpace(nameData.Currency))
                {
                    nameData.Currency = portfolio.BaseCurrency ?? "GBP";
                }

                downloadTasks.Add(DownloadLatestValue(nameData, value => UpdateAndCheck(acc, value, reportLogger, results), reportLogger));
            }

            await Task.WhenAll(downloadTasks);
            
            ReportResults(reportLogger, results);
            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Downloading, "Downloader Completed");
        }

        private static List<Task> DownloadPortfolioLatest(IPortfolio portfolio, IReportLogger reportLogger, List<DownloadResult> results)
        {
            List<Task> downloadTasks = new List<Task>();
            Add(portfolio.Accounts(Account.All));
            Add(portfolio.Currencies);
            Add(portfolio.BenchMarks);

            void Add(IReadOnlyList<IValueList> accounts)
            {
                foreach (IValueList acc in accounts)
                {
                    if (!string.IsNullOrEmpty(acc.Names.Url))
                    {
                        NameData nameData = acc.Names.Copy();
                        if (string.IsNullOrWhiteSpace(nameData.Currency))
                        {
                            nameData.Currency = portfolio.BaseCurrency ?? "GBP";
                        }                
                        results.Add(new DownloadResult(){Name = nameData});
                        downloadTasks.Add(DownloadLatestValue(nameData, value => UpdateAndCheck(acc, value, reportLogger, results), reportLogger));
                    }
                    else
                    {
                        results.Add(new DownloadResult(){Name = acc.Names, Value = -1m});
                        _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.Downloading, $"No Url set for {acc.Names}");
                    }
                }
            }

            return downloadTasks;
        }

        private static void UpdateAndCheck(IValueList valueList, decimal valueToUpdate, IReportLogger logger, List<DownloadResult> results)
        {
            decimal latestValue = valueList.LatestValue()?.Value ?? 0.0m;
            valueList.SetData(DateTime.Today, valueToUpdate, logger);

            DownloadResult result = results.FirstOrDefault(x => x.Name.IsEqualTo(valueList.Names));
            if(result != null)
            {
                result.Value = valueToUpdate;
                result.Success = true;
            }
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

        private static void ReportResults(IReportLogger logger, List<DownloadResult> results)
        {
            results.Sort((x,y)=> y.Success.CompareTo(x.Success));
            foreach (var result in results)
            {
                logger?.Log(ReportType.Information, ReportLocation.Downloading.ToString(),
                    $"DownloadResult. Name={result.Name}, Url='{result.Name.Url}', Success={result.Success}, Value={result.Value}");
            }

            int numberSuccess = results.Count(x => x.Success);
            int numberFailure = results.Count(x => !x.Success && x.Value >= 0.0m);
            int numberNoUrl = results.Count(x=>string.IsNullOrWhiteSpace(x.Name.Url));
            logger?.Log(ReportType.Information, ReportLocation.Downloading.ToString(),
                $"DownloadResults. Total={results.Count} Success={numberSuccess}, Failure={numberFailure}, NoUrl={numberNoUrl}");
        }

        /// <summary>
        /// Downloads the latest value from the website stored in <paramref name="names"/> url field.
        /// </summary>
        internal static async Task DownloadLatestValue(NameData names, Action<decimal> updateValue, IReportLogger reportLogger = null)
        {
            var downloader = PriceDownloaderFactory.Retrieve(names.Url);
            if (downloader == null)
            {
                reportLogger?.Error(ReportLocation.Downloading.ToString(), $"{names.Company}-{names.Name}: Url='{names.Url}' not of supported type");
                return;
            }

            if (!await downloader.TryGetLatestPriceFromUrl(names.Url, names.Currency, updateValue, reportLogger))
            {
                reportLogger?.Error(ReportLocation.Downloading.ToString(), $"{names.Company}-{names.Name}: Couldnt get price data from {names.Url}");
            }
        }
    }
}
