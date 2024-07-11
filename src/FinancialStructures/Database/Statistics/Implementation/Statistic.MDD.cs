using System;
using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.ValueCalculators;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticMDD : StatisticBase
    {
        internal StatisticMDD()
            : base(Statistic.MDD)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            DateTime firstDate = valueList.FirstDate();
            DateTime lastDate = valueList.LatestDate();
            Value = valueList.CalculateValue(
                MDDCalculators.DefaultCalculator(firstDate, lastDate));
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = portfolio.TotalMDD(total, name);
        }
    }
}
