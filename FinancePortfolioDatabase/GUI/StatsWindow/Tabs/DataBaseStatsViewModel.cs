using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.PortfolioAPI;
using System.Collections.Generic;

namespace FinanceViewModels.StatsViewModels
{
    internal class DataBaseStatsViewModel : TabViewModelBase
    {

        private List<DatabaseStatistics> fDatabaseStats;
        public List<DatabaseStatistics> DatabaseStats
        {
            get { return fDatabaseStats; }
            set { fDatabaseStats = value; OnPropertyChanged(); }
        }

        public override void GenerateStatistics(bool displayValueFunds)
        {
            DisplayValueFunds = displayValueFunds;
            DatabaseStats = fPortfolio.GenerateDatabaseStatistics();
        }
        public DataBaseStatsViewModel(Portfolio portfolio, bool displayValueFunds)
    : base(portfolio, displayValueFunds)
        {
            Header = "Database Statistics";
            GenerateStatistics(displayValueFunds);
        }
    }
}
