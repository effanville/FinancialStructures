using Effanville.Common.Structure.DataStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Extensions
{
    /// <summary>
    /// Contains extension methods for statistics of <see cref="IValueList"/>s.
    /// </summary>
    public static partial class ValueListStatistics
    {
        /// <summary>
        /// Calculates the difference between the last two values of a <see cref="IValueList"/>.
        /// </summary>
        public static decimal RecentChange(this IValueList valueList, ICurrency currency = null)
        {
            if (!valueList.Any())
            {
                return 0.0m;
            }

            if (valueList is IExchangeableValueList exchangeableValueList)
            {
                return exchangeableValueList.RecentChange(currency);
            }

            DailyValuation needed = valueList.LatestValue();
            if (needed.Value > 0)
            {
                return needed.Value - valueList.ValueBefore(needed.Day).Value;
            }

            return 0.0m;
        }

        /// <summary>
        /// Calculates the difference between the last two values of a <see cref="IExchangeableValueList"/>.
        /// </summary>
        public static decimal RecentChange(this IExchangeableValueList valueList, ICurrency currency)
        {
            if (!valueList.Any())
            {
                return 0m;
            }

            DailyValuation needed = valueList.LatestValue(currency);
            if (needed.Value > 0)
            {
                DailyValuation previousValue = valueList.ValueBefore(needed.Day, currency);
                return needed.Value - (previousValue?.Value ?? 0.0m);
            }

            return 0.0m;
        }
    }
}
