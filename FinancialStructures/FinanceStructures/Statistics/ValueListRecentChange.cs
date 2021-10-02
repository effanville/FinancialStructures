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
        public static double RecentChange(this IValueList valueList)
        {
            DailyValuation needed = valueList.LatestValue();
            if (needed.Value > 0)
            {
                return needed.Value - valueList.RecentPreviousValue(needed.Day).Value;
            }

            return 0.0;
        }

        /// <summary>
        /// Calculates the difference between the last two values of a <see cref="IExchangableValueList"/>.
        /// </summary>
        public static double RecentChange(this IExchangableValueList valueList, ICurrency currency)
        {
            DailyValuation needed = valueList.LatestValue(currency);
            if (needed.Value > 0)
            {
                return needed.Value - valueList.RecentPreviousValue(needed.Day, currency).Value;
            }

            return 0.0;
        }
    }
}
