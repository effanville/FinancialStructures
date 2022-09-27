namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Helper extensions for <see cref="SecurityTrade"/> logic.
    /// </summary>
    public static class SecurityTradeExtensions
    {
        /// <summary>
        /// Calculates the expected number of shares held after this trade.
        /// </summary>
        public static decimal GetPostTradeShares(this SecurityTrade trade, decimal previousNumShares)
        {
            decimal sign = trade.TradeType.Sign();
            return trade.TradeType == TradeType.ShareReset
                ? sign * trade.NumberShares
                : previousNumShares + sign * trade.NumberShares;
        }
    }
}
