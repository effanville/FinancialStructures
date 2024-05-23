using System;

using Effanville.FinancialStructures.Database.Extensions.Rates;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.ValueCalculators;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticIRR6M : StatisticBase
    {
        internal StatisticIRR6M()
            : base(Statistic.IRR6M)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            var earlierTime = date.AddMonths(-6);
            Value = 100 * valueList.CalculateValue(
                IRRCalculators.DefaultCalculator(earlierTime, date),
                IRRCalculators.Calculators(portfolio, earlierTime, date),
                double.NaN);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = 100 * portfolio.TotalIRR(total, date.AddMonths(-6), date, name);
        }
    }
}
