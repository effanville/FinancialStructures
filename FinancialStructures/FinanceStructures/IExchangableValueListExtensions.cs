using System;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Contains extension methods for <see cref="IExchangableValueList"/>s.
    /// </summary>
    public static class IExchangableValueListExtensions
    {
        /// <summary>
        /// Returns the value from the list, and provides the default value if no value exists.
        /// </summary>
        public static decimal Value(this IExchangableValueList valueList, DateTime time, ICurrency currency, decimal defaultValue)
        {
            return valueList.Value(time, currency)?.Value ?? defaultValue;
        }
    }
}
