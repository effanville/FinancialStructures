using FinancialStructures.Database;

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
        public static double Profit(this IValueList valueList)
        {
            if (!valueList.Any())
            {
                return double.NaN;
            }

            return valueList.LatestValue().Value - valueList.FirstValue().Value;
        }

        /// <summary>
        /// Calculates the difference between the last and investment of a <see cref="IExchangableValueList"/>.
        /// </summary>
        public static double Profit(this IExchangableValueList valueList, ICurrency currency, Account elementType)
        {
            if (!valueList.Any())
            {
                return double.NaN;
            }

            switch (elementType)
            {
                case Account.Security:
                {
                    ISecurity security = valueList as ISecurity;
                    return security.LatestValue(currency).Value - security.TotalInvestment(currency);
                }
                default:
                case Account.BankAccount:
                {
                    return valueList.LatestValue(currency).Value - valueList.FirstValue(currency).Value;
                }
            }
        }


        /// <summary>
        /// Calculates the difference between the last two values of a <see cref="IExchangableValueList"/>.
        /// </summary>
        public static double Profit(this ISecurity valueList, ICurrency currency)
        {
            if (!valueList.Any())
            {
                return double.NaN;
            }

            return valueList.LatestValue(currency).Value - valueList.TotalInvestment(currency);

        }
    }
}
