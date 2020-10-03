namespace FinancialStructures.FinanceInterfaces
{
    /// <summary>
    /// The admissible account types that the Portfolio stores.
    /// These directly correspond to those structures that exist in the database.
    /// </summary>
    public enum Account
    {
        /// <summary>
        /// Default, and used to cycle over all account types.
        /// </summary>
        All,

        /// <summary>
        /// The security type.
        /// </summary>
        Security,

        /// <summary>
        /// The Benchmark type (from the benchmark list).
        /// </summary>
        Benchmark,

        /// <summary>
        /// The bank account(or any similar account) type.
        /// </summary>
        BankAccount,

        /// <summary>
        /// The currency type.
        /// </summary>
        Currency
    }
}
