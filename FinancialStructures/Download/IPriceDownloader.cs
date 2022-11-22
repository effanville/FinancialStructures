using System;
using System.Threading.Tasks;
using Common.Structure.Reporting;
using FinancialStructures.NamingStructures;
using FinancialStructures.StockStructures;
using FinancialStructures.StockStructures.Implementation;

namespace FinancialStructures.Download
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
        /// Try to convert the name of the stock into a indentifier to download price data.
        /// </summary>
        /// <returns></returns>
        Task<bool> TryGetIdentifier(TwoName name,
            Action<string> getIdentifierAction,
            IReportLogger reportLogger = null);

        /// <summary>
        /// Try to get the latest price of the financial object from the
        /// url.
        /// </summary>
        Task<bool> TryGetLatestPriceFromUrl(
           string url,
           Action<decimal> retrieveValueAction,
           IReportLogger reportLogger = null);

        /// <summary>
        /// Try to get the latest price of the financial object from the
        /// code specifier.
        /// </summary>
        Task<bool> TryGetLatestPrice(
            string financialCode,
            Action<decimal> retrieveValueAction,
            IReportLogger reportLogger = null);

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
