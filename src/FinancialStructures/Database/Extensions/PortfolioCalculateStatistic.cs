using System;

using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions
{
    /// <summary>
    /// Helper methods for calculating statistics for a specific account.
    /// </summary>
    public static class PortfolioCalculateStatistic
    {
        /// <summary>
        /// Calcuates a statistic for an account.
        /// </summary>
        /// <typeparam name="S">The type of the statistic to return.</typeparam>
        /// <param name="portfolio">The portfolio to find the account.</param>
        /// <param name="account">The account type.</param>
        /// <param name="name">The name of the account.</param>
        /// <param name="statisticCalculator">The method to calculate the statistic.</param>
        /// <param name="defaultValue">The optional default value to use.</param>
        /// <returns>The value of the desired statistic.</returns>
        public static S CalculateStatistic<S>(
           this IPortfolio portfolio,
           Account account,
           TwoName name,
           Func<IValueList, S> statisticCalculator,
           DateTime date,
           string statName,
           IPortfolioStatisticsCache cache,
           S defaultValue = default(S))
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList valueList))
            {
                return defaultValue;
            }

            if (cache?.TryGetValue(valueList.AccountType, valueList.Names.ToTwoName(), date, statName,
                    out object result) ?? false)
            {
                return (S)result;
            }
            
            var value = statisticCalculator(valueList);
            cache?.AddValue(valueList.AccountType, valueList.Names.ToTwoName(), date, statName, value);
            return value;
        }

        /// <summary>
        /// Calcuates a statistic for an account.
        /// </summary>
        /// <typeparam name="S">The type of the statistic to return.</typeparam>
        /// <param name="portfolio">The portfolio to find the account.</param>
        /// <param name="account">The account type.</param>
        /// <param name="name">The name of the account.</param>
        /// <param name="valueListCalculator">The method to calculate the statistic for an <see cref="IValueList"/>.</param>
        /// <param name="exchangableValueListCalculator">The method to calculate the statistic for an <see cref="IExchangableValueList"/>.</param>
        /// <param name="defaultValue">The optional default value to use.</param>
        /// <returns>The value of the desired statistic.</returns>
        public static S CalculateStatistic<S>(
           this IPortfolio portfolio,
           Account account,
           TwoName name,
           Func<IValueList, S> valueListCalculator,
           Func<IExchangableValueList, S> exchangableValueListCalculator,
           DateTime date,
           string statName,
           IPortfolioStatisticsCache cache,
           S defaultValue = default(S))
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList valueList))
            {
                return defaultValue;
            }

            if (cache?.TryGetValue(valueList.AccountType, valueList.Names.ToTwoName(), date, statName,
                    out object result) ?? false)
            {
                return (S)result;
            }

            var value = valueList is not IExchangableValueList exchangableValueList
                ? valueListCalculator(valueList)
                : exchangableValueListCalculator(exchangableValueList);
            cache?.AddValue(valueList.AccountType, valueList.Names.ToTwoName(), date, statName, value);
            return value;
        }
        /// <summary>
        /// Calcuates a statistic for an account.
        /// </summary>
        /// <typeparam name="S">The type of the statistic to return.</typeparam>
        /// <param name="portfolio">The portfolio to find the account.</param>
        /// <param name="account">The account type.</param>
        /// <param name="name">The name of the account.</param>
        /// <param name="valueListCalculator">The method to calculate the statistic for an <see cref="IValueList"/>.</param>
        /// <param name="exchangableValueListCalculator">The method to calculate the statistic for an <see cref="IExchangableValueList"/>.</param>
        /// <param name="securityCalculator">The method to calculate the statistic for an <see cref="ISecurity"/>.</param>
        /// <param name="defaultValue">The optional default value to use.</param>
        /// <returns>The value of the desired statistic.</returns>
        public static S CalculateStatistic<S>(
           this IPortfolio portfolio,
           Account account,
           TwoName name,
           Func<IValueList, S> valueListCalculator,
           Func<IExchangableValueList, S> exchangableValueListCalculator,
           Func<ISecurity, S> securityCalculator,          
           DateTime date,
           string statName,
           IPortfolioStatisticsCache cache,
           S defaultValue = default(S))
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList valueList))
            {
                return defaultValue;
            }

            if (cache?.TryGetValue(valueList.AccountType, valueList.Names.ToTwoName(), date, statName,
                    out object result) ?? false)
            {
                return (S)result;
            }

            var value = valueList is not IExchangableValueList exchangableValueList
                ? valueListCalculator(valueList)
                : exchangableValueList is not ISecurity security
                    ? exchangableValueListCalculator(exchangableValueList)
                    : securityCalculator(security);
            cache?.AddValue(valueList.AccountType, valueList.Names.ToTwoName(), date, statName, value);
            return value;
        }

        /// <summary>
        /// Calcuates a statistic for an account that satisfies certain conditions.
        /// </summary>
        /// <typeparam name="S">The type of the statistic to return.</typeparam>
        /// <param name="portfolio">The portfolio to find the account.</param>
        /// <param name="account">The account type.</param>
        /// <param name="name">The name of the account.</param>
        /// <param name="preCalculationCheck">A check to perform before calculating the statistic. True if the statistic
        /// should be calculated.</param>
        /// <param name="statisticCalculator">The method to calculate the statistic.</param>
        /// <param name="defaultValue">The optional default value to use.</param>
        /// <returns>The value of the desired statistic.</returns>
        public static S CalculateStatistic<S>(
           this IPortfolio portfolio,
           Account account,
           TwoName name,
           Func<Account, TwoName, bool> preCalculationCheck,
           Func<IValueList, S> statisticCalculator,
           DateTime date,
           string statName,
           IPortfolioStatisticsCache cache,
           S defaultValue = default(S))
        {
            if (!preCalculationCheck(account, name))
            {
                return defaultValue;
            }

            return CalculateStatistic(portfolio,
                account, 
                name,
                statisticCalculator, 
                date, 
                statName,
                cache);
        }

        /// <summary>
        /// Calcuates a statistic for an account.
        /// </summary>
        /// <typeparam name="T">The type of the account to use.</typeparam>
        /// <typeparam name="S">The type of the statistic to return.</typeparam>
        /// <param name="portfolio">The portfolio to find the account.</param>
        /// <param name="account">The account type.</param>
        /// <param name="name">The name of the account.</param>
        /// <param name="statisticCalculator">The method to calculate the statistic.</param>
        /// <param name="defaultValue">The optional default value to use.</param>
        /// <returns>The value of the desired statistic.</returns>
        public static S CalculateStatistic<T, S>(
            this IPortfolio portfolio,
            Account account,
            TwoName name,
            Func<T, S> statisticCalculator,
            DateTime date,
            string statName,
            IPortfolioStatisticsCache cache,
            S defaultValue = default(S))
            where T : IValueList
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList valueList))
            {
                return defaultValue;
            }

            if (cache?.TryGetValue(valueList.AccountType, valueList.Names.ToTwoName(), date, statName,
                    out object result) ?? false)
            {
                return (S)result;
            }
            
            var value = valueList is not T specialValueList
                ? defaultValue
                :  statisticCalculator(specialValueList);
            cache?.AddValue(valueList.AccountType, valueList.Names.ToTwoName(), date, statName, value);
            return value;
        }

        /// <summary>
        /// Calcuates a statistic for an account that satisfies certain conditions.
        /// </summary>
        /// <typeparam name="T">The type of the account to use.</typeparam>
        /// <typeparam name="S">The type of the statistic to return.</typeparam>
        /// <param name="portfolio">The portfolio to find the account.</param>
        /// <param name="account">The account type.</param>
        /// <param name="name">The name of the account.</param>
        /// <param name="preCalculationCheck">A check to perform before calculating the statistic. True if the statistic
        /// should be calculated.</param>
        /// <param name="statisticCalculator">The method to calculate the statistic.</param>
        /// <param name="defaultValue">The optional default value to use.</param>
        /// <returns>The value of the desired statistic.</returns>
        public static S CalculateStatistic<T, S>(
            this IPortfolio portfolio,
            Account account,
            TwoName name,
            Func<Account, TwoName, bool> preCalculationCheck,
            Func<T, S> statisticCalculator,
            DateTime date,
            string statName,
            IPortfolioStatisticsCache cache,
           S defaultValue = default(S))
            where T : IValueList
        {
            if (!preCalculationCheck(account, name))
            {
                return defaultValue;
            }

            return CalculateStatistic(
                portfolio, 
                account,
                name, 
                statisticCalculator, 
                date, 
                statName,
                cache);
        }
    }
}
