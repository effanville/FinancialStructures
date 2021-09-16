using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using Common.Structure.Reporting;
using Common.Structure.WebAccess;

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
        /// <returns></returns>
        public static async Task Download(Account accountType, IPortfolio portfolio, TwoName names, IReportLogger reportLogger = null)
        {
            var downloadTasks = new List<Task>();
            if (accountType == Account.All)
            {
                downloadTasks.AddRange(DownloadPortfolioLatest(portfolio, reportLogger));
            }
            else
            {
                _ = portfolio.TryGetAccount(accountType, names, out IValueList acc);
                downloadTasks.Add(DownloadLatestValue(acc.Names, value => acc.SetData(DateTime.Today, value, reportLogger), reportLogger));
            }

            await Task.WhenAll(downloadTasks);
            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Downloading, "Downloader Completed");
        }

        private static readonly string Pence = "GBX";
        private static readonly string Pounds = "GBP";

        private static Website AddressType(string address)
        {
            if (address.Contains("morningstar"))
            {
                return Website.Morningstar;
            }
            if (address.Contains("yahoo"))
            {
                return Website.Yahoo;
            }
            if (address.Contains("google"))
            {
                return Website.Google;
            }
            if (address.Contains("trustnet"))
            {
                return Website.TrustNet;
            }
            if (address.Contains("bloomberg"))
            {
                return Website.Bloomberg;
            }
            if (address.Contains("markets.ft"))
            {
                return Website.FT;
            }

            return Website.NotImplemented;
        }

        private static List<Task> DownloadPortfolioLatest(IPortfolio portfo, IReportLogger reportLogger)
        {
            var downloadTasks = new List<Task>();
            foreach (ISecurity sec in portfo.FundsThreadSafe)
            {
                if (!string.IsNullOrEmpty(sec.Names.Url))
                {
                    downloadTasks.Add(DownloadLatestValue(sec.Names, value => sec.SetData(DateTime.Today, value, reportLogger), reportLogger));
                }
            }
            foreach (IExchangableValueList acc in portfo.BankAccountsThreadSafe)
            {
                if (!string.IsNullOrEmpty(acc.Names.Url))
                {
                    downloadTasks.Add(DownloadLatestValue(acc.Names, value => acc.SetData(DateTime.Today, value, reportLogger), reportLogger));
                }
            }
            foreach (ICurrency currency in portfo.CurrenciesThreadSafe)
            {
                if (!string.IsNullOrEmpty(currency.Names.Url))
                {
                    downloadTasks.Add(DownloadLatestValue(currency.Names, value => currency.SetData(DateTime.Today, value, reportLogger), reportLogger));
                }
            }
            foreach (ISector sector in portfo.BenchMarksThreadSafe)
            {
                if (!string.IsNullOrEmpty(sector.Names.Url))
                {
                    downloadTasks.Add(DownloadLatestValue(sector.Names, value => sector.SetData(DateTime.Today, value, reportLogger), reportLogger));
                }
            }

            return downloadTasks;
        }

        /// <summary>
        /// Downloads the latest value from the website stored in <paramref name="names"/> url field.
        /// </summary>
        internal static async Task DownloadLatestValue(NameData names, Action<double> updateValue, IReportLogger reportLogger = null)
        {
            string data = await WebDownloader.DownloadFromURLasync(names.Url, reportLogger).ConfigureAwait(false);
            if (string.IsNullOrEmpty(data))
            {
                _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.Downloading, $"{names.Company}-{names.Name}: could not download data from {names.Url}");
                return;
            }
            if (!ProcessDownloadString(names.Url, data, reportLogger, out double value))
            {
                return;
            }

            updateValue(value);
        }

        private static bool ProcessDownloadString(string url, string data, IReportLogger reportLogger, out double value)
        {
            value = double.NaN;
            switch (AddressType(url))
            {
                case Website.FT:
                {
                    value = ProcessFromFT(data);
                    if (double.IsNaN(value))
                    {
                        _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Could not download data from FT url: {url}");
                        return false;
                    }
                    return true;
                }
                case Website.Yahoo:
                {
                    value = ProcessFromYahoo(data);
                    if (double.IsNaN(value))
                    {
                        _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Could not download data from Yahoo url: {url}");
                        return false;
                    }
                    return true;
                }
                default:
                case Website.Morningstar:
                {
                    value = ProcessFromMorningstar(data);
                    if (double.IsNaN(value))
                    {
                        _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Could not download data from Morningstar url: {url}");
                        return false;
                    }
                    return true;
                }
                case Website.Google:
                case Website.TrustNet:
                case Website.Bloomberg:
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Url not of a currently implemented downloadable type: {url}");
                    return false;
            }
        }

        private static double ProcessFromMorningstar(string data)
        {
            bool continuer(char c)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    return true;
                }

                return false;
            };

            int penceValue = data.IndexOf(Pence);
            if (penceValue != -1)
            {
                string containsNewValue = data.Substring(Pence.Length + penceValue, 20);

                char[] digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                string str = new string(digits);
                if (string.IsNullOrEmpty(str))
                {
                    return double.NaN;
                }
                double i = double.Parse(str);
                i /= 100;
                return i;
            }
            string searchName = "<td class=\"line text\">" + Pounds;
            int poundsValue = data.IndexOf(searchName);
            if (poundsValue != -1)
            {
                string containsNewValue = data.Substring(searchName.Length + poundsValue, 20);
                char[] digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                string str = new string(digits);
                if (string.IsNullOrEmpty(str))
                {
                    return double.NaN;
                }
                double i = double.Parse(str);

                return i;
            }

            return double.NaN;
        }

        private static double ProcessFromYahoo(string data)
        {
            bool continuer(char c)
            {
                if (char.IsDigit(c) || c == '.' || c == ',')
                {
                    return true;
                }

                return false;
            };
            int number = 30;
            string searchString = $"data-reactid=\"{number}\"><span class=\"Trsdu(0.3s) Fw(b) Fz(36px) Mb(-4px) D(ib)\" data-reactid=\"{number + 1}\">";
            int poundsValue = data.IndexOf(searchString);
            if (poundsValue != -1)
            {
                string containsNewValue = data.Substring(poundsValue + searchString.Length, 20);

                char[] digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                string str = new string(digits);
                if (string.IsNullOrEmpty(str))
                {
                    return double.NaN;
                }
                double i = double.Parse(str);
                if (data.Contains("GBp"))
                {
                    i /= 100.0;
                }
                return i;
            }

            return double.NaN;
        }

        private static double ProcessFromFT(string data)
        {
            bool continuer(char c)
            {
                if (char.IsDigit(c) || c == '.' || c == ',')
                {
                    return true;
                }

                return false;
            };
            string searchString = "Price (GBP)";
            int poundsValue = data.IndexOf(searchString);
            if (poundsValue != -1)
            {
                string containsNewValue = data.Substring(poundsValue + searchString.Length, 200);

                char[] digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                string str = new string(digits);
                if (string.IsNullOrEmpty(str))
                {
                    return double.NaN;
                }
                double i = double.Parse(str);
                return i;
            }

            int penceValue = data.IndexOf("Price (GBX)");
            if (penceValue != -1)
            {
                string containsNewValue = data.Substring(penceValue + searchString.Length, 200);

                char[] digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                string str = new string(digits);
                if (string.IsNullOrEmpty(str))
                {
                    return double.NaN;
                }
                double i = double.Parse(str);
                return i / 100.0;
            }

            return double.NaN;
        }
    }
}
