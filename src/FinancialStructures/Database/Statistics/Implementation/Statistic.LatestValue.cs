using System;

using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticLatestValue : StatisticBase
    {
        internal StatisticLatestValue()
            : base(Statistic.LatestValue)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            fCurrency = portfolio.BaseCurrency;
            if (account == Account.Currency || account == Account.Benchmark)
            {
                Value = 1.0d;
                return;
            }
            Value = (double)CalculateValue(portfolio, account, valueList, DateTime.Today);
        }            
        decimal CalculateValue(IPortfolio portfolio, Account account, IValueList valueList, DateTime date)
        {
            if (!valueList.Any())
            {
                return 0;
            }

            if (valueList is not IExchangableValueList eValueList)
            {
                return valueList.Value(date)?.Value ?? 0.0m;
            }

            ICurrency currency = portfolio.Currency(eValueList);

            if (account is Account.BankAccount)
            {
                return eValueList.ValueOnOrBefore(date, currency)?.Value ?? 0.0m;
            }

            return eValueList.Value(date, currency)?.Value ?? 0.0m;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = (double)portfolio.TotalValue(total, DateTime.Today, name);
            fCurrency = portfolio.BaseCurrency;
        }
    }
}
