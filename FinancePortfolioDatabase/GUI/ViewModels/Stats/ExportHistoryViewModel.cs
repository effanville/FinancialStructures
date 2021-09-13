using System;
using System.Collections.Generic;
using System.Windows.Input;
using Common.Structure.Reporting;
using Common.UI;
using Common.UI.Commands;
using Common.UI.Services;
using FinancePortfolioDatabase.GUI.Configuration;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.DataExporters;
using FinancialStructures.DataStructures;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    /// <summary>
    /// View model for the options for exporting the portfolio history.
    /// </summary>
    public sealed class ExportHistoryViewModel : DataDisplayViewModelBase
    {
        private readonly IConfiguration fUserConfiguration;
        private IReportLogger ReportLogger => fUiGlobals.ReportLogger;
        private readonly Action<object> fCloseWindowAction;
        private readonly UiGlobals fUiGlobals;

        private int fHistoryGapDays;

        /// <summary>
        /// The number of days to have between history stats.
        /// </summary>
        public int HistoryGapDays
        {
            get => fHistoryGapDays;
            set => SetAndNotify(ref fHistoryGapDays, value, nameof(HistoryGapDays));
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ExportHistoryViewModel(IPortfolio portfolio, Action<object> CloseWindow, UiStyles styles, UiGlobals globals, IConfiguration userConfiguration)
            : base(styles, "", Account.All, portfolio)
        {
            fUiGlobals = globals;
            fCloseWindowAction = CloseWindow;
            fUserConfiguration = userConfiguration;
            if (fUserConfiguration.HasLoaded)
            {
                fUserConfiguration.RestoreFromConfiguration(this);
            }
            else
            {
                HistoryGapDays = 20;
                fUserConfiguration.HasLoaded = true;
            }

            ExportHistoryCommand = new RelayCommand(ExecuteCreateHistory);
        }

        /// <summary>
        /// Command for exporting history data.
        /// </summary>
        public ICommand ExportHistoryCommand
        {
            get;
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
                fCloseWindowAction(new PortfolioHistoryViewModel(DataStore, Styles));
            }
            else
            {
                _ = ReportLogger.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, $"Was not able to create Investment list page at {result.FilePath}");
            }
        }
    }
}
