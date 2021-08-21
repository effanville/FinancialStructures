using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.Statistics;

namespace FinancialStructures.DataExporters.ExportOptions
{
    /// <summary>
    /// Contains display options for a table of statistics.
    /// </summary>
    public class StatisticTableOptions
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
        public Statistic SortingField
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
        public List<Statistic> DisplayFields
        {
            get;
        } = new List<Statistic>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatisticTableOptions()
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
        public StatisticTableOptions(bool shouldDisplay, Statistic sort, SortDirection sortDirection, List<Statistic> display)
        {
            if (!display.Contains(sort))
            {
                throw new Exception($"The table should be sorted by {sort} but this was not selected to display.");
            }
            ShouldDisplay = shouldDisplay;
            SortingField = sort;
            SortingDirection = sortDirection;
            DisplayFields = display;
        }

        /// <summary>
        /// Retrieve display names as strings.
        /// </summary>
        public IEnumerable<string> DisplayFieldNames()
        {
            return DisplayFields.Select(Database => Database.ToString());
        }
    }
}
