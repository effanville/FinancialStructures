using System;

using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticNotes : IStatistic
    {
        /// <inheritdoc/>
        public Statistic StatType => Statistic.Notes;

        /// <inheritdoc/>
        public double Value => double.NaN;

        /// <inheritdoc/>
        public string StringValue { get; private set; }

        /// <inheritdoc/>
        public bool IsNumeric => false;

        /// <inheritdoc/>
        public object ValueAsObject => IsNumeric ? Value : StringValue;

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            StringValue = valueList.Names.Notes;
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            StringValue = string.Empty;
        }

        /// <inheritdoc/>
        public int CompareTo(IStatistic other) => Value.CompareTo(other.Value);

        public override string ToString() => StringValue;
    }
}
