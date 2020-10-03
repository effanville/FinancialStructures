using System;
using StructureCommon.Extensions;

namespace FinancialStructures.StockData
{
    public enum TradeType
    {
        Buy,
        Sell
    }
    public class TradeDetails
    {
        private readonly string fTicker;
        private readonly string fName;
        private readonly string fCompany;
        private readonly DateTime fDay;
        private readonly double fTransactionValue;
        private readonly double fNumberSharesTraded;
        private readonly double fPricePerShare;
        private readonly double fTradeCosts;
        private readonly TradeType fTradeType;

        public TradeDetails()
        {
        }

        public TradeDetails(TradeType type)
        {
            fTradeType = type;
        }

        public TradeDetails(TradeType type, string ticker, string company, string name, DateTime day, double value, double numShares, double price, double costs)
        {
            fTradeType = type;
            fTicker = ticker;
            fCompany = company;
            fName = name;
            fDay = day;
            fTransactionValue = value;
            fNumberSharesTraded = numShares;
            fPricePerShare = price;
            fTradeCosts = costs;
        }

        public override string ToString()
        {
            return fDay.ToUkDateString() + "-" + fTradeType.ToString() + "-" + fCompany + "-" + fName + "-" + fTransactionValue + "-" + fNumberSharesTraded + "-" + fPricePerShare + "-" + fTradeCosts;
        }
    }
}
