using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticCompany : IStatistic
    {
        internal StatisticCompany()
        {
            StatType = Statistic.Company;
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
            StringValue = name.Company;
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            StringValue = name.Company;
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
