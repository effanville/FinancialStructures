using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.DataStructures;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    internal class PortfolioHistoryViewModel : TabViewModelBase
    {

        private int fHistoryGapDays = 20;
        public int HistoryGapDays
        {
            get
            {
                return fHistoryGapDays;
            }
            set
            {
                fHistoryGapDays = value;
                OnPropertyChanged();
            }
        }

        private List<PortfolioDaySnapshot> fHistoryStats;
        public List<PortfolioDaySnapshot> HistoryStats
        {
            get
            {
                return fHistoryStats;
            }
            set
            {
                fHistoryStats = value;
                OnPropertyChanged();
            }
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
