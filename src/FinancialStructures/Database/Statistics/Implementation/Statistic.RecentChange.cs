using System;

using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.FinanceStructures.Statistics;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticRecentChange : StatisticBase
    {
        internal StatisticRecentChange()
            : base(Statistic.RecentChange)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            if (valueList is IExchangableValueList exchangeableValueList)
            {
                ICurrency currency = portfolio.Currency(exchangeableValueList);
                Value = (double)exchangeableValueList.RecentChange(currency);
            }
            else
            {
                Value = (double)valueList.RecentChange();
            }
            
            fCurrency = portfolio.BaseCurrency;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = (double)portfolio.RecentChange(total, name);
            fCurrency = portfolio.BaseCurrency;
        }
    }
}
