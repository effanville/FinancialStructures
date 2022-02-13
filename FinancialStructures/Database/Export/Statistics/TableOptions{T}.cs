using System;
using System.Collections.Generic;
using FinancialStructures.Database.Statistics;

namespace FinancialStructures.Database.Export.Statistics
{
    /// <summary>
    /// Contains data for display of a table, where values are specified
    /// from an <see cref="Enum"/>.
    /// </summary>
    /// <typeparam name="T">An enumerable listing all possible values.</typeparam>
    public class TableOptions<T> where T : Enum
    {
        /// <summary>
        /// Should this table be displayed.
        /// </summary>
        public bool ShouldDisplay
        {
            get;
        }

        /// <summary>
        /// What field to sor the table by.
        /// </summary>
        public T SortingField
        {
            get;
        }

        /// <summary>
        /// In which direction to sort the table.
        /// </summary>
        public SortDirection SortingDirection
        {
            get;
        }

        /// <summary>
        /// What fields to display in the table.
        /// </summary>
        public IReadOnlyList<T> DisplayFields
        {
            get;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TableOptions()
        {
            ShouldDisplay = true;
            SortingDirection = SortDirection.Descending;
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="shouldDisplay">Whether this should be displayed.</param>
        /// <param name="sort">What field to sort the table by.</param>
        /// <param name="sortDirection">What direction to sort.</param>
        /// <param name="display">What fields to display.</param>
        public TableOptions(bool shouldDisplay, T sort, SortDirection sortDirection, IReadOnlyList<T> display)
        {
            ShouldDisplay = shouldDisplay;
            SortingField = sort;
            SortingDirection = sortDirection;
            DisplayFields = display;
        }
    }
}
