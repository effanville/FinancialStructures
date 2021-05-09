namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A value list designed to hold the data for a specific area, like a benchmark.
    /// </summary>
    /// <remarks>
    /// This is largely in existence so that it is clear what it is.</remarks>
    public interface ISector : IValueList
    {
        /// <summary>
        /// Makes a copy of the current Sector.
        /// </summary>
        new ISector Copy();
    }
}
