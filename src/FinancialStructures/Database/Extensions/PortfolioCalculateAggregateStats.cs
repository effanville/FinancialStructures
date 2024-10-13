using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions
{
    /// <summary>
    /// Helper methods for calculating 
    /// </summary>
    public static class PortfolioCalculateAggregateStats
    {
        public static TValue CalculateAggregateValue<TValue>(
            this IPortfolio portfolio,
            Totals total,
            TwoName name,
            Func<Totals, TwoName, bool> preCalculationCheck,
            TValue initialStatisticValue,
            Func<TValue, TValue, TValue> statisticAggregator,
            Func<IValueList, TValue> calculator,
            TValue defaultValue = default)
        {
            if (!preCalculationCheck(total, name))
            {
                return defaultValue;
            }

            return portfolio.CalculateAggregateValue(
                total, 
                name, 
                initialStatisticValue,
                statisticAggregator,
                calculator,
                defaultValue);
        }

        /// <summary>
        /// Calculates an aggregate statistic from a list of accounts.
        /// </summary>
        /// <typeparam name="TValue">The type of the statistic to return.</typeparam>
        /// <param name="portfolio">The portfolio to calculate statistics for.</param>
        /// <param name="total">The total type to calculate.</param>
        /// <param name="name">The name for the total type.</param>
        /// <param name="initialStatisticValue">The initial (default) value for the statistic.</param>
        /// <param name="statisticAggregator">The aggregation method to determine which statistic is preferable.</param>
        /// <param name="calculator">The optional default calculator to use.</param>
        /// <param name="defaultValue">The optional default value to use.</param>
         /// <returns>The statistic.</returns>
        public static TValue CalculateAggregateValue<TValue>(
            this IPortfolio portfolio,
            Totals total,
            TwoName name,
            TValue initialStatisticValue,
            Func<TValue, TValue, TValue> statisticAggregator,
            Func<IValueList, TValue> calculator,
            TValue defaultValue = default)
        {
            IReadOnlyList<IValueList> valueLists = portfolio.Accounts(total, name);

            if (!valueLists.Any())
            {
                return initialStatisticValue;
            }

            TValue finalStatistic = initialStatisticValue;
            foreach (IValueList valueList in valueLists)
            {
                TValue statistic = valueList.CalculateValue(calculator, defaultValue);
                finalStatistic = statisticAggregator(statistic, finalStatistic);
            }

            return finalStatistic;
        }
    }
}
