using System;
using System.Collections.Generic;
using FinancialStructures.NamingStructures;
using Common.Structure.DataStructures;
using Common.Structure.FileAccess;
using Common.Structure.Reporting;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A named list containing values.
    /// </summary>
    public interface IValueList : ICSVAccess, IComparable, IComparable<IValueList>, IEquatable<IValueList>
    {
        /// <summary>
        /// The Name data for this list, including company, name and urls.
        /// </summary>
        NameData Names
        {
            get;
        }

        /// <summary>
        /// The values stored in this list. For use in serialisation or for cycling through
        /// the values stored. Should not be used for editing data.
        /// </summary>
        TimeList Values
        {
            get;
        }

        /// <summary>
        /// Returns a copy of this <see cref="IValueList"/>.
        /// </summary>
        IValueList Copy();

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
        /// </summary>
        DailyValuation LatestValue();

        /// <summary>
        /// The earliest value and date storend in this value list.
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
        DailyValuation RecentPreviousValue(DateTime date);

        /// <summary>
        /// Returns the latest valuation on or before the date <paramref name="date"/>.
        /// </summary>
        DailyValuation NearestEarlierValuation(DateTime date);

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

        /// <summary>
        /// Edits the names of the Value list.
        /// </summary>
        /// <param name="newNames">The updated name to set.</param>
        /// <returns>Was updating name successful.</returns>
        bool EditNameData(NameData newNames);

        /// <summary>
        /// Tries to add data for the date specified if it doesnt exist, or edits data if it exists.
        /// If cannot add any value that one wants to, then doesn't add all the values chosen.
        /// </summary>
        /// <param name="oldDate">The existing date held.</param>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value data to add.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>Was adding or editing successful.</returns>
        bool TryEditData(DateTime oldDate, DateTime date, double value, IReportLogger reportLogger = null);

        /// <summary>
        /// Sets data on the date specified to the value given. This overwrites the existing
        /// value if it exists.
        /// </summary>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value data to add.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        void SetData(DateTime date, double value, IReportLogger reportLogger = null);

        /// <summary>
        /// Attempts to delete data on the date specified.
        /// </summary>
        /// <param name="date">The date to delete data on.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>Whether data was deleted or not.</returns>
        bool TryDeleteData(DateTime date, IReportLogger reportLogger = null);

        /// <summary>
        /// Tries to remove a sector from the associated sectors.
        /// </summary>
        /// <param name="sectorName">The sector to remove.</param>
        /// <returns>Whether removal was successful or not.</returns>
        bool TryRemoveSector(TwoName sectorName);

        /// <summary>
        /// Is the sector listed in this <see cref="IValueList"/>
        /// </summary>
        /// <param name="sectorName">The sector to check.</param>
        bool IsSectorLinked(TwoName sectorName);

        /// <summary>
        /// The total number of sectors associated to this <see cref="IValueList"/>
        /// </summary>
        int NumberSectors();
    }
}
