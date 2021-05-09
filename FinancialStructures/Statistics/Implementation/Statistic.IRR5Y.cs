using System;
using FinancialStructures.Database.Statistics;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticIRR5Y : StatisticBase
    {
        internal StatisticIRR5Y()
            : base(Statistic.IRR5Y)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            Value = 100 * portfolio.IRR(account, name, DateTime.Today.AddMonths(-60), DateTime.Today);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = (100 * portfolio.IRRTotal(total, DateTime.Today.AddMonths(-60), DateTime.Today, name));
        }
    }
}
