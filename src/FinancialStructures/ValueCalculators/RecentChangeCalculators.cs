using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Statistics;

namespace Effanville.FinancialStructures.ValueCalculators
{
    public static class RecentChangeCalculators
    {
        private static Dictionary<IPortfolio, Dictionary<Account, Func<IValueList, decimal>>> state = new();
        public static Dictionary<Account, Func<IValueList, decimal>> Calculators(IPortfolio portfolio)
        {
            if (!state.TryGetValue(portfolio, out Dictionary<Account, Func<IValueList, decimal>> calculators))
            {
                calculators = new Dictionary<Account, Func<IValueList, decimal>>();
                calculators.Add(Account.Security, s => Calculate(portfolio, s));
                calculators.Add(Account.Pension, s => Calculate(portfolio, s));
                calculators.Add(Account.BankAccount, s => Calculate(portfolio, s));
                calculators.Add(Account.Asset, s => Calculate(portfolio, s));
                state[portfolio] = calculators;
            }

            return calculators;
        }

        public static Func<IValueList, decimal> DefaultCalculator => DefaultCalculate;
        static decimal DefaultCalculate(IValueList valueList) 
            => valueList.Any() ? valueList.RecentChange() : 0.0m;

        static decimal Calculate(IPortfolio portfolio, IValueList valueList)
        {
            if (valueList is not IExchangeableValueList exchangableValueList)
            {
                return 0.0m;
            }
            ICurrency currency = portfolio.Currency(exchangableValueList);
            return exchangableValueList.RecentChange(currency);
        }
    }
}