﻿using System;
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
            List<Task> downloadTasks = new List<Task>();
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
            List<Task> downloadTasks = new List<Task>();
            foreach (ISecurity sec in portfo.FundsThreadSafe)
            {
                if (!string.IsNullOrEmpty(sec.Names.Url))
                {
                    downloadTasks.Add(DownloadLatestValue(sec.Names, value => sec.SetData(DateTime.Today, value, reportLogger), reportLogger));
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
                    downloadTasks.Add(DownloadLatestValue(acc.Names, value => acc.SetData(DateTime.Today, value, reportLogger), reportLogger));
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
                    downloadTasks.Add(DownloadLatestValue(currency.Names, value => currency.SetData(DateTime.Today, value, reportLogger), reportLogger));
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
                    downloadTasks.Add(DownloadLatestValue(sector.Names, value => sector.SetData(DateTime.Today, value, reportLogger), reportLogger));
                }
                else
                {
                    _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.Downloading, $"No Url set for {sector.Names}");
                }
            }

            return downloadTasks;
        }

        /// <summary>
        /// Downloads the latest value from the website stored in <paramref name="names"/> url field.
        /// </summary>
        internal static async Task DownloadLatestValue(NameData names, Action<decimal> updateValue, IReportLogger reportLogger = null)
        {
            string data = await WebDownloader.DownloadFromURLasync(names.Url, reportLogger).ConfigureAwait(false);
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

        private static bool ProcessDownloadString(string url, string data, IReportLogger reportLogger, out decimal? value)
        {
            value = null;
            switch (AddressType(url))
            {
                case Website.FT:
                {
                    value = ProcessFromFT(data);
                    if (value == null)
                    {
                        _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Could not download data from FT url: {url}");
                        return false;
                    }

                    return true;
                }
                case Website.Yahoo:
                {
                    value = ProcessFromYahoo(data, url);
                    if (value == null)
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
                    if (value == null)
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

        private static decimal? ProcessFromMorningstar(string data)
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
                    return null;
                }

                return decimal.Parse(str) / 100.0m;
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
                    return null;
                }

                return decimal.Parse(str);
            }

            return null;
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

            bool continuer(char c)
            {
                if (char.IsDigit(c) || c == '.' || c == ',')
                {
                    return true;
                }

                return false;
            };

            int number = 2;
            int poundsValue = -1;
            string searchString;
            do
            {
                searchString = $"data-symbol=\"{code}\" data-test=\"qsp-price\" data-field=\"regularMarketPrice\" data-trend=\"none\" data-pricehint=\"{number}\"";
                poundsValue = data.IndexOf(searchString);
                number++;
            }
            while (poundsValue == -1 && number < 100);
            if (poundsValue != -1)
            {
                string containsNewValue = data.Substring(poundsValue + searchString.Length, 20);

                char[] digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                string str = new string(digits);
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                decimal i = decimal.Parse(str);
                if (data.Contains("GBp"))
                {
                    i /= 100.0m;
                }
                return i;
            }

            return null;
        }

        private static decimal? ProcessFromFT(string data)
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
                    return null;
                }

                return decimal.Parse(str);
            }

            int penceValue = data.IndexOf("Price (GBX)");
            if (penceValue != -1)
            {
                string containsNewValue = data.Substring(penceValue + searchString.Length, 200);

                char[] digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                string str = new string(digits);
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                decimal i = decimal.Parse(str);
                return i / 100.0m;
            }

            return null;
        }
    }
}
