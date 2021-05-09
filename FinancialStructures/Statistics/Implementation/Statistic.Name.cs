using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticName : IStatistic
    {
        internal StatisticName()
        {
            StatType = Statistic.Name;
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
        public bool IsNumeric
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public object ValueAsObject
        {
            get
            {
                return IsNumeric ? (object)Value : (object)StringValue;
            }
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            StringValue = name.Name;
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            StringValue = name.Name;
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
