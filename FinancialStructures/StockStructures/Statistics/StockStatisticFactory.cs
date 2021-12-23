using FinancialStructures.StockStructures.Statistics.Implementation;

namespace FinancialStructures.StockStructures.Statistics
{
    /// <summary>
    /// Generator for various statistic calculators for a stock.
    /// </summary>
    public static class StockStatisticFactory
    {
        /// <summary>
        /// Generates a statistic calculator of the type specified.
        /// </summary>
        public static IStockStatistic Create(StockStatisticType stockStatistic)
        {
            switch (stockStatistic)
            {
                case StockStatisticType.TodayOpen:
                    return new PreviousNDayValue(0, StockDataStream.Open, stockStatistic);
                case StockStatisticType.PrevTwoOpen:
                    return new PreviousNDayValue(2, StockDataStream.Open, stockStatistic);
                case StockStatisticType.PrevThreeOpen:
                    return new PreviousNDayValue(3, StockDataStream.Open, stockStatistic);
                case StockStatisticType.PrevFourOpen:
                    return new PreviousNDayValue(4, StockDataStream.Open, stockStatistic);
                case StockStatisticType.PrevFiveOpen:
                    return new PreviousNDayValue(5, StockDataStream.Open, stockStatistic);
                case StockStatisticType.TodayClose:
                    return new PreviousNDayValue(0, StockDataStream.Close, stockStatistic);
                case StockStatisticType.PrevDayClose:
                    return new PreviousNDayValue(1, StockDataStream.Close, stockStatistic);
                case StockStatisticType.PrevTwoClose:
                    return new PreviousNDayValue(2, StockDataStream.Close, stockStatistic);
                case StockStatisticType.PrevThreeClose:
                    return new PreviousNDayValue(3, StockDataStream.Close, stockStatistic);
                case StockStatisticType.PrevFourClose:
                    return new PreviousNDayValue(4, StockDataStream.Close, stockStatistic);
                case StockStatisticType.PrevFiveClose:
                    return new PreviousNDayValue(5, StockDataStream.Close, stockStatistic);
                case StockStatisticType.PrevDayHigh:
                    return new PreviousNDayValue(1, StockDataStream.High, stockStatistic);
                case StockStatisticType.PrevDayLow:
                    return new PreviousNDayValue(1, StockDataStream.Low, stockStatistic);
                case StockStatisticType.MovingAverageWeek:
                    return new MovingAverage(5, StockDataStream.Open, stockStatistic);
                case StockStatisticType.MovingAverageMonth:
                    return new MovingAverage(20, StockDataStream.Open, stockStatistic);
                case StockStatisticType.MovingAverageHalfYear:
                    return new MovingAverage(50, StockDataStream.Open, stockStatistic);
                case StockStatisticType.RelativeMovingAverageWeekMonth:
                    return new RelativeMovingAverage(5, 20, StockDataStream.Open, stockStatistic);
                case StockStatisticType.RelativeMovingAverageMonthHalfYear:
                    return new RelativeMovingAverage(20, 50, StockDataStream.Open, stockStatistic);
                case StockStatisticType.RelativeMovingAverageWeekHalfYear:
                    return new RelativeMovingAverage(5, 50, StockDataStream.Open, stockStatistic);
                case StockStatisticType.ADXStatTwoWeek:
                    return new ADXStat(14, stockStatistic);
                case StockStatisticType.StochasticTwoWeek:
                    return new StochasticStat(14, stockStatistic);
                case StockStatisticType.StochasticFourWeek:
                    return new StochasticStat(28, stockStatistic);
                case StockStatisticType.PrevDayOpen:
                default:
                    return new PreviousDayOpen();
            }
        }
    }
}
