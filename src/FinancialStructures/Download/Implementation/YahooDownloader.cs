using System;
using System.Text.Json;
using System.Threading.Tasks;

using Effanville.Common.Structure.Reporting;

namespace Effanville.FinancialStructures.Download.Implementation
{
    /// <summary>
    /// An implementation of an <see cref="IPriceDownloader"/> for Yahoo websites.
    /// </summary>
    internal sealed class YahooDownloader : IPriceDownloader
    {
        /// <inheritdoc/>
        public string BaseUrl => "https://query1.finance.yahoo.com/";

        internal YahooDownloader()
        {
        }

        /// <inheritdoc/>
        public async Task<bool> TryGetLatestPriceFromUrl(
            string url,
            string currency,
            Action<decimal> retrieveValueAction,
            IReportLogger reportLogger = null)
        {
            string financialCode = GetFinancialCode(url);
            url = BuildQueryUrl(BaseUrl, financialCode);
            return await TryGetPriceInternal(
                url,
                currency,
                retrieveValueAction,
                reportLogger);
        }

        private static async Task<bool> TryGetPriceInternal(string url,
            string currency,
            Action<decimal> retrieveValueAction, IReportLogger reportLogger = null)
        {
            string webData = await DownloadHelper.GetWebData(url, addCookie: false, reportLogger);
            if (string.IsNullOrEmpty(webData))
            {
                reportLogger?.Error("Downloading", $"Could not download data from {url}");
                return false;
            }

            // stockWebsite here is a csv file or json file
            string newLineSeparator = webData.Contains("\r\n") ? "\r\n" : "\n";
            string[] lines = webData.Split(newLineSeparator);
            if (lines.Length == 1 && lines[0].StartsWith("{"))
            {
                YahooStockData obj = JsonSerializer.Deserialize<YahooStockData>(lines[0]);
                if (obj != null)
                {
                    if (obj.spark.result == null && obj.spark.error != null)
                    {
                        return false;
                    }

                    string valueCurrency = obj.spark.result[0].response[0].meta.Currency;
                    double val = obj.spark.result[0].response[0].meta.RegularMarketPrice;
                    if (string.Equals(currency, "GBP") && string.Equals("GBp", valueCurrency))
                    {
                        val /= 100.0;
                    }
                    retrieveValueAction(Convert.ToDecimal(val));
                    return true;
                }
            }

            return false;
        }

        private static string BuildQueryUrl(string url, string identifier)
            => $"{url}v7/finance/spark?symbols={identifier}";

        /// <summary>
        /// Enables retrieval of the financial code specifier for the url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetFinancialCode(string url)
        {
            string urlSearchString = "/quote/";
            int startIndex = url.IndexOf(urlSearchString);
            int endIndex = url.IndexOfAny(new[] { '/', '?' }, startIndex + urlSearchString.Length);
            if (endIndex == -1)
            {
                endIndex = url.Length;
            }
            string code = url.Substring(startIndex + urlSearchString.Length, endIndex - startIndex - urlSearchString.Length);
            code = code.Replace("%5E", "^").Replace("%3D", "=").ToUpper();

            return code;
        }
    }
}
