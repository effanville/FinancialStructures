namespace FinancialStructures.StockStructures.Statistics
{
    /// <summary>
    /// The admissible types of stock statistic.
    /// </summary>
    public enum StockStatisticType
    {
        /// <summary>
        /// The open price today.
        /// </summary>
        TodayOpen,

        /// <summary>
        /// The open price on the previous day.
        /// </summary>
        PrevDayOpen,

        /// <summary>
        /// The open price on the day before the previous day.
        /// </summary>
        PrevTwoOpen,

        /// <summary>
        /// The open price 3 days ago.
        /// </summary>
        PrevThreeOpen,

        /// <summary>
        /// The open price 4 days ago.
        /// </summary>
        PrevFourOpen,

        /// <summary>
        /// The open price five days ago.
        /// </summary>
        PrevFiveOpen,

        /// <summary>
        /// The close price today.
        /// </summary>
        TodayClose,

        /// <summary>
        /// The close price yesterday.
        /// </summary>
        PrevDayClose,

        /// <summary>
        /// The close price two days ago.
        /// </summary>
        PrevTwoClose,

        /// <summary>
        /// The close price three days ago.
        /// </summary>
        PrevThreeClose,

        /// <summary>
        /// The close price four days ago.
        /// </summary>
        PrevFourClose,

        /// <summary>
        /// The close price five days ago.
        /// </summary>
        PrevFiveClose,

        /// <summary>
        /// The high value yesterday.
        /// </summary>
        PrevDayHigh,

        /// <summary>
        /// The low value yesterday.
        /// </summary>
        PrevDayLow,

        /// <summary>
        /// Considered as 5 days of the open.
        /// </summary>
        MovingAverageWeek,

        /// <summary>
        /// Considered as 20 days of the open.
        /// </summary>
        MovingAverageMonth,

        /// <summary>
        /// The moving average of the past 50 days of the open.
        /// </summary>
        MovingAverageHalfYear,

        /// <summary>
        /// The average of the past week compared to the average over the past month.
        /// </summary>
        RelativeMovingAverageWeekMonth,

        /// <summary>
        /// The average of the past month compared to the average over the past half year.
        /// </summary>
        RelativeMovingAverageMonthHalfYear,

        /// <summary>
        /// The average of the past week compared to the average over the past half year.
        /// </summary>
        RelativeMovingAverageWeekHalfYear,

        /// <summary>
        /// The value of the ADX over the past two weeks.
        /// </summary>
        ADXStatTwoWeek,

        /// <summary>
        /// The stochastic statistic over the past two weeks.
        /// </summary>
        StochasticTwoWeek,

        /// <summary>
        /// The stochastic statistic over the past four weeks.
        /// </summary>
        StochasticFourWeek
    }
}
