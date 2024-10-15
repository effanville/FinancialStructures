namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    internal class PreviousDayOpen : PreviousNDayRelativeValue
    {
        public PreviousDayOpen()
            : base(1, StockDataStream.Open)
        {
            NumberDaysPriorValue = 1;
        }

        public PreviousDayOpen(StockStatisticSettings settings)
            : base(1, StockDataStream.Open)
        {
            NumberDaysPriorValue = 1;
        }
    }
}
