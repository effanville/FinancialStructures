using System;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticUnitPrice : StatisticBase
    {
        internal StatisticUnitPrice()
            : base(Statistic.UnitPrice)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList desired))
            {
                Value = 0.0;
            }
            if (desired is ISecurity security)
            {
                Value = security.UnitPrice.NearestEarlierValue(DateTime.Today).Value;
            }
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            if (total == Totals.Company)
            {
                Value = 0;
            }

            if (total == Totals.All)
            {
                Value = 0;
            }
        }
    }
}
