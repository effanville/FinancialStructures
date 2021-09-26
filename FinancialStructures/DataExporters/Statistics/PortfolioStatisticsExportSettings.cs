namespace FinancialStructures.DataExporters.Statistics
{
    /// <summary>
    /// A class containing settings for the export to file of a <see cref="PortfolioStatistics"/> object.
    /// </summary>
    public sealed class PortfolioStatisticsExportSettings
    {
        /// <summary>
        /// Display with spacing in tables.
        /// </summary>
        public bool Spacing
        {
            get;
            set;
        }

        /// <summary>
        /// Display with colours.
        /// </summary>
        public bool Colours
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PortfolioStatisticsExportSettings(bool spacing, bool colours)
        {
            Spacing = spacing;
            Colours = colours;
        }

        /// <summary>
        /// Creates default settings.
        /// </summary>
        /// <returns></returns>
        public static PortfolioStatisticsExportSettings DefaultSettings()
        {
            return new PortfolioStatisticsExportSettings(spacing: false, colours: false);
        }
    }
}
