using System;
using System.Linq;
using System.Collections.Generic;

namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    internal class PreviousDayOpen : IStockStatistic
    {
        /// <inheritdoc/>
        public StockStatisticType TypeOfStatistic => StockStatisticType.PrevDayOpen;

        /// <inheritdoc/>
        public int BurnInTime => 1;

        /// <inheritdoc/>
        public StockDataStream DataType => StockDataStream.Open;

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock)
        {
            List<decimal> values = stock.Values(date, 1, 0, DataType);
            return Convert.ToDouble(values.First() / values.Last());
        }
    }
}
