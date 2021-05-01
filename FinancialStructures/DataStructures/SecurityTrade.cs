using System;
using FinancialStructures.NamingStructures;
using Common.Structure.Extensions;

namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Contains all information about a Stock trade.
    /// </summary>
    public class SecurityTrade

    {
        /// <summary>
        /// The type of this trade.
        /// </summary>
        public TradeType TradeType
        {
            get;
            set;
        }

        public TwoName Names
        {
            get;
            set;
        }

        public DateTime Day
        {
            get;
            set;
        }

        public double TotalCost
        {
            get
            {
                if (TradeType == TradeType.Buy)
                {
                    return NumberShares * UnitPrice + TradeCosts;
                }

                if (TradeType == TradeType.Sell)
                {
                    return NumberShares * UnitPrice - TradeCosts;
                }

                return 0.0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public double NumberShares
        {
            get;
            set;
        }

        /// <summary>
        /// The price of the underlying that this trade was enacted at.
        /// </summary>
        public double UnitPrice
        {
            get;
            set;
        }

        /// <summary>
        /// The cost of performing this trade. Encompasses all fixed costs and
        /// percentage costs.
        /// </summary>
        public double TradeCosts
        {
            get;
            set;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public SecurityTrade()
        {
        }

        /// <summary>
        /// Construct an instance with only a <see cref="TradeType"/>
        /// </summary>
        public SecurityTrade(TradeType type)
        {
            TradeType = type;
        }

        /// <summary>
        /// Create an instance filling in all data.
        /// </summary>
        public SecurityTrade(TradeType type, TwoName names, DateTime day, double numShares, double price, double costs)
        {
            TradeType = type;
            Names = names;
            Day = day;
            NumberShares = numShares;
            UnitPrice = price;
            TradeCosts = costs;
        }

        /// <summary>
        /// Provides a copy of this <see cref="SecurityTrade"/>.
        /// </summary>
        public SecurityTrade Copy()
        {
            return new SecurityTrade(TradeType, Names, Day, NumberShares, UnitPrice, TradeCosts);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Day.ToUkDateString() + "-" + TradeType.ToString() + "-" + Names.Company + "-" + Names.Name + "-" + TotalCost + "-" + NumberShares + "-" + UnitPrice + "-" + TradeCosts;
        }
    }
}
