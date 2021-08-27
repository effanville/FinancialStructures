using System.Collections.Generic;
using Common.Structure.DisplayClasses;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using FinancialStructures.DataExporters.ExportOptions;
using FinancialStructures.Statistics;

namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Configuration for the stats display
    /// </summary>
    public sealed class StatsOptionsDisplayConfiguration
    {
        /// <summary>
        /// Flag determining whether display has loaded yet.
        /// </summary>
        public bool HasLoaded
        {
            get;
            set;
        }

        public UserDisplayOptions SelectOptions
        {
            get;
            set;
        }

        public List<Selectable<string>> DisplayConditions
        {
            get;
            set;
        } = new List<Selectable<string>>();

        public Statistic SecuritySortingField
        {
            get;
            set;
        }

        public SortDirection SecurityDirection
        {
            get;
            set;
        }

        public List<Selectable<Statistic>> SecurityColumnNames
        {
            get; set;
        } = new List<Selectable<Statistic>>();

        public Statistic BankSortingField
        {
            get;
            set;
        }

        public SortDirection BankDirection
        {
            get;
            set;
        }

        public List<Selectable<Statistic>> BankColumnNames
        {
            get;
            set;
        } = new List<Selectable<Statistic>>();

        public Statistic SectorSortingField
        {
            get;
            set;
        }

        public SortDirection SectorDirection
        {
            get;
            set;
        }

        public List<Selectable<Statistic>> SectorColumnNames
        {
            get;
            set;
        } = new List<Selectable<Statistic>>();


        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatsOptionsDisplayConfiguration()
        {
        }

        internal void StoreConfiguration(StatsOptionsViewModel viewModel)
        {
            SecurityColumnNames = viewModel.SecurityColumnNames;
            SecuritySortingField = viewModel.SecuritySortingField;
            SectorColumnNames = viewModel.SectorColumnNames;
            SectorSortingField = viewModel.SectorSortingField;
            BankColumnNames = viewModel.BankColumnNames;
            BankSortingField = viewModel.BankSortingField;
            DisplayConditions = viewModel.DisplayConditions;
        }

        internal void RestoreFromConfiguration(StatsOptionsViewModel viewModel)
        {
            if (HasLoaded)
            {
                viewModel.SecurityColumnNames = SecurityColumnNames;
                viewModel.SecuritySortingField = SecuritySortingField;
                viewModel.SectorColumnNames = SectorColumnNames;
                viewModel.SectorSortingField = SectorSortingField;
                viewModel.BankColumnNames = BankColumnNames;
                viewModel.BankSortingField = BankSortingField;
                viewModel.DisplayConditions = DisplayConditions;
            }
        }
    }
}
