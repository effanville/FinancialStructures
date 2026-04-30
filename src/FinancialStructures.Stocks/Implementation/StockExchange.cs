using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.NamingStructures;

using Nager.Date;

namespace Effanville.FinancialStructures.Stocks.Implementation
{
    /// <summary>
    /// Simulates a stock exchange.
    /// </summary>
    public class StockExchange : IStockExchange
    {
        /// <inheritdoc/>
        public string ExchangeIdentifier { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

        /// <inheritdoc/>
        public CountryCode CountryDateCode { get; set; } = CountryCode.GB;

        public TimeOnly ExchangeOpen { get; set; } = new TimeOnly(8, 0, 0);

        public TimeOnly ExchangeClose { get; set; } = new TimeOnly(16, 30, 0);

        public DateTime ExchangeOpenInUtc(DateTime date)
        {
            var dateTime = date.Date.Add(ExchangeOpen.ToTimeSpan());
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZone);
        }

        public DateTime ExchangeCloseInUtc(DateTime date)
        {
            var dateTime = date.Date.Add(ExchangeClose.ToTimeSpan());
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZone);
        }

        /// <inheritdoc/>
        public List<Stock> Stocks
        {
            get;
            set;
        } = new List<Stock>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StockExchange()
        {
        }

        /// <inheritdoc/>
        public decimal GetValue(string ticker, DateTime date, StockDataStream datatype = StockDataStream.Close)
        {
            foreach (Stock stock in Stocks)
            {
                if (stock.Name.Ticker.Equals(ticker))
                {
                    return Convert.ToDecimal(stock.Value(date, datatype));
                }
            }

            return 0.0m;
        }

        /// <inheritdoc/>
        public decimal GetValue(TwoName name, DateTime date, StockDataStream datatype = StockDataStream.Close)
        {
            foreach (Stock stock in Stocks)
            {
                if (stock.Name.IsEqualTo(name))
                {
                    return Convert.ToDecimal(stock.Value(date, datatype));
                }
            }

            return 0.0m;
        }

        /// <inheritdoc/>
        public StockDay GetCandle(TwoName name, DateTime date)
        {
            foreach (Stock stock in Stocks)
            {
                if (stock.Name.IsEqualTo(name))
                {
                    return stock.GetData(date);
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public DateTime EarliestDate()
        {
            DateTime earliest = Stocks[0].EarliestTime();

            for (int stockIndex = 1; stockIndex < Stocks.Count; stockIndex++)
            {
                DateTime stockEarliest = Stocks[stockIndex].EarliestTime();
                if (stockEarliest < earliest)
                {
                    earliest = stockEarliest;
                }
            }

            return earliest;
        }

        /// <inheritdoc/>
        public DateTime LatestEarliestDate()
        {
            DateTime earliest = Stocks[0].EarliestTime();

            for (int stockIndex = 1; stockIndex < Stocks.Count; stockIndex++)
            {
                DateTime stockEarliest = Stocks[stockIndex].EarliestTime();
                if (stockEarliest > earliest)
                {
                    earliest = stockEarliest;
                }
            }

            return earliest;
        }

        /// <inheritdoc/>
        public DateTime LastDate()
        {
            DateTime last = Stocks[0].LastTime();

            for (int stockIndex = 1; stockIndex < Stocks.Count; stockIndex++)
            {
                DateTime stockLast = Stocks[stockIndex].LastTime();
                if (stockLast < last)
                {
                    last = stockLast;
                }
            }

            return last;
        }

        /// <inheritdoc/>
        public bool CheckValidity()
        {
            if (Stocks.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}