using System;

using Effanville.Common.Structure.Extensions;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticLatestDate : IStatistic
    {
        /// <inheritdoc/>
        public Statistic StatType => Statistic.LatestDate;

        /// <inheritdoc/>
        public double Value
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public string StringValue
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public bool IsNumeric => false;

        /// <inheritdoc/>
        public object ValueAsObject => StringValue;

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            StringValue = valueList.CalculateValue(
                vl => vl.LatestDate())
                .ToIsoDateString();
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            string identifier = total.GetIdentifier(name);
            StringValue = portfolio.LatestDate(total, identifier).ToIsoDateString();
        }

        /// <inheritdoc/>
        public int CompareTo(IStatistic other) => Value.CompareTo(other.Value);

        /// <inheritdoc/>
        public override string ToString() => StringValue;
    }
}
