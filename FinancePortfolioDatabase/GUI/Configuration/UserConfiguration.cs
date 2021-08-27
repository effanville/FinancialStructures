namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Contains user specific configuration for the ui.
    /// </summary>
    public sealed class UserConfiguration
    {
        /// <summary>
        /// The configuration for the stats window.
        /// </summary>
        public StatsDisplayConfiguration StatsConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserConfiguration()
        {
            StatsConfiguration = new StatsDisplayConfiguration();
        }
    }
}
