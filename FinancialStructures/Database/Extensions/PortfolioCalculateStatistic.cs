using System;

using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions
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
           S defaultValue = default(S))
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList valueList))
            {
                return defaultValue;
            }

            return statisticCalculator(valueList);
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
           S defaultValue = default(S))
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList valueList))
            {
                return defaultValue;
            }

            if (valueList is not IExchangableValueList exchangableValueList)
            {
                return valueListCalculator(valueList);
            }

            return exchangableValueListCalculator(exchangableValueList);
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
           S defaultValue = default(S))
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList valueList))
            {
                return defaultValue;
            }

            if (valueList is not IExchangableValueList exchangableValueList)
            {
                return valueListCalculator(valueList);
            }

            if (exchangableValueList is not ISecurity security)
            {
                return exchangableValueListCalculator(exchangableValueList);
            }

            return securityCalculator(security);
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
           S defaultValue = default(S))
        {
            if (!preCalculationCheck(account, name))
            {
                return defaultValue;
            }

            return CalculateStatistic(portfolio, account, name, statisticCalculator);
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
           S defaultValue = default(S))
            where T : IValueList
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList valueList))
            {
                return defaultValue;
            }

            if (valueList is not T specialValueList)
            {
                return defaultValue;
            }

            return statisticCalculator(specialValueList);
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
           S defaultValue = default(S))
            where T : IValueList
        {
            if (!preCalculationCheck(account, name))
            {
                return defaultValue;
            }

            return CalculateStatistic(portfolio, account, name, statisticCalculator);
        }
    }
}
