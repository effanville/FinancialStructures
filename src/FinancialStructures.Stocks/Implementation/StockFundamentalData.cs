using System.Collections.Generic;

namespace Effanville.FinancialStructures.Stocks.Implementation
{
    public class StockFundamentalData
    {
        public string Index { get; set; }
        public double PeRatio { get; set; }
        public double EPS { get; set; }
        public double Beta5YearMonth { get; set; }
        public double AverageVolume { get; set; }
        public double ForwardDividend { get; set; }
        public double ForwardYield { get; set; }
        public double MarketCap { get; set; }

        public StockFundamentalData() { }
        public StockFundamentalData(StockFundamentalData otherData) 
        { 
            Index = otherData.Index;
            PeRatio = otherData.PeRatio;
            EPS = otherData.EPS;
            Beta5YearMonth = otherData.Beta5YearMonth;
            AverageVolume = otherData.AverageVolume;
            ForwardDividend = otherData.ForwardDividend;
            ForwardYield = otherData.ForwardYield;
            MarketCap = otherData.MarketCap;}
        
        public bool Differs(Dictionary<string, double> dict)
        {
            bool valuesDiffer = true;
            if (dict.TryGetValue("Market cap", out double marketCap))
            {
                valuesDiffer = !MarketCap.Equals(marketCap);
            }
            if (dict.TryGetValue("PE ratio (TTM)", out double peRatio))
            {
                valuesDiffer = !PeRatio.Equals(peRatio);
            }
            if (dict.TryGetValue("Beta (5Y monthly)", out double beta))
            {
                valuesDiffer = !Beta5YearMonth.Equals(beta);
            }
            if (dict.TryGetValue("EPS (TTM)", out double eps))
            {
                valuesDiffer = !EPS.Equals(eps);
            }
            if (dict.TryGetValue("Forward dividend", out double forwardDividend))
            {
                valuesDiffer = !ForwardDividend.Equals(forwardDividend);
            }
            if (dict.TryGetValue("yield", out double yield))
            {
                valuesDiffer = !ForwardYield.Equals(yield);
            }
            if (dict.TryGetValue("Av. Volume", out double avVol))
            {
                valuesDiffer = !AverageVolume.Equals(avVol);
            }
            return valuesDiffer;
        }
    }
}