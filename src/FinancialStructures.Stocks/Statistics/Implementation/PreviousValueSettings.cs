namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    internal class PreviousValueSettings : StockStatisticSettings
    {
        public int NumberDaysPriorValue { get; }
        public PreviousValueSettings(int numberDaysAgoValue, string statType, int lag, StockDataStream priceType) 
            : base(statType, lag, priceType)
        {
            NumberDaysPriorValue = numberDaysAgoValue;
        }
    }
}