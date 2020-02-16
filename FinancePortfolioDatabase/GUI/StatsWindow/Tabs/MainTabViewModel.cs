using FinanceWindowsViewModels;
using GUISupport;
using System;
using System.Windows.Input;

namespace FinanceViewModels.StatsViewModels
{
    internal class MainTabViewModel : TabViewModelBase
    {
        public override bool Closable { get { return false; } }
        Action<TabType, string> OpenTab;

        public ICommand OpenSecuritiesTab { get; private set; }
        public ICommand OpenSecInvTab { get; }
        public ICommand OpenDatabaseTab { get; }
        public ICommand OpenBankAccTab { get; }
        public ICommand OpenHistoryTab { get; }
        public ICommand OpenChartsTab { get; }
        public ICommand OpenHtmlViewerTab { get; }

        public MainTabViewModel(Action<TabType, string> openTab)
            : base(null, false)
        {
            Header = "Statistics Selection";
            OpenTab = openTab;
            OpenSecuritiesTab = new BasicCommand(ExecuteSecuritiesTab);
            OpenSecInvTab = new BasicCommand(ExecuteSecInvTab);
            OpenDatabaseTab = new BasicCommand(ExecuteDatabaseTab);
            OpenBankAccTab = new BasicCommand(ExecuteBankAccTab);
            OpenHistoryTab = new BasicCommand(ExecuteHistoryTab);
            OpenChartsTab = new BasicCommand(ExecuteChartsTab);
            OpenHtmlViewerTab = new BasicCommand(ExecuteHtmlViewerTab);
        }

        private void ExecuteHtmlViewerTab(object obj)
        {
            OpenTab(TabType.StatsViewer, null);
        }

        private void ExecuteChartsTab(object obj)
        {
            OpenTab(TabType.StatsCharts, null);
        }

        private void ExecuteHistoryTab(object obj)
        {
            OpenTab(TabType.PortfolioHistory, null);
        }

        private void ExecuteBankAccTab(object obj)
        {
            OpenTab(TabType.BankAccountStats, null);
        }

        private void ExecuteSecInvTab(object obj)
        {
            OpenTab(TabType.SecurityInvestment, null);
        }

        private void ExecuteSecuritiesTab(object obj)
        {
            OpenTab(TabType.SecurityStats, null);
        }

        private void ExecuteDatabaseTab(object obj)
        {
            OpenTab(TabType.DatabaseStats, null);
        }
    }
}
