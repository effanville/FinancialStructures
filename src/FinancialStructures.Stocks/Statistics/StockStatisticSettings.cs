namespace Effanville.FinancialStructures.Stocks.Statistics
{
    public class AdxStatisticSettings : StockStatisticSettings
    {
        public int SmoothingPeriod { get; }
        public AdxStatisticSettings(int smoothingPeriod, string statType, int lag, StockDataStream priceType) 
            : base(statType, lag, priceType)
        {
            SmoothingPeriod = smoothingPeriod;
        }
    }

    public class StockStatisticSettings
    {
        public string StatType { get; private set; }
        public int Lag { get; private init; }
        public StockDataStream PriceType { get; private set; }

        public StockStatisticSettings(string statType, int lag, StockDataStream priceType)
        {
            StatType = statType;
            Lag = lag;
            PriceType = priceType;
        }
    }
}