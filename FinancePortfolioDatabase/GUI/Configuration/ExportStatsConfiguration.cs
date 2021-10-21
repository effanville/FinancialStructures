using System.Collections.Generic;
using System.Runtime.Serialization;
using Common.Structure.DisplayClasses;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using FinancialStructures.Statistics;

namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Configuration for the stats display
    /// </summary>
    [DataContract]
    public sealed class ExportStatsConfiguration : IConfiguration
    {
        // Internal configurations to store.
        [DataMember(EmitDefaultValue = false)]
        private List<Selectable<string>> DisplayConditions = new List<Selectable<string>>();
        [DataMember]
        private Statistic SecuritySortingField;
        [DataMember]
        private SortDirection SecurityDirection;
        [DataMember(EmitDefaultValue = false)]
        private List<Selectable<Statistic>> SecurityColumnNames = new List<Selectable<Statistic>>();
        [DataMember]
        private Statistic BankSortingField;

        [DataMember]
        private SortDirection BankDirection;
        [DataMember(EmitDefaultValue = false)]
        private List<Selectable<Statistic>> BankColumnNames = new List<Selectable<Statistic>>();
        [DataMember]
        private Statistic SectorSortingField;
        [DataMember]
        private SortDirection SectorDirection;
        [DataMember(EmitDefaultValue = false)]
        private List<Selectable<Statistic>> SectorColumnNames = new List<Selectable<Statistic>>();

        /// <inheritdoc/>
        [DataMember]
        public bool HasLoaded
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [DataMember(EmitDefaultValue = false)]
        public Dictionary<string, IConfiguration> ChildConfigurations
        {
            get;
            set;
        }


        /// <summary>
        /// Default constructor.
        /// </summary>
        public ExportStatsConfiguration()
        {
        }

        /// <inheritdoc/>
        public void StoreConfiguration(object viewModel)
        {
            if (viewModel is ExportStatsViewModel vm)
            {
                SecurityColumnNames = vm.SecurityColumnNames;
                SecuritySortingField = vm.SecuritySortingField;
                SecurityDirection = vm.SecurityDirection;
                SectorColumnNames = vm.SectorColumnNames;
                SectorSortingField = vm.SectorSortingField;
                SectorDirection = vm.SectorDirection;
                BankColumnNames = vm.BankColumnNames;
                BankSortingField = vm.BankSortingField;
                BankDirection = vm.BankDirection;
                DisplayConditions = vm.DisplayConditions;
            }
        }

        /// <inheritdoc/>
        public void RestoreFromConfiguration(object viewModel)
        {
            if (HasLoaded && viewModel is ExportStatsViewModel vm)
            {
                vm.SecurityColumnNames = SecurityColumnNames;
                vm.SecuritySortingField = SecuritySortingField;
                vm.SecurityDirection = SecurityDirection;
                vm.SectorColumnNames = SectorColumnNames;
                vm.SectorSortingField = SectorSortingField;
                vm.SectorDirection = SectorDirection;
                vm.BankColumnNames = BankColumnNames;
                vm.BankSortingField = BankSortingField;
                vm.BankDirection = BankDirection;
                vm.DisplayConditions = DisplayConditions;
            }
        }
    }
}
