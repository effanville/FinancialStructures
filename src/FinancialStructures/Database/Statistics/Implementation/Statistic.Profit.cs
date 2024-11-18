using System;

using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Extensions;
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
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            fCurrency = portfolio.BaseCurrency;
            ICurrency currency = portfolio.Currency(valueList);
            Value = (double)valueList.Profit(currency);
        }
        
        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            string identifier = total.GetIdentifier(name);
            fCurrency = portfolio.BaseCurrency;
            Value = (double)portfolio.TotalProfit(total, identifier);
        }
    }
}
