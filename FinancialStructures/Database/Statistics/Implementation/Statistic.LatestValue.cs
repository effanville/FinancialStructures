using System;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticLatestValue : StatisticBase
    {
        internal StatisticLatestValue()
            : base(Statistic.LatestValue)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            Value = (double)portfolio.Value(account, name, DateTime.Today);
            fCurrency = portfolio.BaseCurrency;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = (double)portfolio.TotalValue(total, DateTime.Today, name);
            fCurrency = portfolio.BaseCurrency;
        }
    }
}
