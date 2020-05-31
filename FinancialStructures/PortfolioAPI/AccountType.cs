namespace FinancialStructures.PortfolioAPI
{
    /// <summary>
    /// The type of an account
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// The security type.
        /// </summary>
        Security,

        /// <summary>
        /// The sector type.
        /// </summary>
        Sector,

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
