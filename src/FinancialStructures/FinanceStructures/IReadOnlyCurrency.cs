namespace Effanville.FinancialStructures.FinanceStructures
{
    public interface IReadOnlyCurrency : IReadOnlyValueList
    {
        /// <summary>
        /// The base currency the currency is derived from.
        /// E.g. in the pair GBP.HKD this is the GBP.
        /// </summary>
        string BaseCurrency { get; }

        /// <summary>
        /// The currency of the valuation.
        /// E.g. in the pair GBP.HKD this is the HKD.
        /// </summary>
        string QuoteCurrency { get; }

        /// <summary>
        /// Returns a currency where the values are given by the reciprocal of the current currency values.
        /// Also inverts the names in the currency pair.
        /// </summary>
        ICurrency Inverted();
    }
}