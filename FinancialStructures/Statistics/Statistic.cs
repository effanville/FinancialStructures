namespace FinancialStructures.Statistics
{
    /// <summary>
    /// Contains all admissible statistics.
    /// </summary>
    public enum Statistic
    {
        /// <summary>
        /// The company.
        /// </summary>
        Company,

        /// <summary>
        /// The Name.
        /// </summary>
        Name,

        /// <summary>
        /// The LatestValue held.
        /// </summary>
        LatestValue,

        /// <summary>
        /// The latest UnitPrice held.
        /// </summary>
        UnitPrice,

        /// <summary>
        /// The number of Units held.
        /// </summary>
        NumberUnits,

        /// <summary>
        /// The recent change.
        /// </summary>
        RecentChange,

        /// <summary>
        /// The fraction out of all securities.
        /// </summary>
        FundFraction,

        /// <summary>
        /// The fraction out of all held in that company.
        /// </summary>
        FundCompanyFraction,

        /// <summary>
        /// The total profit.
        /// </summary>
        Profit,

        /// <summary>
        /// The IRR over the past 3 months of this.
        /// </summary>
        IRR3M,

        /// <summary>
        /// The IRR over the past 6 months of this.
        /// </summary>
        IRR6M,

        /// <summary>
        /// The IRR over the past 1 year of this.
        /// </summary>
        IRR1Y,

        /// <summary>
        /// The IRR over the past 5 years of this.
        /// </summary>
        IRR5Y,

        /// <summary>
        /// The total IRR of this.
        /// </summary>
        IRRTotal,

        /// <summary>
        /// The sectors recorded in this.
        /// </summary>
        Sectors,

        /// <summary>
        /// The number of accounts associated with this.
        /// </summary>
        NumberOfAccounts,

        /// <summary>
        /// The first date held.
        /// </summary>
        FirstDate,

        /// <summary>
        /// The latest date held.
        /// </summary>
        LatestDate,

        /// <summary>
        /// The number of data entries for this.
        /// </summary>
        NumberEntries,

        /// <summary>
        /// The number of data entries per year.
        /// </summary>
        EntryYearDensity
    }
}
