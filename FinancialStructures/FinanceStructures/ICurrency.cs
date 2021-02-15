namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Used to hold values to provide a currency exchange pair.
    /// </summary>
    public interface ICurrency : IValueList
    {
        /// <summary>
        /// The base currency the currency is derived from.
        /// E.g. in the pair GBP.HKD this is the GBP.
        /// </summary>
        string BaseCurrency
        {
            get;
        }

        /// <summary>
        /// The currency of the valuation.
        /// E.g. in the pair GBP.HKD this is the HKD.
        /// </summary>
        string QuoteCurrency
        {
            get;
        }

        /// <summary>
        /// Returns a currency where the values are given by the reciprocal of the current currency values.
        /// Also inverts the names in the currency pair.
        /// </summary>
        ICurrency Inverted();

        /// <summary>
        /// Makes a copy of the current currency.
        /// </summary>
        new ICurrency Copy();
    }
}
