using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;

namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Configuration for the <see cref="ExportHistoryViewModel"/>.
    /// </summary>
    public sealed class ExportHistoryConfiguration : IConfiguration
    {
        private int HistoryGapDays;

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
        public ExportHistoryConfiguration()
        {
        }

        /// <inheritdoc/>
        public void LoadConfiguration(string filePath, IFileSystem fileSystem)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void RestoreFromConfiguration(object viewModel)
        {
            if (viewModel is ExportHistoryViewModel vm)
            {
                vm.HistoryGapDays = HistoryGapDays;
            }
        }

        /// <inheritdoc/>
        public void SaveConfiguration(string filePath, IFileSystem fileSystem)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void StoreConfiguration(object viewModel)
        {
            if (viewModel is ExportHistoryViewModel vm)
            {
                HistoryGapDays = vm.HistoryGapDays;
            }
        }
    }
}
