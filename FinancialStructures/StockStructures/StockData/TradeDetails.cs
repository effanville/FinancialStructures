using System;

namespace FinancialStructures.StockData
{
    public enum TradeType
    {
        Buy,
        Sell
    }
    public class TradeDetails
    {
        private string fTicker;
        private string fName;
        private string fCompany;
        private DateTime fDay;
        private double fTransactionValue;
        private double fNumberSharesTraded;
        private double fPricePerShare;
        private double fTradeCosts;
        private TradeType fTradeType;

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
    }
}
