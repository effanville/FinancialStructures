using System;

using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticCurrency : IStatistic
    {
        /// <inheritdoc/>
        public Statistic StatType => Statistic.Currency;

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
            StringValue = valueList.Names?.Currency;
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            if (total == Totals.Currency
                || total == Totals.AssetCurrency
                || total == Totals.BankAccountCurrency
                || total == Totals.SecurityCurrency
                || total == Totals.PensionCurrency
                || total == Totals.CurrencySector)
            {
                StringValue = name.Name;
            }
            else
            {
                StringValue = string.Empty;
            }
        }

        /// <inheritdoc/>
        public int CompareTo(IStatistic other) => Value.CompareTo(other.Value);

        public override string ToString() => StringValue;
    }
}
