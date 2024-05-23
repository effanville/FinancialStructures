using System;
using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticDrawDown : StatisticBase
    {
        internal StatisticDrawDown()
            : base(Statistic.DrawDown)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            DateTime firstDate = valueList.FirstValue()?.Day ?? DateTime.MaxValue;
            DateTime lastDate = valueList.LatestValue()?.Day ?? DateTime.MinValue;
            Value = valueList.CalculateValue(
                DrawDownCalculators.DefaultCalculator(firstDate, lastDate),
                defaultValue: double.NaN);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = portfolio.TotalDrawdown(total, name);
        }
    }
}
