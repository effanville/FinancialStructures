using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticCurrency : IStatistic
    {
        internal StatisticCurrency()
        {
            StatType = Statistic.Currency;
        }

        /// <inheritdoc/>
        public Statistic StatType
        {
            get;
        }

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
        public object ValueAsObject => IsNumeric ? Value : StringValue;

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList desired))
            {
                StringValue = null;
                return;
            }
            StringValue = desired.Names?.Currency;
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            StringValue = string.Empty;
        }

        /// <inheritdoc/>
        public int CompareTo(IStatistic other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return StringValue;
        }
    }
}
