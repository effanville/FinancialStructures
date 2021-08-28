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
        public const string StatsDisplay = nameof(StatsCreatorWindowViewModel);

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
                { StatsDisplay, new StatsDisplayConfiguration() }
            };
        }

        /// <inheritdoc/>
        public void StoreConfiguration(object viewModel)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void RestoreFromConfiguration(object viewModel)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void LoadConfiguration(string filePath, IFileSystem fileSystem)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void SaveConfiguration(string filePath, IFileSystem fileSystem)
        {
            throw new System.NotImplementedException();
        }
    }
}
