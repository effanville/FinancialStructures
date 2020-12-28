using System;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures
{
    public interface ICashAccount : IValueList
    {
        /// <summary>
        /// Returns a copy of this <see cref="ICashAccount"/>.
        /// </summary>
        new ICashAccount Copy();
        DailyValuation Value(DateTime date, ICurrency currency = null);

        /// <summary>
        /// The latest value and date stored in the value list.
        /// </summary>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation LatestValue(ICurrency currency = null);
        DailyValuation FirstValue(ICurrency currency = null);
        DailyValuation NearestEarlierValuation(DateTime date, ICurrency currency = null);
        DailyValuation LastEarlierValuation(DateTime date, ICurrency currency = null);
    }
}
