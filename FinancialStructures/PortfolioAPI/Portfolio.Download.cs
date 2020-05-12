using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinancialStructures.PortfolioAPI
{
    public static class PortfolioDataUpdater
    {
        private enum WebsiteType
        {
            Morningstar,
            Yahoo,
            Google,
            Bloomberg,
            TrustNet,
            FT,
            NotImplemented
        }

        public static async Task Downloader(IPortfolio portfolio, IReportLogger reportLogger)
        {
            await DownloadPortfolioLatest(portfolio, reportLogger).ConfigureAwait(false);
        }

        public static async Task DownloadOfType(AccountType accountType, IPortfolio portfolio, TwoName names, IReportLogger reportLogger)
        {
            switch (accountType)
            {
                case (AccountType.Security):
                {
                    _ = portfolio.TryGetSecurity(names, out var sec);
                    await DownloadLatestValue(sec.Names, value => sec.UpdateSecurityData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
                    break;
                }
                case (AccountType.BankAccount):
                case (AccountType.Currency):
                case (AccountType.Sector):
                {
                    _ = portfolio.TryGetAccount(accountType, names, out var acc);
                    await DownloadLatestValue(acc.Names, value => acc.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
                    break;
                }
            }
        }

        private static string Pence = "GBX";
        private static string Pounds = "GBP";
        private static HttpClient client = new HttpClient();
        private static bool IsValidWebAddress(string address)
        {
            if (!Uri.TryCreate(address, UriKind.Absolute, out Uri uri) || null == uri)
            {
                return false;
            }

            return Uri.IsWellFormedUriString(address, UriKind.Absolute);
        }

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
                await DownloadLatestValue(sec.Names, value => sec.UpdateSecurityData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
            }
            foreach (ICashAccount acc in portfo.BankAccounts)
            {
                await DownloadLatestValue(acc.Names, value => acc.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
            }
            foreach (ICurrency currency in portfo.Currencies)
            {
                await DownloadLatestValue(currency.Names, value => currency.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
            }
            foreach (ISector sector in portfo.BenchMarks)
            {
                await DownloadLatestValue(sector.Names, value => sector.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
            }
        }

        public static async Task DownloadLatestValue(NameData names, Action<double> updateValue, IReportLogger reportLogger)
        {
            string data = await DownloadFromURL(names.Url, reportLogger).ConfigureAwait(false);
            if (string.IsNullOrEmpty(data))
            {
                reportLogger.LogUseful(ReportType.Error, ReportLocation.Downloading, $"{names.Company}-{names.Name}: could not download data from {names.Url}");
                return;
            }
            if (!ProcessDownloadString(names.Url, data, reportLogger, out double value))
            {
                return;
            }

            updateValue(value);
        }

        public static async Task<string> DownloadFromURL(string url, IReportLogger reportLogger)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            string newUrl = Uri.EscapeUriString(url);
            string output = string.Empty;
            if (IsValidWebAddress(newUrl))
            {
                try
                {
                    HttpRequestMessage requestMessage = new HttpRequestMessage
                    {
                        RequestUri = new Uri(newUrl)
                    };
                    requestMessage.Headers.Add("Cookie", "A1S=d=AQABBBqjZl4CELBdwrYAVcmgp1PaEGx2xoQFEgABAQGPgF5eX_bPb2UB_iMAAAcIGqNmXmx2xoQ&S=AQAAAii_Jnul1r-aKGkxwIrap9c&j=GDPR");
                    requestMessage.Headers.Add("Cookie", "A1=d=AQABBBqjZl4CELBdwrYAVcmgp1PaEGx2xoQFEgABAQGPgF5eX_bPb2UB_iMAAAcIGqNmXmx2xoQ&S=AQAAAii_Jnul1r-aKGkxwIrap9c");
                    requestMessage.Headers.Add("Cookie", "A3=d=AQABBBqjZl4CELBdwrYAVcmgp1PaEGx2xoQFEgABAQGPgF5eX_bPb2UB_iMAAAcIGqNmXmx2xoQ&S=AQAAAii_Jnul1r-aKGkxwIrap9c");
                    requestMessage.Headers.Add("Cookie", "B=89hjmdhf6d8oq&b=3&s=bv");
                    requestMessage.Headers.Add("Cookie", "cmp=v=28&t=1586111544&j=1&o=106");
                    requestMessage.Headers.Add("Cookie", "EuConsent=BOw-KL2OxZWozAOABCENC7uAAAAtl6__f_97_8_v2ddvduz_Ov_j_c__3XW8fPZvcELzhK9Meu_2xzd4u9wNRM5wckx87eJrEso5czISsG-RMod_zt__3ziX9oxPowEc9rz3nbEw6vs2v-ZzBCGJ_Iw");
                    requestMessage.Headers.Add("Cookie", "GUC=AQABAQFegI9fXkIe8wSV");
                    requestMessage.Headers.Add("Cookie", "PRF=t%3D2800.HK%252BVUKE.L%252BIGLS.L%252BSAAA.L%252BVWRL.L");
                    HttpResponseMessage response = await client.SendAsync(requestMessage).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (result != null)
                    {
                        output = result;
                    }
                }
                catch (Exception ex)
                {
                    reportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Failed to download from url {url}. Reason : {ex.Message}");
                    return output;
                }
            }

            return output;
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
                        reportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Could not download data from FT url: {url}");
                        return false;
                    }
                    return true;
                }
                case WebsiteType.Yahoo:
                {
                    value = ProcessFromYahoo(data);
                    if (double.IsNaN(value))
                    {
                        reportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Could not download data from Yahoo url: {url}");
                        return false;
                    }
                    return true;
                }
                default:
                case WebsiteType.Morningstar:
                case WebsiteType.Google:
                case WebsiteType.TrustNet:
                case WebsiteType.Bloomberg:
                    reportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Url not of a currently implemented downloadable type: {url}");
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

                var digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                var str = new string(digits);
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
                var digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                var str = new string(digits);
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

                var digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                var str = new string(digits);
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

                var digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                var str = new string(digits);
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

                var digits = containsNewValue.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

                var str = new string(digits);
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
