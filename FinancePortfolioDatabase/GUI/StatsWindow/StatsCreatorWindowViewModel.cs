using FinanceViewModels.StatsViewModels;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.Reporting;
using FinancialStructures.StatsMakers;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UICommon.Commands;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace FinanceWindowsViewModels
{
    internal class StatsCreatorWindowViewModel : ViewModelBase<IPortfolio>
    {
        public ObservableCollection<object> StatsTabs { get; set; } = new ObservableCollection<object>();

        private bool fDisplayValueFunds = true;
        public bool DisplayValueFunds
        {
            get { return fDisplayValueFunds; }
            set { fDisplayValueFunds = value; OnPropertyChanged(); UpdateData(); }
        }

        public int HistoryGapDays { get; set; }

        private readonly IReportLogger ReportLogger;
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogCreationService;

        public StatsCreatorWindowViewModel(IPortfolio portfolio, IReportLogger reportLogger, IFileInteractionService fileService, IDialogCreationService dialogCreation)
            : base("Stats Creator", portfolio)
        {
            ReportLogger = reportLogger;
            fFileService = fileService;
            fDialogCreationService = dialogCreation;
            StatsTabs.Add(new MainTabViewModel(openTab));
            StatsTabs.Add(new SecuritiesStatisticsViewModel(portfolio, DisplayValueFunds));

            CreateCSVStatsCommand = new RelayCommand(ExecuteExportToCSVCommand);
            CreateInvestmentListCommand = new RelayCommand(ExecuteInvestmentListCommand);
            CreateHTMLCommand = new RelayCommand(ExecuteCreateHTMLCommand);
            ExportHistoryCommand = new RelayCommand(ExecuteCreateHistory);
        }

        public ICommand CreateCSVStatsCommand { get; }

        public ICommand CreateInvestmentListCommand { get; }
        public ICommand CreateHTMLCommand { get; }

        public ICommand ExportHistoryCommand { get; }

        private void ExecuteExportToCSVCommand()
        {
            Action<string> StatsOptionFeedback = (filePath) => StatsFeedback(filePath);
            var context = new StatsOptionsViewModel(DataStore, ExportType.CSV, ReportLogger, StatsOptionFeedback, fFileService, fDialogCreationService);
            fDialogCreationService.DisplayCustomDialog(context);
        }

        private void ExecuteInvestmentListCommand()
        {
            var result = fFileService.SaveFile(".csv", DataStore.DatabaseName + "-CSVStats.csv", DataStore.Directory, "CSV file|*.csv|All files|*.*");
            if (result.Success != null && (bool)result.Success)
            {
                if (!result.FilePath.EndsWith(".csv"))
                {
                    result.FilePath += ".csv";
                }

                InvestmentsExporter.Export(DataStore, result.FilePath, ReportLogger);
            }
            else
            {
                ReportLogger.LogUsefulWithStrings("Error", "StatisticsPage", $"Was not able to create Investment list page at {result.FilePath}");
            }
        }

        private void ExecuteCreateHistory()
        {
            var result = fFileService.SaveFile(".csv", DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day + "-" + DataStore.DatabaseName + "-History.csv", DataStore.Directory, "CSV file|*.csv|All files|*.*");
            if (result.Success != null && (bool)result.Success)
            {
                if (!result.FilePath.EndsWith(".csv"))
                {
                    result.FilePath += ".csv";
                }

                CSVHistoryWriter.WriteHistoryToCSV(DataStore, result.FilePath, HistoryGapDays, ReportLogger);
            }
            else
            {
                ReportLogger.LogUsefulWithStrings("Error", "StatisticsPage", $"Was not able to create Investment list page at {result.FilePath}");
            }
        }

        private void ExecuteCreateHTMLCommand()
        {
            Action<string> StatsOptionFeedback = (filePath => StatsFeedback(filePath));
            var context = new StatsOptionsViewModel(DataStore, ExportType.HTML, ReportLogger, StatsOptionFeedback, fFileService, fDialogCreationService);
            fDialogCreationService.DisplayCustomDialog(context);
        }

        private void StatsFeedback(string filePath)
        {
            LoadTab(TabType.StatsViewer, filePath);
        }

        public override void UpdateData(IPortfolio portfolio = null)
        {
            base.UpdateData(portfolio);

            foreach (var tab in StatsTabs)
            {
                if (tab is TabViewModelBase vmBase)
                {
                    vmBase.GenerateStatistics(DisplayValueFunds);
                }
            }
        }

        Action<TabType, string> openTab
        {
            get
            {
                return (tabtype, filepath) => LoadTab(tabtype, filepath);
            }
        }

        private void LoadTab(TabType tabType, string filepath)
        {
            switch (tabType)
            {
                case (TabType.Main):
                    StatsTabs.Add(new MainTabViewModel(openTab));
                    return;
                case (TabType.SecurityStats):
                    StatsTabs.Add(new SecuritiesStatisticsViewModel(DataStore, DisplayValueFunds));
                    return;
                case (TabType.SecurityInvestment):
                    StatsTabs.Add(new SecurityInvestmentViewModel(DataStore, DisplayValueFunds));
                    return;
                case (TabType.BankAccountStats):
                    StatsTabs.Add(new BankAccStatsViewModel(DataStore, DisplayValueFunds));
                    return;
                case (TabType.DatabaseStats):
                    StatsTabs.Add(new DataBaseStatsViewModel(DataStore, DisplayValueFunds));
                    return;
                case (TabType.PortfolioHistory):
                    StatsTabs.Add(new PortfolioHistoryViewModel(DataStore, DisplayValueFunds));
                    return;
                case (TabType.StatsCharts):
                    StatsTabs.Add(new StatisticsChartsViewModel(DataStore, DisplayValueFunds));
                    return;
                case (TabType.StatsViewer):
                    StatsTabs.Add(new HtmlStatsViewerViewModel(DataStore, DisplayValueFunds, filepath));
                    return;
            }
        }
    }
}
