namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Extension methods for <see cref="TradeType"/>s.
    /// </summary>
    public static class TradeTypeExtensions
    {
        /// <summary>
        /// Returns the sign for calculations with this trade type.
        /// </summary>
        public static double Sign(this TradeType tradeType)
        {
            return tradeType == TradeType.Sell || tradeType == TradeType.CashPayout ? -1.0 : 1.0;
        }
    }
}
