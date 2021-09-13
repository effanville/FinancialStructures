using System;
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

        /// <inheritdoc/>
        public bool HasLoaded
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
            ChildConfigurations = new Dictionary<string, IConfiguration>();
        }

        /// <inheritdoc/>
        public void StoreConfiguration(object viewModel)
        {
            if (viewModel is StatsViewModel vm)
            {
                DisplayValueFunds = vm.DisplayValueFunds;
            }
        }

        /// <inheritdoc/>
        public void RestoreFromConfiguration(object viewModel)
        {
            if (viewModel is StatsViewModel vm)
            {
                vm.DisplayValueFunds = DisplayValueFunds;
            }
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
