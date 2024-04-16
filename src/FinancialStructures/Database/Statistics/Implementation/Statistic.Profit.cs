using System;

using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Statistics;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticProfit : StatisticBase
    {
        internal StatisticProfit()
            : base(Statistic.Profit)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            fCurrency = portfolio.BaseCurrency;
            if (valueList is IExchangableValueList exchangeableValueList)
            {
                ICurrency currency = portfolio.Currency(exchangeableValueList);
                Value = (double)exchangeableValueList.Profit(currency);
                return;
            }

            Value = (double)(valueList.Any() ? valueList.Profit() : 0.0m);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = (double)portfolio.TotalProfit(total, name);
            fCurrency = portfolio.BaseCurrency;
        }
    }
}
