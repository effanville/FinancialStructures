﻿using System;
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

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    public class StatsCreatorWindowViewModel : DataDisplayViewModelBase
    {
        private readonly UiGlobals fUiGlobals;
        public ObservableCollection<object> StatsTabs { get; set; } = new ObservableCollection<object>();

        private bool fDisplayValueFunds = true;
        public bool DisplayValueFunds
        {
            get
            {
                return fDisplayValueFunds;
            }
            set
            {
                fDisplayValueFunds = value;
                OnPropertyChanged();
                UpdateData();
            }
        }

        public int HistoryGapDays
        {
            get; set;
        } = 20;

        private readonly IReportLogger ReportLogger;

        public StatsCreatorWindowViewModel(IPortfolio portfolio, IReportLogger reportLogger, UiGlobals globals)
            : base("Stats Creator", Account.All, portfolio)
        {
            fUiGlobals = globals;
            ReportLogger = reportLogger;
            StatsTabs.Add(new MainTabViewModel(OpenTab));
            StatsTabs.Add(new AccountStatisticsViewModel(portfolio, Account.All, DisplayValueFunds));

            CreateInvestmentListCommand = new RelayCommand(ExecuteInvestmentListCommand);
            CreateStatsCommand = new RelayCommand(ExecuteCreateStatsCommand);
            ExportHistoryCommand = new RelayCommand(ExecuteCreateHistory);
        }

        public ICommand CreateInvestmentListCommand
        {
            get;
        }
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
                CSVHistoryWriter.WriteToCSV(historyStatistics, result.FilePath, ReportLogger);
            }
            else
            {
                _ = ReportLogger.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, $"Was not able to create Investment list page at {result.FilePath}");
            }
        }

        private void ExecuteCreateStatsCommand()
        {
            Action<string> StatsOptionFeedback = (filePath => StatsFeedback(filePath));
            StatsOptionsViewModel context = new StatsOptionsViewModel(DataStore, ReportLogger, StatsOptionFeedback, fUiGlobals);
            fUiGlobals.DialogCreationService.DisplayCustomDialog(context);
        }

        private void StatsFeedback(string filePath)
        {
            LoadTab(TabType.StatsViewer, filePath);
        }

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

        private Action<TabType, string> OpenTab
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
                    StatsTabs.Add(new MainTabViewModel(OpenTab));
                    return;
                case (TabType.SecurityStats):
                    StatsTabs.Add(new AccountStatisticsViewModel(DataStore, Account.Security, DisplayValueFunds));
                    return;
                case (TabType.SecurityInvestment):
                    StatsTabs.Add(new SecurityInvestmentViewModel(DataStore, DisplayValueFunds));
                    return;
                case (TabType.BankAccountStats):
                    StatsTabs.Add(new AccountStatisticsViewModel(DataStore, Account.BankAccount, DisplayValueFunds));
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
