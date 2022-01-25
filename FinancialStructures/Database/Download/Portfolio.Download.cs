using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Structure.Reporting;
using Common.Structure.WebAccess;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Download
{
    /// <summary>
    /// Contains download routines to update portfolio.
    /// </summary>
    public static class PortfolioDataUpdater
    {
        private static readonly string Pence = "GBX";
        private static readonly string Pounds = "GBP";

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
            decimal latestValue = valueList.LatestValue().Value;
            valueList.SetData(DateTime.Today, valueToUpdate, logger);

            decimal newLatestValue = valueList.LatestValue().Value;
            if (newLatestValue == 0 || latestValue == 0)
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
            string data = await WebDownloader.DownloadFromURLasync(PrepareUrlString(names.Url), reportLogger).ConfigureAwait(false);
            if (string.IsNullOrEmpty(data))
            {
                _ = reportLogger?.LogUsefulError(ReportLocation.Downloading, $"{names.Company}-{names.Name}: could not download data from {names.Url}");
                return;
            }

            if (!ProcessDownloadString(names.Url, data, reportLogger, out decimal? value))
            {
                return;
            }

            updateValue(value.Value);
        }

        private static string PrepareUrlString(string url)
        {
            return url.Replace("^", "%5E");
        }

        private static bool ProcessDownloadString(string url, string data, IReportLogger reportLogger, out decimal? value)
        {
            value = null;
            if (url.Contains("morningstar"))
            {
                value = Process(data, $"<td class=\"line text\">{Pounds}", Pence, 20);
            }
            else if (url.Contains("yahoo"))
            {
                value = ProcessFromYahoo(data, url);
            }
            else if (url.Contains("markets.ft"))
            {
                value = Process(data, $"Price ({Pounds})", $"Price ({Pence})", 200);

            }
            else
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Url not of a currently implemented type: {url}");
                return false;
            }

            if (value == null)
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Could not download data from url: {url}");
                return false;
            }
            return true;
        }

        private static decimal? ProcessFromYahoo(string data, string url)
        {
            string urlSearchString = "/quote/";
            int startIndex = url.IndexOf(urlSearchString);
            int endIndex = url.IndexOfAny(new[] { '/', '?' }, startIndex + urlSearchString.Length);
            if (endIndex == -1)
            {
                endIndex = url.Length;
            }
            string code = url.Substring(startIndex + urlSearchString.Length, endIndex - startIndex - urlSearchString.Length);
            code = code.Replace("%5E", "^").ToUpper();

            // seems to be a bug in the website where it uses the wrong code.
            if (code.Equals("USDGBP=X"))
            {
                code = "GBP=X";
            }

            int number = 2;
            int poundsIndex;
            string searchString;
            do
            {
                searchString = $"data-symbol=\"{code}\" data-test=\"qsp-price\" data-field=\"regularMarketPrice\" data-trend=\"none\" data-pricehint=\"{number}\"";
                poundsIndex = data.IndexOf(searchString);
                number++;
            }
            while (poundsIndex == -1 && number < 100);

            decimal? value = ParseDataIntoNumber(data, poundsIndex, searchString.Length, 20);
            if (value.HasValue)
            {
                if (data.Contains("GBp"))
                {
                    return value.Value / 100.0m;
                }

                return value.Value;
            }

            return null;
        }

        private static decimal? Process(string data, string poundsSearchString, string penceSearchString, int searchLength)
        {
            int penceValueIndex = data.IndexOf(penceSearchString);
            decimal? penceResult = ParseDataIntoNumber(data, penceValueIndex, penceSearchString.Length, searchLength);
            if (penceResult.HasValue)
            {
                return penceResult.Value / 100m;
            }

            int poundsValueIndex = data.IndexOf(poundsSearchString);
            decimal? poundsResult = ParseDataIntoNumber(data, poundsValueIndex, poundsSearchString.Length, searchLength);
            if (poundsResult.HasValue)
            {
                return poundsResult.Value;
            }

            return null;
        }

        private static decimal? ParseDataIntoNumber(string data, int startIndex, int offset, int searchLength)
        {
            if (startIndex == -1)
            {
                return null;
            }

            string shortenedDataString = data.Substring(startIndex + offset, searchLength);
            char[] digits = shortenedDataString.SkipWhile(c => !char.IsDigit(c)).TakeWhile(IsNumericValue).ToArray();

            string str = new string(digits);
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return decimal.Parse(str);
        }

        private static bool IsNumericValue(char c)
        {
            if (char.IsDigit(c) || c == '.' || c == ',')
            {
                return true;
            }

            return false;
        }
    }
}
