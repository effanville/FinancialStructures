using System;
using FinancialStructures.Database.Extensions.Rates;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticIRR6M : StatisticBase
    {
        internal StatisticIRR6M()
            : base(Statistic.IRR6M)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            Value = 100 * portfolio.IRR(account, name, date.AddMonths(-6), date);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = 100 * portfolio.TotalIRR(total, date.AddMonths(-6), date, name);
        }
    }
}
