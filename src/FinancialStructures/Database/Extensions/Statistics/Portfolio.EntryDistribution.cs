using System;
using System.Collections.Generic;
using Effanville.FinancialStructures.FinanceStructures.Extensions;

namespace Effanville.FinancialStructures.Database.Extensions.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// Returns a dictionary with the distribution over time of values for the
        /// account type.
        /// </summary>
        public static Dictionary<DateTime, int> EntryDistribution(this IPortfolio portfolio, Totals totals = Totals.Security, string identifier = null)
        {
            return portfolio.CalculateAggregateValue(
                totals,
                identifier,
                new Dictionary<DateTime, int>(),
                (a, b) => MergeDictionaries(a, b),
                vl => vl.EntryDistribution(),
                new Dictionary<DateTime, int>());
        }

        private static Dictionary<DateTime, int> MergeDictionaries(Dictionary<DateTime, int> first, Dictionary<DateTime, int> second)
        {
            Dictionary<DateTime, int> merged = new ();
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
