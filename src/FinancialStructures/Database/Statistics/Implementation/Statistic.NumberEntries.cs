using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticNumberEntries : StatisticBase
    {
        internal StatisticNumberEntries()
            : base(Statistic.NumberEntries)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            Value = valueList.Count();
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Dictionary<DateTime, int> distribution = portfolio.EntryDistribution(total, name);
            Value = distribution.Count();
        }
    }
}
