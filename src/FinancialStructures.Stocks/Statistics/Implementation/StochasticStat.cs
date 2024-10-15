using System;

namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    internal class StochasticStat : IStockStatistic
    {
        /// <inheritdoc/>
        public bool IsNormalised => true;
        
        /// <inheritdoc/>
        public int BurnInTime { get; }

        /// <inheritdoc/>
        public StockDataStream DataType => StockDataStream.None;

        public StochasticStat(int numberDays)
        {
            BurnInTime = numberDays;
        }

        public StochasticStat(StockStatisticSettings settings)
        {
            BurnInTime = settings.Lag;
        }

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock) => Convert.ToDouble(stock.Stochastic(date, BurnInTime));
    }
}
