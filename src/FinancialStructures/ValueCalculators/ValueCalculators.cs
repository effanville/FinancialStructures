using System;
using System.Collections.Generic;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.ValueCalculators
{
    public static class ValueCalculator
    {
        public static decimal DefaultValue(Account accountType)
        {
            if (accountType == Account.Currency || accountType == Account.Benchmark)
            {
                return 1.0m;
            }

            return 0.0m;
        }

        public static Dictionary<Account, Func<IValueList, decimal>> Calculators(IPortfolio portfolio, DateTime time)
        {
            Dictionary<Account, Func<IValueList, decimal>> calculators = new Dictionary<Account, Func<IValueList, decimal>>
                {
                    { Account.BankAccount, s => BankAccountCalculate(s, portfolio, time) },
                    { Account.Security, s => ExchangeableCalculate(s, portfolio, time) },
                    { Account.Pension, s => ExchangeableCalculate(s, portfolio, time) },
                    { Account.Asset, s => ExchangeableCalculate(s, portfolio, time) },
                };

            return calculators;
        }

        public static Func<IValueList, decimal> DefaultCalculator(DateTime time)
            => valueList => valueList.Value(time)?.Value ?? 0.0m;

        private static decimal BankAccountCalculate(IValueList valueList, IPortfolio portfolio, DateTime time)
        {
            if (valueList is not IExchangableValueList eValueList)
            {
                return 0.0m;
            }

            ICurrency currency = portfolio.Currency(eValueList);
            return eValueList.ValueOnOrBefore(time, currency)?.Value ?? 0.0m;
        }

        private static decimal ExchangeableCalculate(IValueList valueList, IPortfolio portfolio, DateTime time)
        {
            if (valueList is not IExchangableValueList eValueList)
            {
                return 0.0m;
            }

            ICurrency currency = portfolio.Currency(eValueList);
            return eValueList.Value(time, currency)?.Value ?? 0.0m;
        }
    }
}