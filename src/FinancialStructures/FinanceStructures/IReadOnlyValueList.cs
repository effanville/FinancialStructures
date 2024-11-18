using System;
using System.Collections.Generic;
using Effanville.Common.Structure.DataStructures;

namespace Effanville.FinancialStructures.FinanceStructures
{
    public interface IReadOnlyValueList : INamedFinancialObject, IComparable, IComparable<IReadOnlyValueList>,
        IEquatable<IReadOnlyValueList>
    {
        /// <summary>
        /// The values stored in this list. For use in serialisation or for cycling through
        /// the values stored. Should not be used for editing data.
        /// </summary>
        TimeList Values { get; }
        
        /// <summary>
        /// Compares to another valueList based upon the value on the specified date.
        /// </summary>
        int ValueComparison(IReadOnlyValueList otherList, DateTime dateTime);
        
        /// <summary>
        /// Whether any value data is held.
        /// </summary>
        bool Any();
        
        /// <summary>
        /// Returns the number of entries in the Value list.
        /// </summary>
        int Count();

        /// <summary>
        /// The latest value and date stored in the value list.
        /// <para/>
        /// If no entries are held, returns null.
        /// </summary>
        DailyValuation LatestValue();

        /// <summary>
        /// The earliest value and date stored in this value list.
        /// <para/>
        /// If no entries are held, returns null.
        /// </summary>
        DailyValuation FirstValue();

        /// <summary>
        /// The value of the list on the specific date.
        /// This is a linearly interpolated value from those values provided,
        /// with the initial value if date is less that the first value.
        /// </summary>
        /// <param name="date">The date to retrieve the value for.</param>
        DailyValuation Value(DateTime date);

        /// <summary>
        /// Returns the most recent value to <paramref name="date"/> that is prior to that date.
        /// This value is strictly prior to <paramref name="date"/>.
        /// </summary>
        DailyValuation ValueBefore(DateTime date);

        /// <summary>
        /// Returns the latest valuation on or before the date <paramref name="date"/>.
        /// </summary>
        DailyValuation ValueOnOrBefore(DateTime date);

        /// <summary>
        /// Calculates the compound annual rate of the Value list.
        /// This is the compound rate from the value on <paramref name="earlierTime"/>
        /// to reach the value at <paramref name="laterTime"/>.
        /// </summary>
        /// <param name="earlierTime">The start time.</param>
        /// <param name="laterTime">The end time.</param>
        double CAR(DateTime earlierTime, DateTime laterTime);

        /// <summary>
        /// Retrieves data in a list ordered by date.
        /// </summary>
        List<DailyValuation> ListOfValues();
    }
}