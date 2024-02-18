namespace FinancialStructures.FinanceStructures.Statistics
{
    /// <summary>
    /// Contains extension methods for statistics of <see cref="IValueList"/>s.
    /// </summary>
    public static partial class ValueListStatistics
    {
        /// <summary>
        /// Calculates the difference between the first and last values of a <see cref="IValueList"/>.
        /// </summary>
        public static decimal Profit(this IValueList valueList)
        {
            if (!valueList.Any())
            {
                return 0.0m;
            }

            return valueList.LatestValue().Value - valueList.FirstValue().Value;
        }

        /// <summary>
        /// Calculates the difference between the last and investment of a <see cref="IExchangableValueList"/>.
        /// </summary>
        public static decimal Profit(this IExchangableValueList valueList, ICurrency currency)
        {
            if (valueList is ISecurity security)
            {
                return security.Profit(currency);
            }
            if (valueList is IAmortisableAsset asset)
            {
                return asset.Profit(currency);
            }

            if (!valueList.Any())
            {
                return 0.0m;
            }

            return valueList.LatestValue(currency).Value - valueList.FirstValue(currency).Value;

        }


        /// <summary>
        /// Calculates the difference between the last two values of a <see cref="IExchangableValueList"/>.
        /// </summary>
        public static decimal Profit(this ISecurity valueList, ICurrency currency)
        {
            if (!valueList.Any())
            {
                return 0.0m;
            }

            return valueList.LatestValue(currency).Value - valueList.TotalInvestment(currency);
        }

        /// <summary>
        /// Calculates the difference between the last two values of a <see cref="IAmortisableAsset"/>.
        /// </summary>
        public static decimal Profit(this IAmortisableAsset asset, ICurrency currency)
        {
            if (!asset.Any())
            {
                return 0.0m;
            }

            return asset.LatestValue(currency).Value - asset.TotalCost(currency);
        }
    }
}
