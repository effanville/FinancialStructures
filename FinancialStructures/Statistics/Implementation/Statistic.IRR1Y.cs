using System;
using FinancialStructures.Database.Statistics;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    /// <summary>
    /// Statistic for the <see cref="Statistic.IRR1Y"/> enum value.
    /// This calculates the Internal rate of return of the object
    /// in the portfolio.
    /// </summary>
    internal class StatisticIRR1Y : StatisticBase
    {
        /// <summary>
        /// Default constructor setting the statistic type.
        /// </summary>
        internal StatisticIRR1Y()
            : base(Statistic.IRR1Y)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            Value = 100 * portfolio.IRR(account, name, DateTime.Today.AddMonths(-12), DateTime.Today);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = (100 * portfolio.IRRTotal(total, DateTime.Today.AddMonths(-12), DateTime.Today, name));
        }
    }
}
