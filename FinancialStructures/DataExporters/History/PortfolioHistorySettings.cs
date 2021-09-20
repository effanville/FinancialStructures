namespace FinancialStructures.DataExporters.History
{
    /// <summary>
    /// Contains settings for a <see cref="PortfolioHistory"/>.
    /// </summary>
    public sealed class PortfolioHistorySettings
    {
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
        public PortfolioHistorySettings(int snapshotIncrement = 20, bool generateSecurityRates = false, bool generateSectorRates = false)
        {
            SnapshotIncrement = snapshotIncrement;
            GenerateSecurityRates = generateSecurityRates;
            GenerateSectorRates = generateSectorRates;
        }
    }
}
