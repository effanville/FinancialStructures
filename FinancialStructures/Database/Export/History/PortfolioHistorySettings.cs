namespace FinancialStructures.Database.Export.History
{
    /// <summary>
    /// Contains settings for a <see cref="PortfolioHistory"/>.
    /// </summary>
    public sealed class PortfolioHistorySettings
    {
        /// <summary>
        /// Should values for Securities be generated.
        /// </summary>
        public bool GenerateSecurityValues
        {
            get;
            set;
        }

        /// <summary>
        /// Should values for BankAccounts be generated.
        /// </summary>
        public bool GenerateBankAccountValues
        {
            get;
            set;
        }

        /// <summary>
        /// Should values for Sectors be generated.
        /// </summary>
        public bool GenerateSectorValues
        {
            get;
            set;
        }

        /// <summary>
        /// Should rates for Securities be generated.
        /// </summary>
        public bool GenerateSecurityRates
        {
            get;
            set;
        }

        /// <summary>
        /// Should rates for sectors be generated.
        /// </summary>
        public bool GenerateSectorRates
        {
            get;
            set;
        }

        /// <summary>
        /// The gap between days to record values for.
        /// </summary>
        public int SnapshotIncrement
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PortfolioHistorySettings(
            int snapshotIncrement = 20,
            bool generateSecurityValues = true,
            bool generateBankAccountValues = true,
            bool generateSectorValues = true,
            bool generateSecurityRates = false,
            bool generateSectorRates = false)
        {
            SnapshotIncrement = snapshotIncrement;
            GenerateSecurityRates = generateSecurityRates;
            GenerateSectorRates = generateSectorRates;
            GenerateSecurityValues = generateSecurityValues;
            GenerateBankAccountValues = generateBankAccountValues;
            GenerateSectorValues = generateSectorValues;
        }
    }
}
