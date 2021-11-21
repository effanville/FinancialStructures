namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Extension methods for <see cref="TradeType"/>s.
    /// </summary>
    public static class TradeTypeExtensions
    {
        /// <summary>
        /// Returns the sign for calculations with this trade type.
        /// This is the sign in the sense of whether the trade details 
        /// money leaving the account or not.
        /// </summary>
        public static double Sign(this TradeType tradeType)
        {
            return tradeType == TradeType.Sell
                || tradeType == TradeType.CashPayout
                || tradeType == TradeType.Dividend
                    ? -1.0
                    : 1.0;
        }

        /// <summary>
        /// Does the <see cref="TradeType"/> correspond to a type of trade pertaining
        /// to an investment in the underlying.
        /// <para/>
        /// The following do consist in an investment:<br/>
        /// 1. Buy<br/>
        /// 2. Sell<br/>
        /// 3. Dividend (as money has been payed out here)<br/>
        /// 4. CashPayout (money has been payed out here)<br/>
        /// <para/>
        /// The following do not consist in an investment:<br/>
        /// 1. ShareReprice (as the fundamental price is just altered)<br/>
        /// 2. Dividend Reinvestment (as no new money has been added)
        /// </summary>
        public static bool IsInvestmentTradeType(this TradeType tradeType)
        {
            if (tradeType.Equals(TradeType.Buy)
                || tradeType.Equals(TradeType.Sell)
                || tradeType.Equals(TradeType.Dividend)
                || tradeType.Equals(TradeType.CashPayout))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Does the <see cref="TradeType"/> correspond to a type of trade pertaining
        /// to an alteration in the number of shares held in the underlying.
        /// <para/>
        /// The following do consist in an investment:<br/>
        /// 1. Buy<br/>
        /// 2. Sell<br/>
        /// 3. DividendReinvestment (as the dividend was reset into more shares)<br/>
        /// 4. ShareReprice (as the number shares has changed)<br/>
        /// <para/>
        /// The following do not consist in an investment:<br/>
        /// 1. Cash Payout<br/>
        /// 2. Dividend (as the dividend doesnt buy more shares)
        /// </summary>
        public static bool IsShareNumberAlteringTradeType(this TradeType tradeType)
        {
            if (tradeType.Equals(TradeType.Buy)
               || tradeType.Equals(TradeType.Sell)
               || tradeType.Equals(TradeType.DividendReinvestment)
               || tradeType.Equals(TradeType.ShareReprice))
            {
                return true;
            }

            return false;
        }
    }
}
