using System;

using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticNumberUnits : StatisticBase
    {
        internal StatisticNumberUnits()
            : base(Statistic.NumberUnits)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            if (valueList is ISecurity security)
            {
                Value = (double)(security.Shares.ValueOnOrBefore(date)?.Value ?? 0.0m);
            }
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = 0.0;
        }
    }
}
