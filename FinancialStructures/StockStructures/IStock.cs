using System;
using System.Collections.Generic;
using FinancialStructures.NamingStructures;
using FinancialStructures.StockStructures.Implementation;

namespace FinancialStructures.StockStructures
{
    /// <summary>
    /// The contract for a Stock.
    /// </summary>
    public interface IStock
    {
        /// <summary>
        /// The code which refers to this Stock.
        /// </summary>
        string Ticker
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the stock in question.
        /// </summary>
        NameData Name
        {
            get;
            set;
        }

        /// <summary>
        /// Values associated to this stock in order earliest -> latest.
        /// </summary>
        List<StockDay> Valuations
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a value to the Stock. Note this does not sort the values, so <see cref="Sort"/> should be called after this.
        /// </summary>
        void AddValue(DateTime time, double open, double high, double low, double close, double volume);

        /// <summary>
        /// Sorts the values in the stock.
        /// </summary>
        void Sort();

        /// <summary>
        /// Calculates the value of the stock at the time specified.
        /// </summary>
        double Value(DateTime date, StockDataStream data = StockDataStream.Close);

        /// <summary>
        /// Returns a collection of values before and after the date specified.
        /// </summary>
        List<double> Values(DateTime date, int numberValuesBefore, int numberValuesAfter = 0, StockDataStream data = StockDataStream.Close);

        /// <summary>
        /// The earliest time held in the stock.
        /// </summary>
        DateTime EarliestTime();

        /// <summary>
        /// The latest time held in the Stock.
        /// </summary>
        DateTime LastTime();

        /// <summary>
        /// Calculates moving average of <paramref name="numberBefore"/> previous values and <paramref name="numberAfter"/> subsequent values from the day <paramref name="day"/> for the stock
        /// for the type of data <paramref name="data"/>.
        /// </summary>
        double MovingAverage(DateTime day, int numberBefore, int numberAfter, StockDataStream data);

        /// <summary>
        /// Calculates the maximum over the period requested.
        /// </summary>
        double Max(DateTime day, int numberBefore, int numberAfter, StockDataStream data);

        /// <summary>
        /// Calculates the minimum over the period requested.
        /// </summary>
        double Min(DateTime day, int numberBefore, int numberAfter, StockDataStream data);

        /// <summary>
        /// Calculate the stockastic statistic.
        /// </summary>
        double Stochastic(DateTime day, int length, int innerLength = 3);

        /// <summary>
        /// Need to have a moving average of this.
        /// </summary>
        double ADX(DateTime day, int length = 14);
    }
}
