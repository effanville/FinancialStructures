using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;
using Common.Structure.Extensions;

namespace FinancialStructures.Statistics
{
    internal class StatisticLatestDate : IStatistic
    {
        internal StatisticLatestDate()
        {
            StatType = Statistic.LatestDate;
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
            if (!portfolio.TryGetAccount(account, name, out var desired))
            {
                return;
            }

            StringValue = desired.LatestValue().Day.ToUkDateString();
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            StringValue = portfolio.LatestDate(total, name).ToUkDateString();
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
