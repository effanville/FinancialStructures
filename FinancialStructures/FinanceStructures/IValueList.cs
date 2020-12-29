using System;
using System.Collections.Generic;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using StructureCommon.FileAccess;
using StructureCommon.Reporting;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A named list containing values.
    /// </summary>
    public interface IValueList : ICSVAccess, IComparable
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
        /// Provides a short string representing the <see cref="IValueList"/>
        /// </summary>
        string ToString();

        /// <summary>
        /// Compares another <see cref="IValueList"/> and determines if they both have the same name and company.
        /// </summary>
        bool IsEqualTo(IValueList otherAccount);

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

        double CAR(DateTime earlierTime, DateTime laterTime);

        /// <summary>
        /// Retrieves data in a list ordered by date.
        /// </summary>
        List<DailyValuation> GetDataForDisplay();

        /// <summary>
        /// Edits the names of the Value list.
        /// </summary>
        /// <param name="newNames">The updated name to set.</param>
        /// <returns>Was updating name successful.</returns>
        bool EditNameData(NameData newNames);
        bool TryAddData(DateTime date, double value, IReportLogger reportLogger = null);
        bool TryAddOrEditData(DateTime oldDate, DateTime date, double value, IReportLogger reportLogger = null);
        bool TryDeleteData(DateTime date, IReportLogger reportLogger = null);
        bool TryRemoveSector(string sectorName);
        bool IsSectorLinked(string sectorName);
        int NumberSectors();
    }
}
