using System;
using System.Collections.Generic;

using Effanville.Common.Structure.MathLibrary.Vectors;

namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    internal class RelativeMovingAverage : IStockStatistic
    {
        private readonly int _firstLength;
        private readonly int _secondLength;
        private readonly bool _fraction;

        /// <inheritdoc/>
        public bool IsNormalised => false;
        
        /// <inheritdoc/>
        public int BurnInTime { get; }

        /// <inheritdoc/>
        public StockDataStream DataType { get; }

        public RelativeMovingAverage(int numberDaysOne, int numberDaysTwo, StockDataStream dataStream)
        {
            BurnInTime = Math.Max(numberDaysOne, numberDaysTwo);
            _firstLength = numberDaysOne;
            _secondLength = numberDaysTwo;
            DataType = dataStream;
        }

        public RelativeMovingAverage(StockStatisticSettings settings)
        {
            DataType = settings.PriceType;
            BurnInTime = settings.Lag;
            
            if (settings is MovingAverageSettings derivedSettings)
            {
                _firstLength = derivedSettings.NumberAverageDays;
                _secondLength = derivedSettings.NumberAverageDaysTwo;
                _fraction = derivedSettings.Fraction;
            }
        }

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock)
        {
            List<decimal> values = stock.Values(date, BurnInTime, 0, DataType);
            decimal firstAverage = DecimalVector.Mean(values, _firstLength);
            decimal secondAverage = DecimalVector.Mean(values, _secondLength);
            if (!_fraction)
            {
                return Convert.ToDouble(firstAverage - secondAverage);
            }

            return Convert.ToDouble(firstAverage / secondAverage);
        }
    }
}
