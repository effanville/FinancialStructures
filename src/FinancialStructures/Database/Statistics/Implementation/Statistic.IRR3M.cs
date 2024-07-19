using System;

using Effanville.FinancialStructures.Database.Extensions.Rates;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.ValueCalculators;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticIRR3M : StatisticBase
    {
        internal StatisticIRR3M()
            : base(Statistic.IRR3M)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            DateTime earlierTime = date.AddMonths(-3);
            Value = 100 * valueList.CalculateValue(
                IRRCalculators.DefaultCalculator(earlierTime, date),
                IRRCalculators.Calculators(portfolio, earlierTime, date),
                double.NaN);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = 100 * portfolio.TotalIRR(total, date.AddMonths(-3), date, name);
        }
    }
}
