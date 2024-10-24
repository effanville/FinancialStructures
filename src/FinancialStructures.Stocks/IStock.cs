﻿using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.Stocks.Implementation;

namespace Effanville.FinancialStructures.Stocks
{
    /// <summary>
    /// The contract for a Stock.
    /// </summary>
    public interface IStock
    {
        /// <summary>
        /// The name of the stock in question.
        /// </summary>
        NameData Name { get; set; }
        
        /// <summary>
        /// The fundamental data associated with this stock.
        /// </summary>
        StockFundamentalData Fundamentals { get; }
        
        /// <summary>
        /// Values associated to this stock in order earliest -> latest.
        /// </summary>
        List<StockDay> Valuations { get; }

        /// <summary>
        /// Adds a value to the Stock. Note this does not sort the values, so <see cref="Sort"/> should be called after this.
        /// </summary>
        void AddValue(DateTime time, decimal open, decimal high, decimal low, decimal close, decimal volume);

        /// <summary>
        /// Sorts the values in the stock.
        /// </summary>
        void Sort();

        /// <summary>
        /// Calculates the value of the stock at the time specified.
        /// </summary>
        decimal Value(DateTime date, StockDataStream data = StockDataStream.Close);

        /// <summary>
        /// Returns a collection of values before and after the date specified.
        /// </summary>
        List<decimal> Values(DateTime date, int numberValuesBefore, int numberValuesAfter = 0, StockDataStream data = StockDataStream.Close);

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
        decimal MovingAverage(DateTime day, int numberBefore, int numberAfter, StockDataStream data);

        /// <summary>
        /// Calculates the maximum over the period requested.
        /// </summary>
        decimal Max(DateTime day, int numberBefore, int numberAfter, StockDataStream data);

        /// <summary>
        /// Calculates the minimum over the period requested.
        /// </summary>
        decimal Min(DateTime day, int numberBefore, int numberAfter, StockDataStream data);

        /// <summary>
        /// Calculate the stockastic statistic.
        /// </summary>
        decimal Stochastic(DateTime day, int length, int innerLength = 3);

        /// <summary>
        /// Need to have a moving average of this.
        /// </summary>
        decimal ADX(DateTime day, int length = 14);
    }
}
