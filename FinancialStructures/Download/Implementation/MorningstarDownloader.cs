using System;
using System.Threading.Tasks;
using Common.Structure.Reporting;
using FinancialStructures.NamingStructures;
using FinancialStructures.StockStructures;
using FinancialStructures.StockStructures.Implementation;

namespace FinancialStructures.Download.Implementation
{
    /// <summary>
    /// An implementation of an <see cref="IPriceDownloader"/> for Morningstar websites.
    /// </summary>
    internal sealed class MorningstarDownloader : IPriceDownloader
    {
        private static readonly string fBaseUrl = "https://www.morningstar.co.uk/";
        /// <inheritdoc/>
        public Task<bool> TryGetIdentifier(TwoName name, Action<string> getIdentifierAction, IReportLogger reportLogger = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<bool> TryGetLatestPriceFromUrl(string url, Action<decimal> retrieveValueAction, IReportLogger reportLogger = null)
        {
            string webData = await DownloadHelper.GetWebData(url, reportLogger);
            if (string.IsNullOrEmpty(webData))
            {
                _ = reportLogger?.LogUsefulError(ReportLocation.Downloading, $"Could not download data from {url}");
                return false;
            }

            decimal? value = Process(webData, $"<td class=\"line text\">{DownloadHelper.PoundsName}", DownloadHelper.PenceName, 20);
            if (!value.HasValue)
            {
                return false;
            }

            retrieveValueAction(value.Value);
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> TryGetLatestPrice(string financialCode, Action<decimal> retrieveValueAction, IReportLogger reportLogger = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryGetLatestPriceData(string financialCode, Action<StockDay> retrieveValueAction, IReportLogger reportLogger = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<bool> TryGetFullPriceHistory(string financialCode, DateTime firstDate, DateTime lastDate, TimeSpan recordInterval, Action<IStock> getHistory, IReportLogger reportLogger = null)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Enables retrieval of the financial code specifier for the url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFinancialCode(string url)
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
