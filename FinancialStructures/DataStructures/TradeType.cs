namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// The type of the trade.
    /// </summary>
    public enum TradeType
    {
        /// <summary>
        /// It is not known the type of the trade. This is the default.
        /// </summary>
        Unknown,

        /// <summary>
        /// The trade was a buy.
        /// </summary>
        Buy,

        /// <summary>
        /// The trade was a sell.
        /// </summary>
        Sell,

        /// <summary>
        /// The trade was a dividend payout.
        /// </summary>
        Dividend,

        /// <summary>
        /// The trade was a dividend payout that was reinvested.
        /// </summary>
        DividendReinvestment,

        /// <summary>
        /// The price of one share was changed.
        /// </summary>
        ShareReprice,

        /// <summary>
        /// The trade was a payout in cash.
        /// </summary>
        CashPayout
    }
}
