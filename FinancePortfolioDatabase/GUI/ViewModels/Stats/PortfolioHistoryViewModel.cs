using System.Collections.Generic;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.DataExporters.History;

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

        public override void UpdateData(IPortfolio DataStore)
        {
            base.UpdateData(DataStore);
            var history = new PortfolioHistory(DataStore, new PortfolioHistorySettings(HistoryGapDays, false, false));
            HistoryStats = history.Snapshots;
        }

        public PortfolioHistoryViewModel(IPortfolio portfolio, UiStyles styles)
            : base(null, styles, portfolio, "History")
        {
            UpdateData(portfolio);
        }
    }
}
