using System;

using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticAccountType : IStatistic
    {
        /// <inheritdoc/>
        public Statistic StatType => Statistic.AccountType;

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
            StringValue = valueList.AccountType.ToString();
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            StringValue = total.ToAccount().ToString();
        }

        /// <inheritdoc/>
        public int CompareTo(IStatistic other) => Value.CompareTo(other.Value);

        public override string ToString() => StringValue;
    }
}
