using System;
using System.Collections.Generic;
using System.Linq;

namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    internal class PreviousNDayRelativeValue : IStockStatistic
    {
        public int NumberDaysPriorValue { get; protected init; }
        
        /// <inheritdoc/>
        public bool IsNormalised => true;
        
        /// <inheritdoc/>
        public StockDataStream DataType { get; }

        /// <inheritdoc/>
        public int BurnInTime { get; }

        public PreviousNDayRelativeValue(int n, StockDataStream dataType)
        {
            BurnInTime = n;
            DataType = dataType;
        }
        
        public PreviousNDayRelativeValue(StockStatisticSettings settings)
        {
            BurnInTime = settings.Lag;
            DataType = settings.PriceType;
            if (settings is PreviousValueSettings previousValueSettings)
            {
                NumberDaysPriorValue = previousValueSettings.NumberDaysPriorValue;
            }
        }

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock)
        {
            List<decimal> values = stock?.Values(date, BurnInTime + NumberDaysPriorValue - 1, 0, DataType);
            if (values == null)
            {
                return double.NaN;
            }
            List<decimal> requiredValues = values.Skip(NumberDaysPriorValue).ToList();
            return Convert.ToDouble(requiredValues.Last() / requiredValues.Average());
        }
    }
}
