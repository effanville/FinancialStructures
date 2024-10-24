﻿using System;
using System.Collections.Generic;

using Effanville.Common.Structure.MathLibrary.Vectors;

namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    internal class MovingAverage : IStockStatistic
    {
        /// <inheritdoc/>
        public StockDataStream DataType
        {
            get;
        }

        /// <inheritdoc/>
        public int BurnInTime
        {
            get;
        }

        /// <inheritdoc/>
        public StockStatisticType TypeOfStatistic
        {
            get;
        }

        public MovingAverage(int numberDays, StockDataStream dataStream, StockStatisticType typeOfStatistic)
        {
            BurnInTime = numberDays;
            TypeOfStatistic = typeOfStatistic;
            DataType = dataStream;
        }

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock)
        {
            List<decimal> values = stock.Values(date, BurnInTime, 0, DataType);
            return Convert.ToDouble(DecimalVector.Mean(values, BurnInTime));
        }
    }
}
