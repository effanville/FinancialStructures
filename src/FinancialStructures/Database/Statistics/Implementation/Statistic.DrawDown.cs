using System;
using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Extensions;
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
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            DateTime firstDate = valueList.FirstValue()?.Day ?? DateTime.MaxValue;
            DateTime lastDate = valueList.LatestValue()?.Day ?? DateTime.MinValue;
            Value = valueList.DrawDown(firstDate, lastDate);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            string identifier = total.GetIdentifier(name);
            Value = portfolio.TotalDrawdown(total, identifier);
        }
    }
}
