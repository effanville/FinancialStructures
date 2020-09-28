namespace FinancialStructures.FinanceInterfaces
{
    /// <summary>
    /// The type of an account
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// Default, and used to cycle over all account types.
        /// </summary>
        All,

        /// <summary>
        /// A company comprising of a collection of securities
        /// </summary>
        SecurityCompany,

        /// <summary>
        /// A company comprising of a collection of bank accounts.
        /// </summary>
        BankAccountCompany,

        /// <summary>
        /// A company that comprises of securities or bank accounts, or both.
        /// </summary>
        Company,

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
        Currency,

        /// <summary>
        /// Artificial construct consisting of Securities and BankAccounts listed in the relevant Sector.
        /// </summary>
        Sector
    }
}
