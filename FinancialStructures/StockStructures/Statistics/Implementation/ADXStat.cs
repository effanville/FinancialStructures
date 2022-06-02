using System;

namespace FinancialStructures.StockStructures.Statistics.Implementation
{
    internal class ADXStat : IStockStatistic
    {
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
        public StockDataStream DataType => StockDataStream.None;

        public ADXStat(int numberDays, StockStatisticType statisticType)
        {
            BurnInTime = numberDays;
            TypeOfStatistic = statisticType;
        }

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock)
        {
            return Convert.ToDouble(stock.ADX(date, BurnInTime));
        }
    }
}
