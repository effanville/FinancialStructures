using System;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A <see cref="IValueList"/> implementation that also has the ability to exchange the values with a <see cref="ICurrency"/>.
    /// </summary>
    public interface IExchangableValueList : IValueList
    {
        /// <summary>
        /// The first value and date stored in this security.
        /// </summary>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation FirstValue(ICurrency currency);

        /// <summary>
        /// The latest value and date stored in the value list.
        /// </summary>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation LatestValue(ICurrency currency);

        /// <summary>
        /// Gets the value on the specific date specified.
        /// This is a linearly interpolated value from those values provided,
        /// with the initial value if date is less that the first value.
        /// </summary>
        /// <param name="date">The date to query the value on.</param>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation Value(DateTime date, ICurrency currency);
    }
}
