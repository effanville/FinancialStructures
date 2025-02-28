using System;
using Effanville.Common.Structure.DataEdit;
using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.FileAccess;

namespace Effanville.FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A named list containing values.
    /// </summary>
    public interface IValueList : IReadOnlyValueList, ICSVAccess
    {
        /// <summary>
        /// Tries to add data for the date specified if it doesnt exist, or edits data if it exists.
        /// If cannot add any value that one wants to, then doesn't add all the values chosen.
        /// </summary>
        /// <param name="oldDate">The existing date held.</param>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value data to add.</param>
        /// <returns>An update detailing whether the add or edit was successful.</returns>
        UpdateResult<DailyValuation> TryEditData(DateTime oldDate, DateTime date, decimal value);

        /// <summary>
        /// Sets data on the date specified to the value given. This overwrites the existing
        /// value if it exists.
        /// </summary>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value data to add.</param>
        /// <returns>An update detailing what data was set.</returns>
        UpdateResult<DailyValuation> SetData(DateTime date, decimal value);

        /// <summary>
        /// Attempts to delete data on the date specified.
        /// </summary>
        /// <param name="date">The date to delete data on.</param>
        /// <returns>An update detailing whether data was deleted or not and the value deleted.</returns>
        UpdateResult<DailyValuation> TryDeleteData(DateTime date);
    }
}
