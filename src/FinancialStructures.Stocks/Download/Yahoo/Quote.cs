namespace Effanville.FinancialStructures.Stocks.Download.Yahoo
{
    public class Quote
    {
        public double?[] low { get; set; }
        public double?[] open { get; set; }
        public int?[] volume { get; set; }
        public double?[] high { get; set; }
        public double?[] close { get; set; }
    }
}