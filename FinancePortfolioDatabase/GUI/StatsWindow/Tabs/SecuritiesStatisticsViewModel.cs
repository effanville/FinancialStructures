using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using GUISupport;
using System.Collections.Generic;

namespace FinanceViewModels.StatsViewModels
{
    internal class SecuritiesStatisticsViewModel : TabViewModelBase
    {
        private List<SecurityStatsHolder> fSecuritiesStats;

        public List<SecurityStatsHolder> SecuritiesStats
        {
            get { return fSecuritiesStats; }
            set { fSecuritiesStats = value; OnPropertyChanged(); }
        }

        public override void GenerateStatistics(bool displayValueFunds)
        {
            DisplayValueFunds = displayValueFunds;
            SecuritiesStats = fPortfolio.GenerateSecurityStatistics(DisplayValueFunds);
        }

        public SecuritiesStatisticsViewModel(Portfolio portfolio, bool displayValueFunds)
            :base(portfolio,displayValueFunds)
        {
            Header = "Security Statistics";
            GenerateStatistics(displayValueFunds);
        }
    }
}
