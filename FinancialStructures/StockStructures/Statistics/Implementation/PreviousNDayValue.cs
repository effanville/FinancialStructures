using System;
using System.Linq;

namespace FinancialStructures.StockStructures.Statistics.Implementation
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
            return Convert.ToDouble(stock.Values(date, BurnInTime, 0, DataType).First());
        }
    }
}
