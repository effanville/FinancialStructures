using System;
using System.Threading.Tasks;

using Effanville.Common.Structure.Reporting;

namespace Effanville.FinancialStructures.Download.Implementation
{
    /// <summary>
    /// An implementation of an <see cref="IPriceDownloader"/> for Morningstar websites.
    /// </summary>
    internal sealed class MorningstarDownloader : IPriceDownloader
    {
        /// <inheritdoc/>
        public string BaseUrl => "https://www.morningstar.co.uk/";

        /// <inheritdoc/>
        public async Task<bool> TryGetLatestPriceFromUrl(
            string url,
            string currency,
            Action<decimal> retrieveValueAction, 
            IReportLogger reportLogger = null)
        {
            string webData = await DownloadHelper.GetWebData(url,addCookie: false, reportLogger);
            if (string.IsNullOrEmpty(webData))
            {
                reportLogger?.Error(ReportLocation.Downloading.ToString(), $"Could not download data from {url}");
                return false;
            }

            decimal? value = Process(webData, $"<td class=\"line text\">{currency}", DownloadHelper.PenceName, 20);
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
            string urlSearchString = "id=";
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

        private static decimal? Process(string data, string poundsSearchString, string penceSearchString, int searchLength)
        {
            int penceValueIndex = data.IndexOf(penceSearchString);
            decimal? penceResult = DownloadHelper.ParseDataIntoNumber(data, penceValueIndex, penceSearchString.Length, searchLength, true);
            if (penceResult.HasValue)
            {
                return penceResult.Value / 100m;
            }

            int poundsValueIndex = data.IndexOf(poundsSearchString);
            decimal? poundsResult = DownloadHelper.ParseDataIntoNumber(data, poundsValueIndex, poundsSearchString.Length, searchLength, true);
            if (poundsResult.HasValue)
            {
                return poundsResult.Value;
            }

            return null;
        }
    }
}
