﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;
using StructureCommon.WebAccess;

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
            switch (accountType)
            {
                case (Account.Security):
                {
                    _ = portfolio.TryGetSecurity(names, out ISecurity sec);
                    await DownloadLatestValue(sec.Names, value => sec.UpdateSecurityData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
                    break;
                }
                case (Account.BankAccount):
                case (Account.Currency):
                case (Account.Benchmark):
                {
                    _ = portfolio.TryGetAccount(accountType, names, out ISingleValueDataList acc);
                    await DownloadLatestValue(acc.Names, value => acc.TryAddOrEditData(DateTime.Today, DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
                    break;
                }
                case (Account.All):
                {
                    await DownloadPortfolioLatest(portfolio, reportLogger);
                    break;
                }
            }
        }

        private static readonly string Pence = "GBX";
        private static readonly string Pounds = "GBP";

        private static WebsiteType AddressType(string address)
        {
            if (address.Contains("morningstar"))
            {
                return WebsiteType.Morningstar;
            }
            if (address.Contains("yahoo"))
            {
                return WebsiteType.Yahoo;
            }
            if (address.Contains("google"))
            {
                return WebsiteType.Google;
            }
            if (address.Contains("trustnet"))
            {
                return WebsiteType.TrustNet;
            }
            if (address.Contains("bloomberg"))
            {
                return WebsiteType.Bloomberg;
            }
            if (address.Contains("markets.ft"))
            {
                return WebsiteType.FT;
            }

            return WebsiteType.NotImplemented;
        }

        private static async Task DownloadPortfolioLatest(IPortfolio portfo, IReportLogger reportLogger)
        {
            foreach (ISecurity sec in portfo.Funds)
            {
                if (!string.IsNullOrEmpty(sec.Url))
                {
                    await DownloadLatestValue(sec.Names, value => sec.UpdateSecurityData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
                }
            }
            foreach (ICashAccount acc in portfo.BankAccounts)
            {
                if (!string.IsNullOrEmpty(acc.Url))
                {
                    await DownloadLatestValue(acc.Names, value => acc.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
                }
            }
            foreach (ICurrency currency in portfo.Currencies)
            {
                if (!string.IsNullOrEmpty(currency.Url))
                {
                    await DownloadLatestValue(currency.Names, value => currency.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
                }
            }
            foreach (ISector sector in portfo.BenchMarks)
            {
                if (!string.IsNullOrEmpty(sector.Url))
                {
                    await DownloadLatestValue(sector.Names, value => sector.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
                }
            }
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
                case WebsiteType.FT:
                {
                    value = ProcessFromFT(data);
                    if (double.IsNaN(value))
                    {
                        _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Could not download data from FT url: {url}");
                        return false;
                    }
                    return true;
                }
                case WebsiteType.Yahoo:
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
                case WebsiteType.Morningstar:
                {
                    value = ProcessFromMorningstar(data);
                    if (double.IsNaN(value))
                    {
                        _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Could not download data from Morningstar url: {url}");
                        return false;
                    }
                    return true;
                }
                case WebsiteType.Google:
                case WebsiteType.TrustNet:
                case WebsiteType.Bloomberg:
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
            int number = 31;
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