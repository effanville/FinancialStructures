namespace FinancialStructures.Database.Export.Report
{
    /// <summary>
    /// Contains settings for the creation of a portfolio report.
    /// </summary>
    public sealed class PortfolioReportSettings
    {
        /// <summary>
        /// Only display accounts that have non zero current value.
        /// </summary>
        public bool DisplayValueFunds
        {
            get;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PortfolioReportSettings(bool displayValueFunds)
        {
            DisplayValueFunds = displayValueFunds;
        }

        /// <summary>
        /// Generate default settings values.
        /// </summary>
        public static PortfolioReportSettings DefaultSettings()
        {
            return new PortfolioReportSettings(displayValueFunds: true);
        }
    }
}
