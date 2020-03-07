using FinanceCommonViewModels;
using FinanceViewModels.StatsViewModels;
using FinanceWindows.StatsWindows;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.StatsMakers;
using GUISupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class StatsCreatorWindowViewModel : ViewModelBase
    {
        private Portfolio fPortfolio;
        private List<Sector> Sectors;

        public ObservableCollection<object> StatsTabs { get; set; } = new ObservableCollection<object>();

        private bool fDisplayValueFunds = true;
        public bool DisplayValueFunds
        {
            get { return fDisplayValueFunds; }
            set { fDisplayValueFunds = value; OnPropertyChanged(); UpdateData(); }
        }

        public int HistoryGapDays { get; set; }

        Action<string, string, string> ReportLogger;

        public StatsCreatorWindowViewModel(Portfolio portfolio, List<Sector> sectors, Action<string, string, string> reportLogger)
            : base("Stats Creator")
        {
            StatsTabs.Add(new MainTabViewModel(openTab));
            StatsTabs.Add(new SecuritiesStatisticsViewModel(portfolio, DisplayValueFunds));

            if (portfolio != null)
            {
                fPortfolio = portfolio;
            }
            if (sectors != null)
            {
                Sectors = sectors;
            }

            ReportLogger = reportLogger;
            CreateCSVStatsCommand = new BasicCommand(ExecuteExportToCSVCommand);
            CreateInvestmentListCommand = new BasicCommand(ExecuteInvestmentListCommand);
            CreateHTMLCommand = new BasicCommand(ExecuteCreateHTMLCommand);
            ExportHistoryCommand = new BasicCommand(ExecuteCreateHistory);
        }

        public ICommand CreateCSVStatsCommand { get; }

        public ICommand CreateInvestmentListCommand { get; }
        public ICommand CreateHTMLCommand { get; }

        public ICommand ExportHistoryCommand { get; }

        private void ExecuteExportToCSVCommand(Object obj)
        {
            var optionWindow = new StatsOptionsWindow();
            Action<string> StatsOptionFeedback = (filePath) => StatsFeedback(optionWindow, filePath);
            var context = new StatsOptionsViewModel(fPortfolio, Sectors, ExportType.CSV, ReportLogger, StatsOptionFeedback);
            optionWindow.DataContext = context;
            optionWindow.ShowDialog();
        }

        private void ExecuteInvestmentListCommand(Object obj)
        {
            SaveFileDialog saving = new SaveFileDialog() { DefaultExt = ".csv", FileName = fPortfolio.DatabaseName + "-CSVStats.csv", InitialDirectory = fPortfolio.Directory };
            saving.Filter = "CSV file|*.csv|All files|*.*";
            if (saving.ShowDialog() == DialogResult.OK)
            {
                if (!saving.FileName.EndsWith(".csv"))
                {
                    saving.FileName += ".csv";
                }

                InvestmentsExporter.Export(fPortfolio, saving.FileName, ReportLogger);
            }
            else
            {
                ReportLogger("Error", "StatisticsPage", $"Was not able to create Investment list page at {saving.FileName}");
            }
            saving.Dispose();
        }

        private void ExecuteCreateHistory(Object obj)
        {
            SaveFileDialog saving = new SaveFileDialog() { DefaultExt = ".csv", FileName = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day + "-" + fPortfolio.DatabaseName + "-History.csv", InitialDirectory = fPortfolio.Directory };
            saving.Filter = "CSV file|*.csv|All files|*.*";
            if (saving.ShowDialog() == DialogResult.OK)
            {
                if (!saving.FileName.EndsWith(".csv"))
                {
                    saving.FileName += ".csv";
                }

                CSVHistoryWriter.WriteHistoryToCSV(fPortfolio, ReportLogger, saving.FileName, HistoryGapDays);
            }
            else
            {
                ReportLogger("Error", "StatisticsPage", $"Was not able to create Investment list page at {saving.FileName}");
            }
            saving.Dispose();
        }

        private void ExecuteCreateHTMLCommand(Object obj)
        {
            var optionWindow = new StatsOptionsWindow();
            Action<string> StatsOptionFeedback = (filePath) => StatsFeedback(optionWindow, filePath);
            var context = new StatsOptionsViewModel(fPortfolio, Sectors, ExportType.HTML, ReportLogger, StatsOptionFeedback);
            optionWindow.DataContext = context;
            optionWindow.ShowDialog();
        }

        private void StatsFeedback(StatsOptionsWindow window, string filePath)
        {
            window.Close();
            LoadTab(TabType.StatsViewer, filePath);
        }

        public override void UpdateData(Portfolio portfolio = null, List<Sector> sectors = null)
        {
            foreach (var tab in StatsTabs)
            {
                if (tab is TabViewModelBase vmBase)
                {
                    vmBase.GenerateStatistics(DisplayValueFunds);
                }
            }
        }

        Action<TabType, string> openTab => (tabtype, filepath) => LoadTab(tabtype, filepath);

        private void LoadTab(TabType tabType, string filepath)
        {
            switch (tabType)
            {
                case (TabType.Main):
                    StatsTabs.Add(new MainTabViewModel(openTab));
                    return;
                case (TabType.SecurityStats):
                    StatsTabs.Add(new SecuritiesStatisticsViewModel(fPortfolio, DisplayValueFunds));
                    return;
                case (TabType.SecurityInvestment):
                    StatsTabs.Add(new SecurityInvestmentViewModel(fPortfolio, DisplayValueFunds));
                    return;
                case (TabType.BankAccountStats):
                    StatsTabs.Add(new BankAccStatsViewModel(fPortfolio, DisplayValueFunds));
                    return;
                case (TabType.DatabaseStats):
                    StatsTabs.Add(new DataBaseStatsViewModel(fPortfolio, DisplayValueFunds));
                    return;
                case (TabType.PortfolioHistory):
                    StatsTabs.Add(new PortfolioHistoryViewModel(fPortfolio, DisplayValueFunds));
                    return;
                case (TabType.StatsCharts):
                    StatsTabs.Add(new StatisticsChartsViewModel(fPortfolio, DisplayValueFunds));
                    return;
                case (TabType.StatsViewer):
                    StatsTabs.Add(new HtmlStatsViewerViewModel(fPortfolio, DisplayValueFunds, filepath));
                    return;
            }
        }
    }
}
