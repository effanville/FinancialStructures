using System;
using Effanville.Common.Structure.DataStructures;

namespace Effanville.FinancialStructures.FinanceStructures
{
    public interface IReadOnlyExchangeableValueList : IReadOnlyValueList
    {
        /// <summary>
        /// The first value and date stored in this list.
        /// </summary>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation FirstValue(IReadOnlyCurrency currency);

        /// <summary>
        /// The latest value and date stored in the value list.
        /// </summary>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation LatestValue(IReadOnlyCurrency currency);

        /// <summary>
        /// Gets the value on the specific date specified.
        /// This is a linearly interpolated value from those values provided,
        /// with the initial value if date is less that the first value.
        /// </summary>
        /// <param name="date">The date to query the value on.</param>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation Value(DateTime date, IReadOnlyCurrency currency);

        /// <summary>
        /// Returns the most recent value to <paramref name="date"/> that is prior to that date.
        /// This value is strictly prior to <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The date to query the value on.</param>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation ValueBefore(DateTime date, IReadOnlyCurrency currency);

        /// <summary>
        /// Returns the latest valuation on or before the date <paramref name="date"/>.
        /// This value can be on the date <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The date to query the value for.</param>
        /// <param name="currency">An optional currency to exchange the value with.</param>
        DailyValuation ValueOnOrBefore(DateTime date, IReadOnlyCurrency currency);
    }
}