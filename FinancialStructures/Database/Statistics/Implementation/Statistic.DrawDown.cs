using System;
using FinancialStructures.Database.Extensions.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticDrawDown : StatisticBase
    {
        internal StatisticDrawDown()
            : base(Statistic.DrawDown)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            Value = portfolio.Drawdown(account, name);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = portfolio.TotalDrawdown(total, name);
        }
    }
}
