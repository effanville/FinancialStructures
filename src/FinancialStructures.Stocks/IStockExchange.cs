using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Common.Structure.Reporting;

using FinancialStructures.NamingStructures;
using FinancialStructures.Stocks.Implementation;
using Nager.Date;

namespace FinancialStructures.Stocks
{
    /// <summary>
    /// Contains the contract for a Stock exchange.
    /// </summary>
    public interface IStockExchange
    {
        /// <summary>
        /// The stock exchange name, eg London stock exchange.
        /// </summary>
        string ExchangeIdentifier
        {
            get;
        }
        
        /// <summary>
        /// The stock exchange name, eg London stock exchange.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// The time zone the exchange is in
        /// </summary>
        TimeZoneInfo TimeZone
        {
            get;
        }

        /// <summary>
        /// The code for the country to determine trading days.
        /// </summary>
        CountryCode CountryDateCode
        {
            get;
        }

        /// <summary>
        /// The stocks that are part of this exchange.
        /// </summary>
        List<Stock> Stocks
        {
            get;
        }

        DateTime ExchangeOpenInUtc(DateTime date);

        DateTime ExchangeCloseInUtc(DateTime date);

        /// <summary>
        /// Returns the value of the stock indexed by ticker on the date desired.
        /// </summary>
        decimal GetValue(string ticker, DateTime date, StockDataStream datatype = StockDataStream.Close);

        /// <summary>
        /// Returns the value of the stock with the desired name on the date desired.
        /// </summary>
        decimal GetValue(TwoName name, DateTime date, StockDataStream datatype = StockDataStream.Close);

        /// <summary>
        /// Returns the complete candle preceding the time specified.
        /// </summary>
        StockDay GetCandle(TwoName name, DateTime date);

        /// <summary>
        /// Returns the earliest date held in the stock data.
        /// </summary>
        DateTime EarliestDate();

        /// <summary>
        /// Returns the latest date out of all first dates of each stock.
        /// </summary>
        DateTime LatestEarliestDate();

        /// <summary>
        /// Returns the last date held in the exchange.
        /// </summary>
        DateTime LastDate();

        /// <summary>
        /// Ensures that the data in the StockExchange is valid.
        /// </summary>
        bool CheckValidity();

        /// <summary>
        /// Instantiates a <see cref="StockExchange"/> from a file
        /// where each line is
        /// Ticker, Company,Name,Url
        /// </summary>
        void Configure(string stockFilePath, IReportLogger logger = null);

        /// <summary>
        /// Instantiates a <see cref="StockExchange"/> from a file
        /// where each line is
        /// Ticker, Company,Name,Url
        /// </summary>
        void Configure(string stockFilePath, IFileSystem fileSystem, IReportLogger logger = null);

        /// <summary>
        /// Downloads data for the stock exchange between the dates provided.
        /// </summary>
        Task Download(DateTime startDate, DateTime endDate, IReportLogger reportLogger = null);

        /// <summary>
        /// Downloads data for the stock exchange on the latest date possible.
        /// </summary>
        Task Download(IReportLogger reportLogger = null);
    }
}
