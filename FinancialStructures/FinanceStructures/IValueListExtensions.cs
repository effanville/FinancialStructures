using System;

namespace FinancialStructures.FinanceStructures
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
    }
}
