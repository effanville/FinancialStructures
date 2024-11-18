using System;
using Effanville.Common.Structure.Extensions;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticLastInvestmentDate : IStatistic
    {
        /// <inheritdoc/>
        public Statistic StatType => Statistic.LastInvestmentDate;

        /// <inheritdoc/>
        public double Value => double.NaN;

        /// <inheritdoc/>
        public string StringValue { get; private set; }

        /// <inheritdoc/>
        public bool IsNumeric => false;

        /// <inheritdoc/>
        public object ValueAsObject => StringValue;

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            StringValue = valueList.LastInvestmentDate().ToIsoDateString();
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            string identifier = total.GetIdentifier(name);
            StringValue = portfolio.LastInvestmentDate(total, identifier).ToIsoDateString();
        }

        /// <inheritdoc/>
        public int CompareTo(IStatistic other) => Value.CompareTo(other.Value);

        public override string ToString() => StringValue;
    }
}
