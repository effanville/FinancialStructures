using System;
using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticWeeklyChange : StatisticBase
    {
        internal StatisticWeeklyChange()
            : base(Statistic.WeeklyChange)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            fCurrency = valueList.Names.Currency ?? portfolio.BaseCurrency;
            if (valueList is IExchangeableValueList exchangeableValueList)
            {
                ICurrency currency = portfolio.Currency(exchangeableValueList);
                DailyValuation needed = exchangeableValueList.LatestValue(currency);
                if (needed?.Value > 0)
                {
                    DailyValuation previousValue = exchangeableValueList.Value(date.AddDays(-7), currency);
                    Value = (double)(needed.Value - (previousValue?.Value ?? 0.0m));
                    return;
                }

                return;
            }
            var latestValue = valueList.LatestValue();
            if (latestValue?.Value > 0)
            {
                DailyValuation weekAgoValue = valueList.Value(date.AddDays(-7));
                Value = (double)(latestValue.Value - weekAgoValue?.Value ?? 0.0m);
            }
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            string identifier = total.GetIdentifier(name);
            var latestValue = portfolio.TotalValue(total, identifier);
            var weekAgoValue = portfolio.TotalValue(total, date.AddDays(-1), identifier);
            Value = (double)(latestValue - weekAgoValue);
            fCurrency = portfolio.BaseCurrency;
        }
    }
}
