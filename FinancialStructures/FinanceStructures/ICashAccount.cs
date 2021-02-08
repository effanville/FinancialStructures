namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A type of account used to simulate a Bank account, or similar. Contains a single
    /// list of values, but does not keep track of investments into, or IRR of the account.
    /// </summary>
    public interface ICashAccount : IExchangableValueList, IValueList
    {
        /// <summary>
        /// Returns a copy of this <see cref="ICashAccount"/>.
        /// </summary>
        new ICashAccount Copy();
    }
}
