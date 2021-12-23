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
            Value = portfolio.SectorAccounts(account, name).Count;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = portfolio.SectorAccounts(total.ToAccount(), name).Count;
        }
    }
}
