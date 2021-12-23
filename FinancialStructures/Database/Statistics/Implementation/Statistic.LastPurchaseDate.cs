using System;
using Common.Structure.Extensions;
using FinancialStructures.Database.Extensions.Values;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticLastPurchaseDate : IStatistic
    {
        internal StatisticLastPurchaseDate()
        {
            StatType = Statistic.LastPurchaseDate;
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
        public void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            StringValue = portfolio.LastPurchaseDate(account, name).ToUkDateString();
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            StringValue = portfolio.LastPurchaseDate(total, name).ToUkDateString();
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
