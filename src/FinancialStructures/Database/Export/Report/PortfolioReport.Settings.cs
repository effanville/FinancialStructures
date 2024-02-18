namespace FinancialStructures.Database.Export.Report
{
    public sealed partial class PortfolioReport
    {
        /// <summary>
        /// Contains settings for the creation of a portfolio report.
        /// </summary>
        public sealed class Settings
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
            public Settings(bool displayValueFunds)
            {
                DisplayValueFunds = displayValueFunds;
            }

            /// <summary>
            /// Generate default settings values.
            /// </summary>
            public static Settings Default()
            {
                return new Settings(displayValueFunds: true);
            }
        }
    }
}
