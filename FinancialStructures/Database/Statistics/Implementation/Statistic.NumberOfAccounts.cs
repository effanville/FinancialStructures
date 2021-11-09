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
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            Value = portfolio.SectorAccounts(account, name).Count;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = portfolio.SectorAccounts(total.ToAccount(), name).Count;
        }
    }
}
