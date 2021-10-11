using FinancialStructures.DataStructures;

namespace FinancialStructures.FinanceStructures.Statistics
{
    /// <summary>
    /// Contains extension methods for calculating statistics for securities.
    /// </summary>
    public static partial class SecurityStatistics
    {
        /// <summary>
        /// Calculates the mean share price for the security.
        /// <para/>
        /// This is (Sum_Trades (price * numShares)) / total shares held
        /// </summary>
        public static double MeanSharePrice(this ISecurity security, TradeType tradeType)
        {
            double sum = 0.0;
            double numShares = 0.0;
            foreach (var trade in security.SecurityTrades)
            {
                if (trade.TradeType == tradeType)
                {
                    sum += trade.TotalCost;
                    numShares += trade.NumberShares;
                }
            }

            return sum / numShares;
        }
    }
}
