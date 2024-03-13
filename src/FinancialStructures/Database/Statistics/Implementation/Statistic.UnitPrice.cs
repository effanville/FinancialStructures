using System;

using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticUnitPrice : StatisticBase
    {
        internal StatisticUnitPrice()
            : base(Statistic.UnitPrice)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList desired))
            {
                Value = 0.0;
            }
            if (desired is ISecurity security)
            {
                Value = (double)(security.UnitPrice.ValueOnOrBefore(date)?.Value ?? 0.0m);
                fCurrency = security.Names.Currency ?? portfolio.BaseCurrency;
            }
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
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
