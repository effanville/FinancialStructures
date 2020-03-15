using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
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

        public async static Task Downloader(Portfolio portfolio, Action<string, string, string> reportLogger)
        {
            await DownloadPortfolioLatest(portfolio, reportLogger).ConfigureAwait(false);
        }

        public async static Task DownloadSecurity(Portfolio portfolio, string company, string name, Action<string, string, string> reportLogger)
        {
            portfolio.TryGetSecurity(company, name, out var sec);
            await DownloadLatestValue(sec.GetCompany(), sec.GetName(), sec.GetUrl(), value => sec.UpdateSecurityData(value, reportLogger, DateTime.Today), reportLogger).ConfigureAwait(false);
        }

        public async static Task DownloadCurrency(Portfolio portfolio, NameData name, Action<string, string, string> reportLogger)
        {
            portfolio.TryGetCurrency(name.Name, out var currency);
            await DownloadLatestValue(string.Empty, currency.GetName(), currency.GetUrl(), value => currency.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
        }

        public async static Task DownloadBankAccount(Portfolio portfolio, NameData name, Action<string, string, string> reportLogger)
        {
            portfolio.TryGetBankAccount(name.Company, name.Name, out var acc);
            await DownloadLatestValue(acc.GetCompany(), acc.GetName(), acc.GetUrl(), value => acc.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
        }

        public async static Task DownloadSector(Portfolio portfolio, NameData name, Action<string, string, string> reportLogger)
        {
            portfolio.TryGetSector(name.Name, out var sector);
            await DownloadLatestValue(string.Empty, sector.GetName(), sector.GetUrl(), value => sector.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
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

        private async static Task DownloadPortfolioLatest(Portfolio portfo, Action<string, string, string> reportLogger)
        {
            foreach (var sec in portfo.GetSecurities())
            {
                await DownloadLatestValue(sec.GetCompany(), sec.GetName(), sec.GetUrl(), value => sec.UpdateSecurityData(value, reportLogger, DateTime.Today), reportLogger).ConfigureAwait(false);
            }
            foreach (var acc in portfo.GetBankAccounts())
            {
                await DownloadLatestValue(acc.GetCompany(), acc.GetName(), acc.GetUrl(), value => acc.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
            }
            foreach (var currency in portfo.GetCurrencies())
            {
                await DownloadLatestValue(string.Empty, currency.GetName(), currency.GetUrl(), value => currency.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
            }
            foreach (var sector in portfo.GetBenchMarks())
            {
                await DownloadLatestValue(string.Empty, sector.GetName(), sector.GetUrl(), value => sector.TryAddData(DateTime.Today, value, reportLogger), reportLogger).ConfigureAwait(false);
            }
        }

        public async static Task DownloadLatestValue(string company, string name, string url, Action<double> updateValue, Action<string, string, string> reportLogger)
        {
            string data = await DownloadFromURL(url, reportLogger).ConfigureAwait(false);
            if (string.IsNullOrEmpty(data))
            {
                reportLogger("Error", "Downloading", $"{company}-{name}: could not download data from {url}");
                return;
            }
            if (!ProcessDownloadString(url, data, reportLogger, out double value))
            {
                return;
            }

            updateValue(value);
        }

        public static async Task<string> DownloadFromURL(string url, Action<string, string, string> reportLogger)
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
                    HttpResponseMessage response = await client.GetAsync(newUrl).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (result != null)
                    {
                        output = result;
                    }
                }
                catch (Exception ex)
                {
                    reportLogger("Error", "Downloading", $"Failed to download from url {url}. Reason : {ex.Message}");
                    return output;
                }
            }

            return output;
        }

        private static bool ProcessDownloadString(string url, string data, Action<string, string, string> reportLogger, out double value)
        {
            value = double.NaN;
            switch (AddressType(url))
            {
                case WebsiteType.FT:
                    {
                        value = ProcessFromFT(data);
                        if (double.IsNaN(value))
                        {
                            reportLogger("Error", "Downloading", $"Could not download data from morningstar url: {url}");
                            return false;
                        }
                        return true;
                    }
                case WebsiteType.Yahoo:
                    {
                        value = ProcessFromYahoo(data);
                        if (double.IsNaN(value))
                        {
                            reportLogger("Error", "Downloading", $"Could not download data from Yahoo url: {url}");
                            return false;
                        }
                        return true;
                    }
                default:
                case WebsiteType.Morningstar:
                case WebsiteType.Google:
                case WebsiteType.TrustNet:
                case WebsiteType.Bloomberg:
                    reportLogger("Error", "Downloading", $"Url not of a currently implemented downloadable type: {url}");
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
                i = i / 100;
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
            string searchString = "data-reactid=\"33\"><span class=\"Trsdu(0.3s) Fw(b) Fz(36px) Mb(-4px) D(ib)\" data-reactid=\"34\">";
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
                    i = i / 100.0;
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
