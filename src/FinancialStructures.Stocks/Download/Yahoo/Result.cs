namespace Effanville.FinancialStructures.Stocks.Download.Yahoo
{
    public class Result
    {
        public Meta meta { get; set; }
        public int[] timestamp { get; set; }
        public Indicators indicators { get; set; }
    }
}