﻿using System;
using System.Collections.Generic;

using Effanville.Common.Structure.MathLibrary.Vectors;

namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    internal class RelativeMovingAverage : IStockStatistic
    {
        private readonly int fFirstLength;
        private readonly int fSecondLength;

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

        /// <inheritdoc/>
        public StockDataStream DataType
        {
            get;
        }

        public RelativeMovingAverage(int numberDaysOne, int numberDaysTwo, StockDataStream dataStream, StockStatisticType statisticType)
        {
            BurnInTime = Math.Max(numberDaysOne, numberDaysTwo);
            fFirstLength = numberDaysOne;
            fSecondLength = numberDaysTwo;
            TypeOfStatistic = statisticType;
            DataType = dataStream;
        }

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock)
        {
            List<decimal> values = stock.Values(date, BurnInTime, 0, DataType);
            return Convert.ToDouble(DecimalVector.Mean(values, fFirstLength) - DecimalVector.Mean(values, fSecondLength));
        }
    }
}
