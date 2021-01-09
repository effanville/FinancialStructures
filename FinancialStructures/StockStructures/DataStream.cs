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
        Open,
        High,
        Low,
        Close,
        HighOpen,
        LowOpen,
        CloseOpen,
        Volume
    }
}
