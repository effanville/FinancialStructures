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
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            Value = 0.0;
            if (valueList is ISecurity security)
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
