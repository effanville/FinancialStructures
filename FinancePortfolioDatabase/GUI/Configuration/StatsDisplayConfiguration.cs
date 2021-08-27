namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Configuration for the stats display
    /// </summary>
    public sealed class StatsDisplayConfiguration
    {
        /// <summary>
        /// Flag determining whether display has loaded yet.
        /// </summary>
        public bool HasLoaded
        {
            get;
            set;
        }

        /// <summary>
        /// Which tab was last selected.
        /// </summary>
        public int SelectedTab
        {
            get;
            set;
        }

        public StatsOptionsDisplayConfiguration OptionsConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatsDisplayConfiguration()
        {
            OptionsConfiguration = new StatsOptionsDisplayConfiguration();
        }
    }
}
