using System.Collections.Generic;
using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;

namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Configuration for the stats display
    /// </summary>
    public sealed class StatsDisplayConfiguration : IConfiguration
    {
        private bool DisplayValueFunds;

        /// <summary>
        /// Name of the child configuration for the StatsOptions window.
        /// </summary>
        public const string StatsOptions = nameof(StatsOptionsViewModel);

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

        /// <inheritdoc/>
        public Dictionary<string, IConfiguration> ChildConfigurations
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatsDisplayConfiguration()
        {
            ChildConfigurations = new Dictionary<string, IConfiguration>
            {
                { StatsOptions, new StatsOptionsDisplayConfiguration() }
            };
        }

        /// <inheritdoc/>
        public void StoreConfiguration(object viewModel)
        {
            if (viewModel is StatsCreatorWindowViewModel vm)
            {
                DisplayValueFunds = vm.DisplayValueFunds;
            }
        }

        /// <inheritdoc/>
        public void RestoreFromConfiguration(object viewModel)
        {
            if (viewModel is StatsCreatorWindowViewModel vm)
            {
                vm.DisplayValueFunds = DisplayValueFunds;
            }
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
