using System;
using StructureCommon.Extensions;

namespace FinancialStructures.StockStructures.Implementation
{
    /// <summary>
    /// Contains all information about a Stock trade.
    /// </summary>
    public class Trade
    {
        /// <summary>
        /// The type of this trade.
        /// </summary>
        public TradeType TradeType
        {
            get;
        }

        /// <summary>
        /// The <see cref="Stock.Ticker"/>
        /// </summary>
        public string Ticker
        {
            get;
        }

        private readonly string fName;
        private readonly string fCompany;
        private readonly DateTime fDay;
        private readonly double fTransactionValue;
        private readonly double fNumberSharesTraded;
        private readonly double fPricePerShare;
        private readonly double fTradeCosts;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Trade()
        {
        }

        /// <summary>
        /// Construc an instance with only a <see cref="TradeType"/>
        /// </summary>
        public Trade(TradeType type)
        {
            TradeType = type;
        }

        /// <summary>
        /// Create an instance filling in all data.
        /// </summary>
        public Trade(TradeType type, string ticker, string company, string name, DateTime day, double value, double numShares, double price, double costs)
        {
            TradeType = type;
            Ticker = ticker;
            fCompany = company;
            fName = name;
            fDay = day;
            fTransactionValue = value;
            fNumberSharesTraded = numShares;
            fPricePerShare = price;
            fTradeCosts = costs;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return fDay.ToUkDateString() + "-" + TradeType.ToString() + "-" + fCompany + "-" + fName + "-" + fTransactionValue + "-" + fNumberSharesTraded + "-" + fPricePerShare + "-" + fTradeCosts;
        }
    }
}
