using System;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticNumberOfAccounts : StatisticBase
    {
        internal StatisticNumberOfAccounts()
            : base(Statistic.NumberOfAccounts)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            Value = 1;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = portfolio.Accounts(total, name).Count;
        }
    }
}
