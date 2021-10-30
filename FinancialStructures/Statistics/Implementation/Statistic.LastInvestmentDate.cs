using Common.Structure.Extensions;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticLastInvestmentDate : IStatistic
    {
        internal StatisticLastInvestmentDate()
        {
            StatType = Statistic.LastInvestmentDate;
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
            StringValue = portfolio.LastInvestmentDate(account, name).ToUkDateString();
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            StringValue = portfolio.LastInvestmentDate(total, name).ToUkDateString();
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
