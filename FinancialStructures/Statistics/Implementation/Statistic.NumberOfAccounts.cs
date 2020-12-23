using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
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
            Value = portfolio.SectorSecurities(name.Name).Count;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = portfolio.SectorSecurities(name.Name).Count;
        }
    }
}
