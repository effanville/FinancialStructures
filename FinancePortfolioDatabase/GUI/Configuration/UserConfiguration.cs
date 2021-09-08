using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;

namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Contains user specific configuration for the ui.
    /// </summary>
    public sealed class UserConfiguration : IConfiguration
    {
        /// <summary>
        /// Name of the child configuration for the stats window.
        /// </summary>
        public const string StatsDisplay = nameof(StatsViewModel);

        /// <summary>
        /// Name of the child configuration for the StatsOptions window.
        /// </summary>
        public const string StatsOptions = nameof(StatsOptionsViewModel);

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
        public UserConfiguration()
        {
            ChildConfigurations = new Dictionary<string, IConfiguration>
            {
                { StatsDisplay, new StatsDisplayConfiguration() },
                { StatsOptions, new StatsOptionsDisplayConfiguration() }
            };
        }

        /// <inheritdoc/>
        public void StoreConfiguration(object viewModel)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void RestoreFromConfiguration(object viewModel)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void LoadConfiguration(string filePath, IFileSystem fileSystem)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void SaveConfiguration(string filePath, IFileSystem fileSystem)
        {
            throw new NotImplementedException();
        }
    }
}
