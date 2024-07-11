using System;
using System.Collections.Generic;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.ValueCalculators
{
    public static class EntryDistributionCalculator
    {
        private static Dictionary<(IPortfolio, DateTime), Dictionary<Account, Func<IValueList, Dictionary<DateTime, int>>>> state = new();
        
        public static Dictionary<Account, Func<IValueList, Dictionary<DateTime, int>>> Calculators(IPortfolio portfolio, DateTime time)
        {
            if (!state.TryGetValue((portfolio, time), out Dictionary<Account, Func<IValueList, Dictionary<DateTime, int>>> calculators))
            {
                calculators = new Dictionary<Account, Func<IValueList, Dictionary<DateTime, int>>>
                {
                    { Account.Security, CalculateValuesForSecurity },
                    { Account.Pension, CalculateValuesForSecurity },
                };
                state[(portfolio, time)] = calculators;
            }

            return calculators;
        }
        
        public static Func<IValueList, Dictionary<DateTime, int>> DefaultCalculator()
            => CalculateValues;
        static Dictionary<DateTime, int> CalculateValues(IValueList valueList)
        {
            Dictionary<DateTime, int> totals = new ();
            foreach (DailyValuation value in valueList.ListOfValues())
            {
                totals.Add(value.Day, 1);
            }

            return totals;
        }
        
        static Dictionary<DateTime, int> CalculateValuesForSecurity(IValueList valueList)
        {              
            if (valueList is not ISecurity security)
            {
                return null;
            }
            
            Dictionary<DateTime, int> totals = new ();
            if (security.Any())
            {
                foreach (DailyValuation value in security.Shares.Values())
                {
                    if (totals.TryGetValue(value.Day, out _))
                    {
                        totals[value.Day] += 1;
                    }
                    else
                    {
                        totals.Add(value.Day, 1);
                    }
                }

                foreach (DailyValuation priceValue in security.UnitPrice.Values())
                {
                    if (totals.TryGetValue(priceValue.Day, out _))
                    {
                        totals[priceValue.Day] += 1;
                    }
                    else
                    {
                        totals.Add(priceValue.Day, 1);
                    }
                }

                foreach (DailyValuation investmentValue in security.Investments.Values())
                {
                    if (totals.TryGetValue(investmentValue.Day, out _))
                    {
                        totals[investmentValue.Day] += 1;
                    }
                    else
                    {
                        totals.Add(investmentValue.Day, 1);
                    }
                }
            }

            return totals;
        }
    }
}