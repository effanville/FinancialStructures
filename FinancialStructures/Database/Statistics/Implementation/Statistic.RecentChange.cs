using System;
using FinancialStructures.Database.Extensions.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticRecentChange : StatisticBase
    {
        internal StatisticRecentChange()
            : base(Statistic.RecentChange)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            Value = (double)portfolio.RecentChange(account, name);
            fCurrency = portfolio.BaseCurrency;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = (double)portfolio.RecentChange(total, name);
            fCurrency = portfolio.BaseCurrency;
        }
    }
}
