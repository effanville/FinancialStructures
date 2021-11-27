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
        public static decimal MeanSharePrice(this ISecurity security, TradeType tradeType)
        {
            if (!security.Shares.Any())
            {
                return 0m;
            }

            decimal sum = 0.0m;
            decimal numShares = 0.0m;
            foreach (var trade in security.Trades)
            {
                if (trade.TradeType == tradeType)
                {
                    sum += trade.TotalCost;
                    numShares += trade.NumberShares;
                }
            }

            if (sum.Equals(0.0m))
            {
                return 0.0m;
            }

            return sum / numShares;
        }
    }
}
