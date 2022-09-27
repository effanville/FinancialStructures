using System;

namespace FinancialStructures.StockStructures.Statistics.Implementation
{
    internal class StochasticStat : IStockStatistic
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

        public StockDataStream DataType => StockDataStream.None;

        public StochasticStat(int numberDays, StockStatisticType statisticType)
        {
            BurnInTime = numberDays;
            TypeOfStatistic = statisticType;
        }

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock)
        {
            return Convert.ToDouble(stock.Stochastic(date, BurnInTime));
        }
    }
}
