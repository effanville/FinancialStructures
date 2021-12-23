using System;
using FinancialStructures.Database.Extensions.Rates;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticIRR5Y : StatisticBase
    {
        internal StatisticIRR5Y()
            : base(Statistic.IRR5Y)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            Value = 100 * portfolio.IRR(account, name, date.AddMonths(-60), date);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = (100 * portfolio.TotalIRR(total, date.AddMonths(-60), date, name));
        }
    }
}
