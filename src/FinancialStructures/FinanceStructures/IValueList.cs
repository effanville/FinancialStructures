using System;
using Effanville.Common.Structure.FileAccess;
using Effanville.Common.Structure.Reporting;

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
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>Was adding or editing successful.</returns>
        bool TryEditData(DateTime oldDate, DateTime date, decimal value, IReportLogger reportLogger = null);

        /// <summary>
        /// Sets data on the date specified to the value given. This overwrites the existing
        /// value if it exists.
        /// </summary>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value data to add.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        void SetData(DateTime date, decimal value, IReportLogger reportLogger = null);

        /// <summary>
        /// Attempts to delete data on the date specified.
        /// </summary>
        /// <param name="date">The date to delete data on.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>Whether data was deleted or not.</returns>
        bool TryDeleteData(DateTime date, IReportLogger reportLogger = null);
    }
}
