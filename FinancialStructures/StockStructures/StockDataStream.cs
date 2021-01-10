namespace FinancialStructures.StockStructures
{
    /// <summary>
    /// Possible streams of data to retrieve from a <see cref="Stock"/>.
    /// </summary>
    public enum StockDataStream
    {
        /// <summary>
        /// The default value.
        /// </summary>
        None,

        /// <summary>
        /// The Opening value.
        /// </summary>
        Open,

        /// <summary>
        /// The high value.
        /// </summary>
        High,

        /// <summary>
        /// The low value.
        /// </summary>
        Low,

        /// <summary>
        /// The closing value.
        /// </summary>
        Close,
        HighOpen,
        LowOpen,
        CloseOpen,

        /// <summary>
        /// The total volume.
        /// </summary>
        Volume
    }
}
