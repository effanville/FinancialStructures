using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.StatisticStructures;

namespace FinanceViewModels.StatsViewModels
{
    internal class DataBaseStatsViewModel : TabViewModelBase
    {

        private List<DatabaseStatistics> fDatabaseStats;
        public List<DatabaseStatistics> DatabaseStats
        {
            get
            {
                return fDatabaseStats;
            }
            set
            {
                fDatabaseStats = value;
                OnPropertyChanged();
            }
        }

        public override void GenerateStatistics(bool displayValueFunds)
        {
            DisplayValueFunds = displayValueFunds;
            DatabaseStats = fPortfolio.GenerateDatabaseStatistics();
        }
        public DataBaseStatsViewModel(IPortfolio portfolio, bool displayValueFunds)
    : base(portfolio, displayValueFunds)
        {
            Header = "Database Statistics";
            GenerateStatistics(displayValueFunds);
        }
    }
}
