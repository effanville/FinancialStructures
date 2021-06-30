using System;
using System.Windows.Input;
using FinancePortfolioDatabase.GUI.Windows.Stats;
using Common.UI.Commands;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    internal class MainTabViewModel : TabViewModelBase
    {
        public override bool Closable
        {
            get
            {
                return false;
            }
        }

        private readonly Action<TabType, string> OpenTab;

        public ICommand OpenSecuritiesTab
        {
            get; private set;
        }
        public ICommand OpenSecInvTab
        {
            get;
        }
        public ICommand OpenBankAccTab
        {
            get;
        }
        public ICommand OpenHistoryTab
        {
            get;
        }
        public ICommand OpenChartsTab
        {
            get;
        }
        public ICommand OpenHtmlViewerTab
        {
            get;
        }

        public MainTabViewModel(Action<TabType, string> openTab)
            : base(null, false)
        {
            Header = "Statistics Selection";
            OpenTab = openTab;
            OpenSecuritiesTab = new RelayCommand(ExecuteSecuritiesTab);
            OpenSecInvTab = new RelayCommand(ExecuteSecInvTab);
            OpenBankAccTab = new RelayCommand(ExecuteBankAccTab);
            OpenHistoryTab = new RelayCommand(ExecuteHistoryTab);
            OpenChartsTab = new RelayCommand(ExecuteChartsTab);
            OpenHtmlViewerTab = new RelayCommand(ExecuteHtmlViewerTab);
        }

        private void ExecuteHtmlViewerTab()
        {
            OpenTab(TabType.StatsViewer, null);
        }

        private void ExecuteChartsTab()
        {
            OpenTab(TabType.StatsCharts, null);
        }

        private void ExecuteHistoryTab()
        {
            OpenTab(TabType.PortfolioHistory, null);
        }

        private void ExecuteBankAccTab()
        {
            OpenTab(TabType.BankAccountStats, null);
        }

        private void ExecuteSecInvTab()
        {
            OpenTab(TabType.SecurityInvestment, null);
        }

        private void ExecuteSecuritiesTab()
        {
            OpenTab(TabType.SecurityStats, null);
        }
    }
}
