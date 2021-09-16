using FinancialStructures.StockStructures.Implementation;

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

        /// <summary>
        /// The ratio of the Days high to the days open prices.
        /// </summary>
        HighOpen,

        /// <summary>
        /// The ratio of the days low to the days open prices.
        /// </summary>
        LowOpen,

        /// <summary>
        /// The ratio of the Days close to the days opening price.
        /// </summary>
        CloseOpen,

        /// <summary>
        /// The total volume.
        /// </summary>
        Volume
    }
}
