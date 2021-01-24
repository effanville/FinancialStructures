using System;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A type of account used to simulate a Bank account, or similar. Contains a single
    /// list of values, but does not keep track of investments into, or IRR of the account.
    /// </summary>
    public interface ICashAccount : IExchangableValueList, IValueList
    {
        /// <summary>
        /// Returns a copy of this <see cref="ICashAccount"/>.
        /// </summary>
        new ICashAccount Copy();

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

        /// <summary>
        /// Returns the latest valuation on or before the date <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The date to query the value for.</param>
        /// <param name="currency">An optional currency to exchange the value with.</param>
        DailyValuation NearestEarlierValuation(DateTime date, ICurrency currency);

        /// <summary>
        /// Returns the most recent value to <paramref name="date"/> that is prior to that date.
        /// This value is strictly prior to <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The date to query the value on.</param>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation RecentPreviousValue(DateTime date, ICurrency currency);
    }
}
