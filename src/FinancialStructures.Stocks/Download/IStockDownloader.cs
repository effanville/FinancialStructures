using System;
using System.Threading.Tasks;

using Common.Structure.Reporting;

using FinancialStructures.Stocks.Implementation;

namespace FinancialStructures.Stocks.Download
{
    public interface IStockDownloader
    {
        /// <summary>
        /// The base url for the downloader.
        /// </summary>
        string BaseUrl
        {
            get;
        }

        string GetFinancialCode(string url);

        /*Task<bool> TryGetPrice(
            string financialCode,
            DateTime date,
            Action<decimal> retrieveValueAction,
            IReportLogger reportLogger = null);*/

        /// <summary>
        /// Try to get the financial data for the last day.
        /// </summary>
        Task<bool> TryGetLatestPriceData(
            string financialCode,
            Action<StockDay> retrieveValueAction,
            IReportLogger reportLogger = null);

        /*Task<bool> TryGetPriceHistory(
            string financialCode,
            DateTime firstDate,
            DateTime lastDate,
            TimeSpan recordInterval,
            Action<TimeList> getHistory,
            IReportLogger reportLogger = null);*/

        /// <summary>
        /// Try to get the complete price history for the financial object
        /// between the dates specified.
        /// </summary>
        Task<bool> TryGetFullPriceHistory(
            string financialCode,
            DateTime firstDate,
            DateTime lastDate,
            TimeSpan recordInterval,
            Action<IStock> getHistory,
            IReportLogger reportLogger = null);
    }
}