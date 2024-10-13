using System;
using Effanville.FinancialStructures.Database.Extensions.Rates;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticIRRTimePeriod : StatisticBase
    {
        private readonly int _lookbackWindowInMonths;

        internal StatisticIRRTimePeriod(Statistic stat, int lookbackWindowInMonths)
            : base(stat)
        {
            _lookbackWindowInMonths = lookbackWindowInMonths;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            DateTime earlierTime = date.AddMonths(_lookbackWindowInMonths);
            Value = 100 * valueList.CalculateValue(
                vl =>
                {
                    ICurrency currency = portfolio.Currency((IValueList)vl);
                    return vl.IRR(currency, earlierTime, date);
                },
                defaultValue: double.NaN);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = 100 * portfolio.TotalIRR(total, date.AddMonths(_lookbackWindowInMonths), date, name);
        }
    }
}