using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.ValueCalculators
{
    public static class InvestmentCalculators
    {
        private static Dictionary<(IPortfolio, DateTime), Dictionary<Account, Func<IValueList, decimal>>> state = new();
        public static Dictionary<Account, Func<IValueList, decimal>> Calculators(IPortfolio portfolio, DateTime date)
        {
            if (!state.TryGetValue((portfolio, date), out Dictionary<Account, Func<IValueList, decimal>> calculators))
            {
                calculators = new Dictionary<Account, Func<IValueList, decimal>>
                {
                    { Account.Security, s => CalculateSecurity(portfolio, s) },
                    { Account.Pension, s => CalculateSecurity(portfolio, s) },
                    { Account.Asset, s => CalculateAsset(portfolio, s, date) }
                };
                state[(portfolio, date)] = calculators;
            }

            return calculators;
        }

        public static Func<IValueList, decimal> DefaultCalculator => DefaultCalculate;
        static decimal DefaultCalculate(IValueList valueList) 
            => 0.0m;

        static decimal CalculateSecurity(IPortfolio portfolio, IValueList valueList)
        {
            if (valueList is not ISecurity security)
            {
                return 0.0m;
            }
            ICurrency currency = portfolio.Currency(security);
            return security.TotalInvestment(currency);
        }
        
        static decimal CalculateAsset(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            if (valueList is not IAmortisableAsset asset)
            {
                return 0.0m;
            }
            ICurrency currency = portfolio.Currency(asset);
            return asset.TotalCost(date, currency);
        }
    }
}