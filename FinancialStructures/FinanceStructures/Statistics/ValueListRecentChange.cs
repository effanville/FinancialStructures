using Common.Structure.DataStructures;

namespace FinancialStructures.FinanceStructures.Statistics
{
    /// <summary>
    /// Contains extension methods for statistics of <see cref="IValueList"/>s.
    /// </summary>
    public static partial class ValueListStatistics
    {
        /// <summary>
        /// Calculates the difference between the last two values of a <see cref="IValueList"/>.
        /// </summary>
        public static decimal RecentChange(this IValueList valueList)
        {
            if (!valueList.Any())
            {
                return 0m;
            }

            DailyValuation needed = valueList.LatestValue();
            if (needed.Value > 0)
            {
                return needed.Value - valueList.ValueBefore(needed.Day).Value;
            }

            return 0.0m;
        }

        /// <summary>
        /// Calculates the difference between the last two values of a <see cref="IExchangableValueList"/>.
        /// </summary>
        public static decimal RecentChange(this IExchangableValueList valueList, ICurrency currency)
        {
            if (!valueList.Any())
            {
                return 0m;
            }

            DailyValuation needed = valueList.LatestValue(currency);
            if (needed.Value > 0)
            {
                var previousValue = valueList.ValueBefore(needed.Day, currency);
                return needed.Value - (previousValue != null ? previousValue.Value : 0.0m);
            }

            return 0.0m;
        }
    }
}
