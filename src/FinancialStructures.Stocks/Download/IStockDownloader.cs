using System;
using System.Threading.Tasks;

namespace Effanville.FinancialStructures.Stocks.Download
{
    public interface IStockDownloader
    {
        /// <summary>
        /// The base url for the downloader.
        /// </summary>
        string BaseUrl { get; }

        string GetFinancialCode(string url);

        /// <summary>
        /// Try to get the financial data for the last day.
        /// </summary>
        Task<bool> TryGetLatestPriceData(IStock stock);

        /// <summary>
        /// Try to get the complete price history for the financial object
        /// between the dates specified.
        /// </summary>
        Task<bool> TryGetFullPriceHistory(
            IStock stock,
            DateTime firstDate,
            DateTime lastDate);
    }
}