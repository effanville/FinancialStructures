using System;

using Effanville.Common.Structure.Extensions;
using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticEntryYearDensity : StatisticBase
    {
        internal StatisticEntryYearDensity()
            : base(Statistic.EntryYearDensity)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            Value = ((double)365 * valueList.Count()) / ((valueList.LatestValue()?.Day - valueList.FirstValue()?.Day)?.Days ?? 0);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = ((double)365 * portfolio.EntryDistribution(total, name).Count) / (portfolio.LatestDate(total, name) - portfolio.FirstValueDate(total, name)).Days;
        }

        /// <inheritdoc/>
        public override string ToString() => Value.TruncateToString();
    }
}
