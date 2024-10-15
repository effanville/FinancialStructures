using System;
using System.Collections.Generic;

using Effanville.Common.Structure.MathLibrary.Vectors;

namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    internal class MovingAverage : IStockStatistic
    {
        /// <inheritdoc/>
        public bool IsNormalised => false;
        
        /// <inheritdoc/>
        public StockDataStream DataType { get; }

        /// <inheritdoc/>
        public int BurnInTime { get; }

        public MovingAverage(int numberDays, StockDataStream dataStream)
        {
            BurnInTime = numberDays;
            DataType = dataStream;
        }
        
        public MovingAverage(StockStatisticSettings settings)
        {
            BurnInTime = settings.Lag;
            DataType = settings.PriceType;
        }

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock)
        {
            List<decimal> values = stock?.Values(date, BurnInTime, 0, DataType);
            if (values == null)
            {
                return double.NaN;
            }
            
            return Convert.ToDouble(DecimalVector.Mean(values, BurnInTime));
        }
    }
}
