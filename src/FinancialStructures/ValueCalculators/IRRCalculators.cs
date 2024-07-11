using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.ValueCalculators
{
    public static class IRRCalculators
    {
        private static Dictionary<IPortfolio, Dictionary<Account, Func<IValueList, double>>> state = new();
        private static Dictionary<(IPortfolio, DateTime, DateTime), Dictionary<Account, Func<IValueList, double>>> dateState = new();
        public static Dictionary<Account, Func<IValueList, double>> Calculators(IPortfolio portfolio)
        {
            if (!state.TryGetValue(portfolio, out Dictionary<Account, Func<IValueList, double>> calculators))
            {
                calculators = new Dictionary<Account, Func<IValueList, double>>
                {
                    { Account.Security, s => CalculateSecurity(portfolio, s) },
                    { Account.Pension, s => CalculateSecurity(portfolio, s) }
                };
                state[portfolio] = calculators;
            }

            return calculators;
        }
        
        public static Dictionary<Account, Func<IValueList, double>> Calculators(IPortfolio portfolio, DateTime earlierTime, DateTime laterTime)
        {
            if (!dateState.TryGetValue((portfolio, earlierTime, laterTime), out Dictionary<Account, Func<IValueList, double>> calculators))
            {
                calculators = new Dictionary<Account, Func<IValueList, double>>
                {
                    { Account.Security, s => CalculateSecurity(portfolio, s, earlierTime, laterTime) },
                    { Account.Pension, s => CalculateSecurity(portfolio, s, earlierTime, laterTime) }
                };
                dateState[(portfolio, earlierTime, laterTime)] = calculators;
            }

            return calculators;
        }

        public static Func<IValueList, double> DefaultCalculator() 
            => vL => DefaultCalculate(
                vL, 
                vL.FirstDate(), 
                vL.LatestDate());
        
        public static Func<IValueList, double> DefaultCalculator(DateTime earlierTime, DateTime laterTime) 
            => vL => DefaultCalculate(vL, earlierTime, laterTime);

        static double DefaultCalculate(IValueList valueList, DateTime earlierTime, DateTime laterTime)
        {
            DateTime earliestTime = valueList.FirstDate();
            if (earlierTime < earliestTime)
            {
                earlierTime = earliestTime;
            }

            DateTime latestTime = valueList.LatestDate();
            if (laterTime > latestTime)
            {
                laterTime = latestTime;
            }
            
            return valueList.Any()
                ? valueList.CAR(earlierTime, laterTime)
                : double.NaN;
        }

        static double CalculateSecurity(IPortfolio portfolio, IValueList valueList)
        {
            if (valueList is not ISecurity security)
            {
                return double.NaN;
            }

            ICurrency currency = portfolio.Currency(security);
            return security.IRR(currency);
        }
        
        static double CalculateSecurity(IPortfolio portfolio, IValueList valueList, DateTime earlierTime, DateTime laterTime)
        {
            if (valueList is not ISecurity security)
            {
                return double.NaN;
            }
            
            DateTime earliestTime = valueList.FirstValue()?.Day ?? DateTime.MaxValue;
            if (earlierTime < earliestTime)
            {
                earlierTime = earliestTime;
            }

            DateTime latestTime = valueList.LatestValue()?.Day ?? DateTime.MinValue;
            if (laterTime > latestTime)
            {
                laterTime = latestTime;
            }

            ICurrency currency = portfolio.Currency(security);
            return security.IRR(earlierTime, laterTime, currency);
        }
    }
}