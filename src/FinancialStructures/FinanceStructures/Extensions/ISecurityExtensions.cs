using System;
using System.Linq;
using Effanville.FinancialStructures.DataStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Extensions
{
    /// <summary>
    /// Contains extension methods for calculating statistics for securities.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class ISecurityExtensions
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
            foreach (SecurityTrade trade in security.Trades)
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

        public static DateTime LastPurchaseDate(this ISecurity security) 
            => security.Trades.LastOrDefault(trade => trade.TradeType.Equals(TradeType.Buy))?.Day ?? default(DateTime);

        public static DateTime LastInvestmentDate(this ISecurity security) 
            => security.LastInvestment()?.Day ?? DateTime.MinValue;
    }
}
