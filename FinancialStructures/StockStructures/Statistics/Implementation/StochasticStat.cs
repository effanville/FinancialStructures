using System;

namespace FinancialStructures.StockStructures.Statistics.Implementation
{
    public class StochasticStat : IStockStatistic
    {
        public int BurnInTime
        {
            get;
        }

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

        public double Calculate(DateTime date, IStock stock)
        {
            return stock.Stochastic(date, BurnInTime);
        }
    }
}
