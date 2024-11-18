namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    /// <summary>
    /// Statistic for the <see cref="Statistic.IRR1Y"/> enum value.
    /// This calculates the Internal rate of return of the object
    /// in the portfolio.
    /// </summary>
    internal class StatisticIRR1Y : StatisticIRRTimePeriod
    {
        /// <summary>
        /// Default constructor setting the statistic type.
        /// </summary>
        internal StatisticIRR1Y()
            : base(Statistic.IRR1Y, -12)
        {
        }
    }
}
