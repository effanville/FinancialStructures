using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticRecentChange : StatisticBase
    {
        internal StatisticRecentChange()
            : base(Statistic.RecentChange)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            Value = portfolio.RecentChange(account, name);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            if (total == Totals.Company)
            {
                Value = portfolio.RecentChange(Totals.SecurityCompany, name.Company);
            }

            if (total == Totals.All)
            {
                Value = portfolio.RecentChange();
            }
        }
    }
}
