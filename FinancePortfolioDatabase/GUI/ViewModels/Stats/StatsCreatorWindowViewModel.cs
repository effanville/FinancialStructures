using System;
using System.Collections.Generic;
using System.Windows.Input;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
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

        private StatsOptionsViewModel fDisplay;
        public StatsOptionsViewModel Display
        {
            get => fDisplay;
            set => SetAndNotify(ref fDisplay, value, nameof(Display));
        }

        public int HistoryGapDays
        {
            get;
            set;
        } = 20;

        private readonly IReportLogger ReportLogger;
        private readonly Action<object> fLoadTab;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatsCreatorWindowViewModel(IPortfolio portfolio, IReportLogger reportLogger, UiGlobals globals, IConfiguration userConfiguration, Action<object> loadTab)
            : base("Stats Creator", Account.All, portfolio)
        {
            fUserConfiguration = userConfiguration;
            if (fUserConfiguration.HasLoaded)
            {
                fUserConfiguration.RestoreFromConfiguration(this);
            }

            fLoadTab = loadTab;
            fUiGlobals = globals;
            ReportLogger = reportLogger;

            Display = new StatsOptionsViewModel(DataStore, ReportLogger, obj => fLoadTab(obj), fUiGlobals, fUserConfiguration);

            CreateInvestmentListCommand = new RelayCommand(ExecuteInvestmentListCommand);
            ExportHistoryCommand = new RelayCommand(ExecuteCreateHistory);
        }

        /// <summary>
        /// Command to create a csv list of all investments.
        /// </summary>
        public ICommand CreateInvestmentListCommand
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
                fLoadTab(new SecurityInvestmentViewModel(DataStore));
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
                fLoadTab(new PortfolioHistoryViewModel(DataStore));
            }
            else
            {
                _ = ReportLogger.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, $"Was not able to create Investment list page at {result.FilePath}");
            }
        }

        /// <inheritdoc/>
        public override void UpdateData(IPortfolio portfolio = null)
        {
            base.UpdateData(portfolio);
            Display.UpdateData(portfolio);

            fUserConfiguration.StoreConfiguration(this);
        }
    }
}
