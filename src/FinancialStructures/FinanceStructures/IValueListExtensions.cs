using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.Database;

namespace Effanville.FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Contains extension methods for <see cref="IValueList"/>s.
    /// </summary>
    public static class IValueListExtensions
    {
        /// <summary>
        /// Returns the value from the list, and provides the default value if no value exists.
        /// </summary>
        public static decimal Value(this IValueList valueList, DateTime time, decimal defaultValue)
        {
            return valueList.Value(time)?.Value ?? defaultValue;
        }

        public static DateTime LatestDate(this IValueList valueList)
            => valueList.LatestValue()?.Day ?? DateTime.MinValue;

        public static DateTime FirstDate(this IValueList valueList)
            => valueList.FirstValue()?.Day ?? DateTime.MaxValue;
        
        /// <summary>
        /// Calculates a statistic for an account.
        /// </summary>
        /// <param name="valueList">The valueList to find the value for.</param>
        /// <param name="calculatorMapping">The mapping of account type to the calculator to use.</param>
        /// <param name="defaultCalculator">The optional default calculator to use.</param>
        /// <param name="defaultValue">The optional default value to use.</param>
        /// <typeparam name="TValue">The type of the statistic to return.</typeparam>
        /// <returns>The statistic value</returns>
        public static TValue CalculateValue<TValue>(
            this IValueList valueList,            
            Func<IValueList, TValue> defaultCalculator,
            IDictionary<Account, Func<IValueList, TValue>> calculatorMapping= null,
            TValue defaultValue = default)
        {
            if (!valueList.Any())
            {
                return defaultValue;
            }

            if(calculatorMapping == null 
               || !calculatorMapping.TryGetValue(valueList.AccountType, out Func<IValueList, TValue> calculator))
            {
                calculator = defaultCalculator;
            }

            return calculator != null ? calculator(valueList) : defaultValue;
        }
    }
}
