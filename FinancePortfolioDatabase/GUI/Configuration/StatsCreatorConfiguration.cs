using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Runtime.Serialization;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;

namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Configuration object for the <see cref="StatsCreatorWindowViewModel"/>
    /// </summary>
    [DataContract]
    public sealed class StatsCreatorConfiguration : IConfiguration
    {
        /// <inheritdoc/>
        [DataMember]
        public Dictionary<string, IConfiguration> ChildConfigurations
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [DataMember]
        public bool HasLoaded
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatsCreatorConfiguration()
        {
            ChildConfigurations = new Dictionary<string, IConfiguration>
            {
                { UserConfiguration.StatsOptions, new ExportStatsConfiguration() },
                { UserConfiguration.HistoryOptions, new ExportHistoryConfiguration() }
            };
        }

        /// <inheritdoc/>
        public void RestoreFromConfiguration(object viewModel)
        {
        }

        /// <inheritdoc/>
        public void StoreConfiguration(object viewModel)
        {
        }
    }
}
