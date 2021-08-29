using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.Windows.Stats;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.DataExporters;
using FinancialStructures.DataStructures;
using Common.Structure.Reporting;
using Common.UI.Commands;
using Common.UI.Services;
using Common.UI;
using FinancePortfolioDatabase.GUI.Configuration;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    /// <summary>
    /// View model for the statistics display.
    /// </summary>
    public class StatsCreatorWindowViewModel : DataDisplayViewModelBase
    {
        private readonly IConfiguration fUserConfiguration;
        private readonly UiGlobals fUiGlobals;

        /// <summary>
        /// The tabs to display in the window.
        /// </summary>
        public ObservableCollection<object> StatsTabs
        {
            get;
            set;
        } = new ObservableCollection<object>();

        private bool fDisplayValueFunds = true;

        public bool DisplayValueFunds
        {
            get => fDisplayValueFunds;
            set
            {
                SetAndNotify(ref fDisplayValueFunds, value, nameof(DisplayValueFunds));
                UpdateData();
            }
        }

        public int HistoryGapDays
        {
            get;
            set;
        } = 20;

        private readonly IReportLogger ReportLogger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatsCreatorWindowViewModel(IPortfolio portfolio, IReportLogger reportLogger, UiGlobals globals, IConfiguration userConfiguration)
            : base("Stats Creator", Account.All, portfolio)
        {
            fUserConfiguration = userConfiguration;
            fUserConfiguration.HasLoaded = true;
            fUiGlobals = globals;
            ReportLogger = reportLogger;
            StatsTabs.Add(new MainTabViewModel(OpenTab));
            StatsTabs.Add(new AccountStatisticsViewModel(portfolio, Account.All, DisplayValueFunds));

            CreateInvestmentListCommand = new RelayCommand(ExecuteInvestmentListCommand);
            CreateStatsCommand = new RelayCommand(ExecuteCreateStatsCommand);
            ExportHistoryCommand = new RelayCommand(ExecuteCreateHistory);
        }

        /// <summary>
        /// Command to create a csv list of all investments.
        /// </summary>
        public ICommand CreateInvestmentListCommand
        {
            get;
        }

        /// <summary>
        /// Command to create a statistics object.
        /// </summary>
        public ICommand CreateStatsCommand
        {
            get;
        }

        public ICommand ExportHistoryCommand
        {
            get;
        }

        private void ExecuteInvestmentListCommand()
        {
            FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile(".csv", DataStore.DatabaseName(fUiGlobals.CurrentFileSystem) + "-CSVStats.csv", DataStore.Directory(fUiGlobals.CurrentFileSystem), "CSV file|*.csv|All files|*.*");
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
                _ = ReportLogger.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, $"Was not able to create Investment list page at {result.FilePath}");
            }
        }

        private async void ExecuteCreateHistory()
        {
            FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile(".csv", DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day + "-" + DataStore.DatabaseName(fUiGlobals.CurrentFileSystem) + "-History.csv", DataStore.Directory(fUiGlobals.CurrentFileSystem), "CSV file|*.csv|All files|*.*");
            if (result.Success != null && (bool)result.Success)
            {
                if (!result.FilePath.EndsWith(".csv"))
                {
                    result.FilePath += ".csv";
                }

                List<PortfolioDaySnapshot> historyStatistics = await DataStore.GenerateHistoryStats(HistoryGapDays).ConfigureAwait(false);
                CSVHistoryWriter.WriteToCSV(historyStatistics, result.FilePath, fUiGlobals.CurrentFileSystem, ReportLogger);
            }
            else
            {
                _ = ReportLogger.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, $"Was not able to create Investment list page at {result.FilePath}");
            }
        }

        private void ExecuteCreateStatsCommand()
        {
            void StatsOptionFeedback(string filePath) => LoadTab(TabType.StatsViewer, filePath);
            StatsOptionsViewModel context = new StatsOptionsViewModel(DataStore, ReportLogger, StatsOptionFeedback, fUiGlobals, fUserConfiguration.ChildConfigurations[StatsDisplayConfiguration.StatsOptions]);
            fUiGlobals.DialogCreationService.DisplayCustomDialog(context);
        }

        /// <inheritdoc/>
        public override void UpdateData(IPortfolio portfolio = null)
        {
            base.UpdateData(portfolio);

            foreach (object tab in StatsTabs)
            {
                if (tab is TabViewModelBase vmBase)
                {
                    vmBase.GenerateStatistics(DisplayValueFunds);
                }
            }
        }

        private Action<TabType, string> OpenTab => (tabtype, filepath) => LoadTab(tabtype, filepath);

        private void LoadTab(TabType tabType, string filepath)
        {
            switch (tabType)
            {
                case TabType.Main:
                    StatsTabs.Add(new MainTabViewModel(OpenTab));
                    return;
                case TabType.SecurityStats:
                    StatsTabs.Add(new AccountStatisticsViewModel(DataStore, Account.Security, DisplayValueFunds));
                    return;
                case TabType.SecurityInvestment:
                    StatsTabs.Add(new SecurityInvestmentViewModel(DataStore, DisplayValueFunds));
                    return;
                case TabType.BankAccountStats:
                    StatsTabs.Add(new AccountStatisticsViewModel(DataStore, Account.BankAccount, DisplayValueFunds));
                    return;
                case TabType.PortfolioHistory:
                    StatsTabs.Add(new PortfolioHistoryViewModel(DataStore, DisplayValueFunds));
                    return;
                case TabType.StatsCharts:
                    StatsTabs.Add(new StatisticsChartsViewModel(DataStore, DisplayValueFunds));
                    return;
                case TabType.StatsViewer:
                    StatsTabs.Add(new HtmlStatsViewerViewModel(DataStore, DisplayValueFunds, filepath));
                    return;
            }
        }
    }
}
