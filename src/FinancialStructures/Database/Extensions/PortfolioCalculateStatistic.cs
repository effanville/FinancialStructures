using System;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions
{
    /// <summary>
    /// Helper methods for calculating statistics for a specific account.
    /// </summary>
    public static class PortfolioCalculateStatistic
    {
        /// <summary>
        /// Calculates a statistic for an account that satisfies certain conditions.
        /// </summary>
        /// <param name="portfolio">The portfolio to find the account.</param>
        /// <param name="account">The account type.</param>
        /// <param name="name">The name of the account.</param>
        /// <param name="preCalculationCheck">A check to perform before calculating the statistic. True if the statistic
        /// should be calculated.</param>
        /// <param name="calculator">The optional default calculator to use.</param>
        /// <param name="defaultValue">The optional default value to use.</param>
        /// <typeparam name="TValue">The type of the statistic to return.</typeparam>
        /// <returns>The value of the desired statistic.</returns>
        public static TValue CalculateStatistic<TValue>(
            this IPortfolio portfolio,
            Account account,
            TwoName name,
            Func<Account, TwoName, bool> preCalculationCheck,
            Func<IValueList, TValue> calculator,
            TValue defaultValue = default)
        {
            if (!preCalculationCheck(account, name))
            {
                return defaultValue;
            }

            return portfolio.CalculateValue(
                account, 
                name, 
                calculator,
                defaultValue);
        }
        
        /// <summary>
        /// Calculates a statistic for an account.
        /// </summary>
        /// <param name="portfolio">The portfolio to find the account.</param>
        /// <param name="account">The account type.</param>
        /// <param name="name">The name of the account.</param>
        /// <param name="calculator">The optional default calculator to use.</param>
        /// <param name="defaultValue">The optional default value to use.</param>
        /// <typeparam name="TValue">The type of the statistic to return.</typeparam>
        /// <returns>The statistic value</returns>
        public static TValue CalculateValue<TValue>(
            this IPortfolio portfolio,
            Account account,
            TwoName name,
            Func<IValueList, TValue> calculator,
            TValue defaultValue = default) 
            => portfolio.TryGetAccount(account, name, out IValueList valueList) 
                ? valueList.CalculateValue(calculator, defaultValue)
                : defaultValue;
    }
}
