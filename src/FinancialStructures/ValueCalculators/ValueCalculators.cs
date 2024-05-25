using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.ValueCalculators
{
    public static class ValueCalculator
    {
        private static Dictionary<(IPortfolio, DateTime), Dictionary<Account, Func<IValueList, decimal>>> state = new();
        
        public static Dictionary<Account, Func<IValueList, decimal>> Calculators(IPortfolio portfolio, DateTime time)
        {
            //if (!state.TryGetValue((portfolio, time), out Dictionary<Account, Func<IValueList, decimal>> calculators))
            //{
                var calculators = new Dictionary<Account, Func<IValueList, decimal>>
                {
                    { Account.BankAccount, s => BankAccCalculate(s, portfolio, time) },
                    { Account.Security, s => ExchangableCalculate(s, portfolio, time) },
                    { Account.Pension, s => ExchangableCalculate(s, portfolio, time) },
                    { Account.Asset, s => ExchangableCalculate(s, portfolio, time) },
                };
                //state[(portfolio, time)] = calculators;
            //}

            return calculators;
        }

        public static Func<IValueList, decimal> DefaultCalculator(DateTime time) 
            => valueList => valueList.Value(time)?.Value ?? 0.0m;

        static decimal BankAccCalculate(IValueList valueList, IPortfolio portfolio, DateTime time)
        {
            if (valueList is not IExchangableValueList eValueList)
            {
                return 0.0m;
            }
            ICurrency currency = portfolio.Currency(eValueList);
            return eValueList.ValueOnOrBefore(time, currency)?.Value ?? 0.0m;
        }
        
        static decimal ExchangableCalculate(IValueList valueList, IPortfolio portfolio, DateTime time)
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