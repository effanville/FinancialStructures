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

        [DataMember]
        internal bool GenerateSecurityValues;

        [DataMember]
        internal bool GenerateBankAccountValues;

        [DataMember]
        internal bool GenerateSectorValues;

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
                vm.GenerateSecurityValues = GenerateSecurityValues;
                vm.GenerateBankAccountValues = GenerateBankAccountValues;
                vm.GenerateSectorValues = GenerateSectorValues;
            }
        }

        /// <inheritdoc/>
        public void StoreConfiguration(object viewModel)
        {
            if (viewModel is ExportHistoryViewModel vm)
            {
                HistoryGapDays = vm.HistoryGapDays;
                GenerateSecurityValues = vm.GenerateSecurityValues;
                GenerateBankAccountValues = vm.GenerateBankAccountValues;
                GenerateSectorValues = vm.GenerateSectorValues;
            }
        }
    }
}
