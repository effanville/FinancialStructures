using FinancialStructures.DatabaseInterfaces;
using FinancialStructures.DisplayStructures;
using FinancialStructures.PortfolioAPI;
using System.Collections.Generic;

namespace FinanceViewModels.StatsViewModels
{
    class PortfolioHistoryViewModel : TabViewModelBase
    {

        private int fHistoryGapDays = 25;
        public int HistoryGapDays
        {
            get { return fHistoryGapDays; }
            set { fHistoryGapDays = value; OnPropertyChanged(); }
        }

        private List<HistoryStatistic> fHistoryStats;
        public List<HistoryStatistic> HistoryStats
        {
            get { return fHistoryStats; }
            set { fHistoryStats = value; OnPropertyChanged(); }
        }

        public override async void GenerateStatistics(bool displayValueFunds)
        {
            DisplayValueFunds = displayValueFunds;
            HistoryStats = await fPortfolio.GenerateHistoryStats(HistoryGapDays).ConfigureAwait(false);
        }

        public PortfolioHistoryViewModel(IPortfolio portfolio, bool displayValueFunds)
    : base(portfolio, displayValueFunds)
        {
            Header = "Portfolio History";
            GenerateStatistics(displayValueFunds);
        }
    }
}
