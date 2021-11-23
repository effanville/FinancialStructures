using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Common.Structure.DisplayClasses;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using FinancialStructures.Database.Statistics;

namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Configuration for the stats display
    /// </summary>
    [DataContract]
    public sealed class StatsDisplayConfiguration : IConfiguration
    {
        [DataMember]
        internal bool DisplayValueFunds;
        [DataMember(EmitDefaultValue = false)]
        internal List<Selectable<Statistic>> StatisticNames;

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
                StatisticNames = vm.StatisticNames;
            }
        }

        /// <inheritdoc/>
        public void RestoreFromConfiguration(object viewModel)
        {
            if (viewModel is StatsViewModel vm)
            {
                vm.DisplayValueFunds = DisplayValueFunds;
                if (StatisticNames != null && StatisticNames.Any())
                {
                    vm.StatisticNames = StatisticNames;
                }
            }
        }
    }
}
