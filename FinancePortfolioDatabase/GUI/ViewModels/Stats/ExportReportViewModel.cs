using System;
using System.Windows.Input;
using Common.Structure.FileAccess;
using Common.Structure.Reporting;
using Common.UI;
using Common.UI.Commands;
using Common.UI.Services;
using FinancePortfolioDatabase.GUI.Configuration;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.Database.Export.Report;
using FinancialStructures.Database.Extensions;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    /// <summary>
    /// Export to report options and routine.
    /// </summary>
    public sealed class ExportReportViewModel : DataDisplayViewModelBase
    {
        private readonly Action<object> fCloseWindowAction;
        private bool fDisplayValueFunds;

        /// <summary>
        /// The number of days to have between history stats.
        /// </summary>
        public bool DisplayValueFunds
        {
            get => fDisplayValueFunds;
            set => SetAndNotify(ref fDisplayValueFunds, value, nameof(DisplayValueFunds));
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ExportReportViewModel(UiGlobals globals, UiStyles styles, IConfiguration userConfiguration, IPortfolio portfolio, Action<object> CloseWindow)
            : base(globals, styles, userConfiguration, portfolio, "", Account.All)
        {
            fCloseWindowAction = CloseWindow;
            if (fUserConfiguration.HasLoaded)
            {
                fUserConfiguration.RestoreFromConfiguration(this);
            }
            else
            {
                DisplayValueFunds = true;
                fUserConfiguration.StoreConfiguration(this);
                fUserConfiguration.HasLoaded = true;
            }

            ExportReportCommand = new RelayCommand(ExecuteCreateReport);
        }

        /// <summary>
        /// Command for exporting history data.
        /// </summary>
        public ICommand ExportReportCommand
        {
            get;
        }

        private void ExecuteCreateReport()
        {
            fUserConfiguration.StoreConfiguration(this);
            FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile(ExportType.Html.ToString().ToLower(), $"{DataStore.DatabaseName(fUiGlobals.CurrentFileSystem)}-report.html", DataStore.Directory(fUiGlobals.CurrentFileSystem), "Html file|*.html|All files|*.*");
            if (result.Success)
            {
                if (!result.FilePath.EndsWith(".html"))
                {
                    result.FilePath += ".html";
                }
                PortfolioReport portfolioInvestments = new PortfolioReport(DataStore, PortfolioReportSettings.DefaultSettings());
                portfolioInvestments.ExportToFile(fUiGlobals.CurrentFileSystem, result.FilePath, PortfolioReportExportSettings.DefaultSettings(), ReportLogger);
                fCloseWindowAction(new HtmlStatsViewerViewModel(Styles, fUiGlobals, result.FilePath));
            }
            else
            {
                _ = fUiGlobals.ReportLogger.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, $"Was not able to create Investment list page at {result.FilePath}");
            }
        }
    }
}
