using System;
using System.Threading.Tasks;

using Effanville.Common.Structure.Reporting;

namespace Effanville.FinancialStructures.Download.Implementation
{
    /// <summary>
    /// An implementation of an <see cref="IPriceDownloader"/> for Yahoo websites.
    /// </summary>
    internal sealed class YahooDownloader : IPriceDownloader
    {
        private static readonly char DefaultCommaSeparator = ',';

        /// <inheritdoc/>
        public string BaseUrl => "https://uk.finance.yahoo.com/";

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
            return await TryGetPriceInternal(url, financialCode, retrieveValueAction, reportLogger);
        }

        public async Task<bool> TryGetLatestPrice(string financialCode, Action<decimal> retrieveValueAction, IReportLogger reportLogger = null)
        {
            string url = BuildQueryUrl(BaseUrl, financialCode);
            return await TryGetPriceInternal(url, financialCode, retrieveValueAction, reportLogger);
        }

        private static async Task<bool> TryGetPriceInternal(string url, string financialCode, Action<decimal> retrieveValueAction, IReportLogger reportLogger = null)
        {
            string webData = await DownloadHelper.GetWebData(url, addCookie: false, reportLogger);
            if (string.IsNullOrEmpty(webData))
            {
                reportLogger?.Error("Downloading", $"Could not download data from {url}");
                return false;
            }

            decimal? value = GetValue(webData, financialCode);
            if (!value.HasValue)
            {
                return false;
            }

            retrieveValueAction(value.Value);
            return true;
        }

        private static string BuildQueryUrl(string url, string identifier)
        {
            return $"{url}/quote/{identifier}";
        }

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

        private static decimal? GetValue(string webData, string financialCode)
        {
            int number = 2;
            int poundsIndex;
            string searchString;
            do
            {
                searchString = $"data-symbol=\"{financialCode}\" data-testid=\"qsp-price\" data-field=\"regularMarketPrice\" data-trend=\"none\" data-pricehint=\"{number}\"";
                poundsIndex = webData.IndexOf(searchString);
                number++;
            }
            while (poundsIndex == -1 && number < 100);

            decimal? value = DownloadHelper.ParseDataIntoNumber(webData, poundsIndex, searchString.Length, 20, true);
            if (value.HasValue)
            {
                int ind = webData.IndexOf("Delayed Quote</span>");
                if (webData.Contains("Currency in GBp") || (ind > 0 && webData.Substring(ind, 200).Contains("GBp")))
                {
                    return value.Value / 100.0m;
                }

                return value.Value;
            }

            return null;
        }
    }
}
