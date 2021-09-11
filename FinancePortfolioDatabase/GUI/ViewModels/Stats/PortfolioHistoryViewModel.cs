using System.Collections.Generic;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.DataStructures;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    internal class PortfolioHistoryViewModel : DataDisplayViewModelBase
    {
        public override bool Closable => true;

        private int fHistoryGapDays = 20;
        public int HistoryGapDays
        {
            get => fHistoryGapDays;
            set
            {
                fHistoryGapDays = value;
                OnPropertyChanged();
            }
        }

        private List<PortfolioDaySnapshot> fHistoryStats;
        public List<PortfolioDaySnapshot> HistoryStats
        {
            get => fHistoryStats;
            set
            {
                fHistoryStats = value;
                OnPropertyChanged();
            }
        }

        public override async void UpdateData(IPortfolio DataStore)
        {
            base.UpdateData(DataStore);
            HistoryStats = await DataStore.GenerateHistoryStats(HistoryGapDays).ConfigureAwait(false);
        }

        public PortfolioHistoryViewModel(IPortfolio portfolio)
            : base("History", portfolio)
        {
            UpdateData(portfolio);
        }
    }
}
