using System;
using FinancialStructures.Database.Statistics;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticIRR6M : StatisticBase
    {
        internal StatisticIRR6M()
            : base(Statistic.IRR6M)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            Value = 100 * portfolio.IRR(account, name, DateTime.Today.AddMonths(-6), DateTime.Today);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = (100 * portfolio.TotalIRR(total, DateTime.Today.AddMonths(-6), DateTime.Today, name));
        }
    }
}
