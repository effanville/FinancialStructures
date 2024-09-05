using System;
using System.Threading.Tasks;

using Effanville.Common.Structure.Reporting;

namespace Effanville.FinancialStructures.Download
{
    /// <summary>
    /// Provides general mechanisms for downloading financial instrument price data from a website.
    /// </summary>
    public interface IPriceDownloader
    {
        /// <summary>
        /// The base url for the downloader.
        /// </summary>
        string BaseUrl
        {
            get;
        }

        /// <summary>
        /// Try to get the latest price of the financial object from the
        /// url.
        /// </summary>
        Task<bool> TryGetLatestPriceFromUrl(
            string url,
            string currency,
            Action<decimal> retrieveValueAction,
            IReportLogger reportLogger = null);
    }
}