using System;
using System.Collections.Generic;
using Common.Structure.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// Returns a dictionary with the distribution over time of values for the
        /// account type.
        /// </summary>
        public static Dictionary<DateTime, int> EntryDistribution(this IPortfolio portfolio, Totals elementType = Totals.Security, TwoName company = null)
        {
            return portfolio.CalculateAggregateStatistic(
                totals,
                company,
                new Dictionary<DateTime, int>(),
                valueList => CalculateValues(valueList),
                (a, b) => MergeDictionaries(a, b));

            Dictionary<DateTime, int> CalculateValues(IValueList valueList)
            {
                if (valueList is ISecurity security)
                {
                    return CalculateValuesForSecurity(security);
                }

                Dictionary<DateTime, int> totals = new Dictionary<DateTime, int>();
                foreach (DailyValuation value in valueList.ListOfValues())
                {
                    totals.Add(value.Day, 1);
                }

                return totals;
            }

            Dictionary<DateTime, int> CalculateValuesForSecurity(ISecurity security)
            {
                Dictionary<DateTime, int> totals = new Dictionary<DateTime, int>();
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

        private static Dictionary<DateTime, int> MergeDictionaries(Dictionary<DateTime, int> first, Dictionary<DateTime, int> second)
        {
            Dictionary<DateTime, int> merged = new Dictionary<DateTime, int>();
            foreach (KeyValuePair<DateTime, int> pair in first)
            {
                if (second.TryGetValue(pair.Key, out int value))
                {
                    merged[pair.Key] = value + pair.Value;
                    _ = second.Remove(pair.Key);
                }

                merged[pair.Key] = pair.Value;
            }

            foreach (KeyValuePair<DateTime, int> pair in second)
            {
                merged[pair.Key] = pair.Value;
            }

            return merged;
        }
    }
}
