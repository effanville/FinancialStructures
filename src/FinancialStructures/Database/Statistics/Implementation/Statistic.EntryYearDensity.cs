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
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            Value = ((valueList.LatestValue()?.Day - valueList.FirstValue()?.Day)?.Days ?? 0) / ((double)365 * valueList.Count());
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = (portfolio.LatestDate(total, name) - portfolio.FirstValueDate(total, name)).Days / ((double)365 * portfolio.EntryDistribution(total, name).Count);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return IsNumeric ? Value.TruncateToString(4) : StringValue;
        }
    }
}
