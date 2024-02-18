namespace FinancialStructures.Database
{
    /// <summary>
    /// The admissible total types one can calculate
    /// </summary>
    public enum Totals
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
        /// A company comprising of a collection of assets.
        /// </summary>
        AssetCompany,

        /// <summary>
        /// A company comprising of a collection of pensions.
        /// </summary>
        PensionCompany,

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
        /// The asset type.
        /// </summary>
        Asset,

        /// <summary>
        /// The pension type.
        /// </summary>
        Pension,

        /// <summary>
        /// Artificial construct consisting of Securities and BankAccounts listed in the relevant Sector.
        /// </summary>
        Sector,

        /// <summary>
        /// A sector comprising of a collection of securities.
        /// </summary>
        SecuritySector,

        /// <summary>
        /// A sector comprising of a collection of bank accounts.
        /// </summary>
        BankAccountSector,

        /// <summary>
        /// A sector comprising of a collection of assets.
        /// </summary>
        AssetSector,

        /// <summary>
        /// A sector comprising of a collection of pensions.
        /// </summary>
        PensionSector,

        /// <summary>
        /// A sector where all accounts are associated to the currency.
        /// </summary>
        CurrencySector,

        /// <summary>
        /// All securities associated to a currency.
        /// </summary>
        SecurityCurrency,

        /// <summary>
        /// All bank accounts associated to a currency.
        /// </summary>
        BankAccountCurrency,

        /// <summary>
        /// All assets associated to a currency.
        /// </summary>
        AssetCurrency,

        /// <summary>
        /// All pensions associated to a currency.
        /// </summary>
        PensionCurrency,
    }
}
