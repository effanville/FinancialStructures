using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.ValueCalculators
{
    public static class DebtCalculators
    {
        private static Dictionary<(IPortfolio, DateTime), Dictionary<Account, Func<IValueList, decimal>>> state = new();
        
        public static Dictionary<Account, Func<IValueList, decimal>> Calculators(IPortfolio portfolio, DateTime time)
        {
            if (!state.TryGetValue((portfolio, time), out Dictionary<Account, Func<IValueList, decimal>> calculators))
            {
                calculators = new Dictionary<Account, Func<IValueList, decimal>>
                {
                    { Account.Asset, s => AssetCalculate(s, portfolio, time) },
                };
                state[(portfolio, time)] = calculators;
            }

            return calculators;
        }

        public static Func<IValueList, decimal> DefaultCalculator(DateTime time)
            => _ => 0.0m;
        
        static decimal AssetCalculate(IValueList valueList, IPortfolio portfolio, DateTime time)
        {
            if (valueList is not IAmortisableAsset asset)
            {
                return 0.0m;
            }
            ICurrency currency = portfolio.Currency(asset);
            var latestValue = asset.Debt.LatestValuation();
            decimal currencyValue = currency == null ? 1.0m : currency.Value(time)?.Value ?? 1.0m;
            return latestValue?.Value * currencyValue ?? 0.0m;
        }
    }
}