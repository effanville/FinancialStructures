using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;

namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Configuration object for the <see cref="StatsCreatorWindowViewModel"/>
    /// </summary>
    public sealed class StatsCreatorConfiguration : IConfiguration
    {
        /// <inheritdoc/>
        public Dictionary<string, IConfiguration> ChildConfigurations
        {
            get;
            set;
        }

        /// <inheritdoc/>
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
        public void LoadConfiguration(string filePath, IFileSystem fileSystem)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void RestoreFromConfiguration(object viewModel)
        {
        }

        /// <inheritdoc/>
        public void SaveConfiguration(string filePath, IFileSystem fileSystem)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void StoreConfiguration(object viewModel)
        {
        }
    }
}
