using System;
using System.Linq;
using System.Collections.Generic;

namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    internal class PreviousNDayValue : IStockStatistic
    {
        /// <inheritdoc/>
        public StockDataStream DataType
        {
            get;
        }

        /// <inheritdoc/>
        public StockStatisticType TypeOfStatistic
        {
            get;
        }

        /// <inheritdoc/>
        public int BurnInTime
        {
            get;
        }

        public PreviousNDayValue(int n, StockDataStream dataType, StockStatisticType typeOfStatistic)
        {
            BurnInTime = n;
            DataType = dataType;
            TypeOfStatistic = typeOfStatistic;
        }

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock)
        {
            List<decimal> values = stock.Values(date, BurnInTime, 0, DataType);
            return Convert.ToDouble(values.First() / values.Last());
        }
    }
}
