using System.Collections.Generic;
using System.Runtime.Serialization;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;

namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Configuration for the <see cref="ExportHistoryViewModel"/>.
    /// </summary>
    [DataContract]
    public sealed class ExportHistoryConfiguration : IConfiguration
    {
        [DataMember]
        internal int HistoryGapDays;

        /// <inheritdoc/>
        [DataMember(EmitDefaultValue = false)]
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
        public ExportHistoryConfiguration()
        {
            ChildConfigurations = new Dictionary<string, IConfiguration>();
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
        public void StoreConfiguration(object viewModel)
        {
            if (viewModel is ExportHistoryViewModel vm)
            {
                HistoryGapDays = vm.HistoryGapDays;
            }
        }
    }
}
