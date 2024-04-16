using System;
using System.Linq;

using Effanville.Common.Structure.Extensions;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
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
        public void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            if (valueList is not ISecurity sec)
            {
                StringValue = default(DateTime).ToUkDateString();
                return;
            }

            DateTime latest = sec.Trades.LastOrDefault(trade => trade.TradeType.Equals(TradeType.Buy))?.Day ?? default(DateTime);
            StringValue = latest.ToUkDateString();
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
