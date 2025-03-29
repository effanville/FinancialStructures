using System;
using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticWeeklyChange : StatisticBase
    {
        private int _changeWindowDays;
        internal StatisticWeeklyChange(int changeWindowDays = -7)
            : base(Statistic.WeeklyChange)
            => _changeWindowDays = changeWindowDays;

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            fCurrency = valueList.Names.Currency ?? portfolio.BaseCurrency;
            if (valueList is IExchangeableValueList exchangeableValueList)
            {
                ICurrency currency = portfolio.Currency(exchangeableValueList);
                DailyValuation needed = exchangeableValueList.Value(date, currency);
                if (needed?.Value > 0)
                {
                    DailyValuation previousValue = exchangeableValueList.Value(date.AddDays(_changeWindowDays), currency);
                    Value = (double)(needed.Value - (previousValue?.Value ?? 0.0m));
                    return;
                }

                return;
            }

            DailyValuation latestValue = valueList.Value(date);
            if (latestValue?.Value > 0)
            {
                DailyValuation weekAgoValue = valueList.Value(date.AddDays(_changeWindowDays));
                Value = (double)(latestValue.Value - weekAgoValue?.Value ?? 0.0m);
            }
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            string identifier = total.GetIdentifier(name);
            decimal latestValue = portfolio.TotalValue(total, date, identifier);
            decimal weekAgoValue = portfolio.TotalValue(total, date.AddDays(_changeWindowDays), identifier);
            Value = (double)(latestValue - weekAgoValue);
            fCurrency = portfolio.BaseCurrency;
        }
    }
}
