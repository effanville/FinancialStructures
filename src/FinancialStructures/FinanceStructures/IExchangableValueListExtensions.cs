using System;

namespace Effanville.FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Contains extension methods for <see cref="IExchangeableValueList"/>s.
    /// </summary>
    public static class IExchangableValueListExtensions
    {
        /// <summary>
        /// Returns the value from the list, and provides the default value if no value exists.
        /// </summary>
        public static decimal Value(this IExchangeableValueList valueList, DateTime time, ICurrency currency, decimal defaultValue)
        {
            return valueList.Value(time, currency)?.Value ?? defaultValue;
        }
    }
}
