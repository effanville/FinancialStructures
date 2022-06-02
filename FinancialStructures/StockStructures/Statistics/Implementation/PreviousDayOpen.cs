using System;
using System.Linq;

namespace FinancialStructures.StockStructures.Statistics.Implementation
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
            return Convert.ToDouble(stock.Values(date, 1, 0, DataType).First());
        }
    }
}
