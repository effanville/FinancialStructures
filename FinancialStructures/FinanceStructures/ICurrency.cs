namespace FinancialStructures.FinanceStructures
{
    public interface ICurrency : ISingleValueDataList
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
        /// </summary>
        /// <returns></returns>
        ICurrency Inverted();

        /// <summary>
        /// Makes a copy of the current currency.
        /// </summary>
        new ICurrency Copy();
    }
}
