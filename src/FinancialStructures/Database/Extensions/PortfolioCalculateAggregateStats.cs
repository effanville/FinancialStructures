using System;
using System.Collections.Generic;
using System.Linq;

using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions
{
    /// <summary>
    /// Helper methods for calculating 
    /// </summary>
    public static class PortfolioCalculateAggregateStats
    {
        /// <summary>
        /// Calculates an aggregate statistic from a list of accounts.
        /// </summary>
        /// <typeparam name="S">The type of the statistic to return.</typeparam>
        /// <param name="portfolio">The portfolio to calculate statistics for.</param>
        /// <param name="total">The total type to calculate.</param>
        /// <param name="name">The name for the total type.</param>
        /// <param name="initialStatisticValue">The initial (default) value for the statistic.</param>
        /// <param name="valueListStatisticCalculator">The method to calculate a statistic from an individual account.</param>
        /// <param name="statisticAggregator">The aggregation method to determine which statistic is preferable.</param>
        /// <returns>The statistic.</returns>
        public static S CalculateAggregateStatistic<S>(
            this IPortfolio portfolio,
            Totals total,
            TwoName name,
            S initialStatisticValue,
            Func<IValueList, S> valueListStatisticCalculator,
            Func<S, S, S> statisticAggregator)
        {
            var accounts = portfolio.Accounts(total, name);
            return CalculateAggregateStatistic(
                accounts,
                initialStatisticValue,
                valueListStatisticCalculator,
                statisticAggregator);
        }

        /// <summary>
        /// Calculates an aggregate statistic from a list of accounts.
        /// </summary>
        /// <typeparam name="S">The type of the statistic to return.</typeparam>
        /// <param name="portfolio">The portfolio to calculate statistics for.</param>
        /// <param name="total">The total type to calculate.</param>
        /// <param name="name">The name for the total type.</param>
        /// <param name="preCalculationCheck">A check to perform prior to calculating the statistic.</param>
        /// <param name="initialStatisticValue">The initial (default) value for the statistic.</param>
        /// <param name="valueListStatisticCalculator">The method to calculate a statistic from an individual account.</param>
        /// <param name="statisticAggregator">The aggregation method to determine which statistic is preferable.</param>
        /// <returns>The statistic.</returns>
        public static S CalculateAggregateStatistic<S>(
            this IPortfolio portfolio,
            Totals total,
            TwoName name,
            Func<Totals, TwoName, bool> preCalculationCheck,
            S initialStatisticValue,
            Func<IValueList, S> valueListStatisticCalculator,
            Func<S, S, S> statisticAggregator)
        {
            if (!preCalculationCheck(total, name))
            {
                return default(S);
            }

            return CalculateAggregateStatistic(
                portfolio,
                total,
                name,
                initialStatisticValue,
                valueListStatisticCalculator,
                statisticAggregator);
        }

        /// <summary>
        /// Calculates an aggregate statistic from a list of accounts.
        /// </summary>
        /// <typeparam name="T">The type of the accounts to aggregate.</typeparam>
        /// <typeparam name="S">The type of the statistic to return.</typeparam>
        /// <param name="portfolio">The portfolio to calculate statistics for.</param>
        /// <param name="total">The total type to calculate.</param>
        /// <param name="name">The name for the total type.</param>
        /// <param name="preCalculationCheck">A check to perform prior to calculating the statistic.</param>
        /// <param name="initialStatisticValue">The initial (default) value for the statistic.</param>
        /// <param name="valueListStatisticCalculator">The method to calculate a statistic from an individual account.</param>
        /// <param name="statisticAggregator">The aggregation method to determine which statistic is preferable.</param>
        /// <returns>The statistic.</returns>
        public static S CalculateAggregateStatistic<T, S>(
            this IPortfolio portfolio,
            Totals total,
            TwoName name,
            Func<Totals, TwoName, bool> preCalculationCheck,
            S initialStatisticValue,
            Func<T, S> valueListStatisticCalculator,
            Func<S, S, S> statisticAggregator)
            where T : class, IValueList
        {
            if (!preCalculationCheck(total, name))
            {
                return default(S);
            }

            IReadOnlyList<IValueList> accounts = portfolio.Accounts(total, name);
            IReadOnlyList<T> typedAccounts = accounts.Select(acc => acc as T).ToList();
            return CalculateAggregateStatistic(
                typedAccounts,
                initialStatisticValue,
                valueListStatisticCalculator,
                statisticAggregator);
        }

        /// <summary>
        /// Calculates an aggregate statistic from a list of accounts.
        /// </summary>
        /// <typeparam name="S">The type of the statistic to return.</typeparam>
        /// <param name="accounts">The list of accounts to aggregate over.</param>
        /// <param name="initialStatisticValue">The initial (default) value for the statistic.</param>
        /// <param name="valueListStatisticCalculator">The method to calculate a statistic from an individual account.</param>
        /// <param name="statisticAggregator">The aggregation method to determine which statistic is preferable.
        /// This acts on the new statistic and current value to produce a new value.</param>
        /// <returns>The statistic.</returns>
        public static S CalculateAggregateStatistic<S>(
            IReadOnlyList<IValueList> accounts,
            S initialStatisticValue,
            Func<IValueList, S> valueListStatisticCalculator,
            Func<S, S, S> statisticAggregator)
        {
            if (!accounts.Any())
            {
                return initialStatisticValue;
            }

            S finalStatistic = initialStatisticValue;
            foreach (IValueList account in accounts)
            {
                if (account == null)
                {
                    continue;
                }

                if (!account.Any())
                {
                    continue;
                }

                S statistic = valueListStatisticCalculator(account);
                finalStatistic = statisticAggregator(statistic, finalStatistic);
            }

            return finalStatistic;
        }

        /// <summary>
        /// Calculates an aggregate statistic from a list of accounts.
        /// </summary>
        /// <typeparam name="T">The type of the account to calculate statistics for.</typeparam>
        /// <typeparam name="S">The type of the statistic to return.</typeparam>
        /// <param name="accounts">The list of accounts to aggregate over.</param>
        /// <param name="initialStatisticValue">The initial (default) value for the statistic.</param>
        /// <param name="valueListStatisticCalculator">The method to calculate a statistic from an individual account.</param>
        /// <param name="statisticAggregator">The aggregation method to determine which statistic is preferable.</param>
        /// <returns>The statistic.</returns>
        public static S CalculateAggregateStatistic<T, S>(
            IReadOnlyList<T> accounts,
            S initialStatisticValue,
            Func<T, S> valueListStatisticCalculator,
            Func<S, S, S> statisticAggregator)
            where T : IValueList
        {
            if (!accounts.Any())
            {
                return initialStatisticValue;
            }

            S finalStatistic = initialStatisticValue;
            foreach (T account in accounts)
            {
                if (account == null)
                {
                    continue;
                }

                if (!account.Any())
                {
                    continue;
                }

                S statistic = valueListStatisticCalculator(account);
                finalStatistic = statisticAggregator(statistic, finalStatistic);
            }

            return finalStatistic;
        }
    }
}
