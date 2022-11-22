namespace FinancialStructures.Database.Statistics
{
    /// <summary>
    /// The direction for sorting.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Sorting occurs in an ascending direction.
        /// </summary>
        Ascending,

        /// <summary>
        /// Sorting occurs in a descending direction.
        /// </summary>
        Descending
    }

    /// <summary>
    /// helpers for the SortDirection enum
    /// </summary>
    public static class SortDirectionHelpers
    {
        /// <summary>
        /// Inverts the SortingDirection.
        /// </summary>
        public static SortDirection Invert(SortDirection sortDirection)
        {
            if(sortDirection == SortDirection.Ascending)
            {
                return SortDirection.Descending;
            }
            else
            {
                return SortDirection.Ascending;
            }
        }
    }
}
