using System;
using System.Threading.Tasks;
using Effanville.Common.Structure.Reporting;
using Effanville.Common.Structure.WebAccess;

namespace Effanville.FinancialStructures.Download.Implementation
{
    /// <summary>
    /// An implementation of an <see cref="IPriceDownloader"/> for Bloomberg websites.
    /// </summary>
    internal sealed class BloombergDownloader : IPriceDownloader
    {
        private readonly IReportLogger _logger;
        private readonly WebDownloader _webDownloader;

        /// <inheritdoc/>
        public string BaseUrl => "https://www.bloomberg.com/";

        public BloombergDownloader(IReportLogger logger, WebDownloader webDownloader)
        {
            _logger = logger;
            _webDownloader = webDownloader;
        }

        /// <inheritdoc/>
        public async Task<bool> TryGetLatestPriceFromUrl(
            string url,
            string currency,
            Action<decimal> retrieveValueAction)
        {
            string webData = await _webDownloader.GetWebData(url, addCookie: true);
            if (string.IsNullOrEmpty(webData))
            {
                _logger?.Error(ReportLocation.Downloading.ToString(), $"Could not download data from {url}");
                return false;
            }

            decimal? value = Process(webData, 200, _logger);
            if (!value.HasValue)
            {
                return false;
            }

            retrieveValueAction(value.Value);
            return true;
        }

        /// <summary>
        /// Enables retrieval of the financial code specifier for the url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetFinancialCode(string url)
        {
            string urlSearchString = "quote/";
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

        private static decimal? Process(string data, int searchLength, IReportLogger reportLogger)
        {
            string searchString =
                "<div class=\"currentPrice_currentPriceContainer__nC8vw\"><div data-component=\"sized-price\" class=\"sized-price media-ui-SizedPrice_extraLarge-05pKbJRbUH8-\">";
            int penceValueIndex = data.IndexOf(searchString);
            decimal? penceResult = DownloadHelper.ParseDataIntoNumber(data, penceValueIndex, searchString.Length, searchLength, true);
            if (penceResult.HasValue)
            {
                return penceResult.Value;
            }

            return null;
        }
    }
}