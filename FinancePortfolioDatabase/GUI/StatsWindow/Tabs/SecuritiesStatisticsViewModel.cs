using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.StatisticStructures;
using System.Collections.Generic;

namespace FinanceViewModels.StatsViewModels
{
    internal class SecuritiesStatisticsViewModel : TabViewModelBase
    {
        private List<SecurityStatistics> fSecuritiesStats;

        public List<SecurityStatistics> SecuritiesStats
        {
            get { return fSecuritiesStats; }
            set { fSecuritiesStats = value; OnPropertyChanged(); }
        }

        public override void GenerateStatistics(bool displayValueFunds)
        {
            DisplayValueFunds = displayValueFunds;
            SecuritiesStats = fPortfolio.GenerateSecurityStatistics(DisplayValueFunds);
        }

        public SecuritiesStatisticsViewModel(IPortfolio portfolio, bool displayValueFunds)
            : base(portfolio, displayValueFunds)
        {
            Header = "Security Statistics";
            GenerateStatistics(displayValueFunds);
        }
    }
}
