using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FinanceWindows;
using FinancialStructures.Database;
using FinancialStructures.Database.Download;
using StructureCommon.Reporting;
using UICommon.Commands;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    public class OptionsToolbarViewModel : ViewModelBase<IPortfolio>
    {
        private string fFileName;
        private string fDirectory;
        private readonly UiGlobals fUiGlobals;
        private readonly Action<Action<IPortfolio>> DataUpdateCallback;
        private readonly IReportLogger fReportLogger;
        private string fBaseCurrency;

        public string BaseCurrency
        {
            get
            {
                return fBaseCurrency;
            }
            set
            {
                SetAndNotify(ref fBaseCurrency, value, nameof(BaseCurrency));
                _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Editing BaseCurrency.");
            }
        }

        private List<string> fCurrencies;

        public List<string> Currencies
        {
            get
            {
                return fCurrencies;
            }
            set
            {
                SetAndNotify(ref fCurrencies, value, nameof(Currencies));
            }
        }

        public OptionsToolbarViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, IReportLogger reportLogger, UiGlobals globals)
            : base("Options", portfolio)
        {
            fReportLogger = reportLogger;
            fUiGlobals = globals;
            DataUpdateCallback = updateData;
            UpdateData(portfolio);

            OpenHelpCommand = new RelayCommand(OpenHelpDocsCommand);
            NewDatabaseCommand = new RelayCommand(ExecuteNewDatabase);
            SaveDatabaseCommand = new RelayCommand(ExecuteSaveDatabase);
            LoadDatabaseCommand = new RelayCommand(ExecuteLoadDatabase);
            UpdateDataCommand = new RelayCommand(ExecuteUpdateData);
            CleanDataCommand = new RelayCommand(ExecuteCleanData);
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            CurrencyDropDownClosed = new RelayCommand(DropDownClosed);
        }


        public override void UpdateData(IPortfolio portfolio)
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.AddingData, $"Updating data in OptionsToolbarViewModel");
            base.UpdateData(portfolio);
            fFileName = portfolio.DatabaseName + portfolio.Extension;
            fDirectory = portfolio.Directory;
            Currencies = portfolio.Names(Account.Currency).Concat(portfolio.Companies(Account.Currency)).Distinct().ToList();
            if (!Currencies.Contains(portfolio.BaseCurrency))
            {
                Currencies.Add(portfolio.BaseCurrency);
            }

            BaseCurrency = portfolio.BaseCurrency;
        }

        public ICommand OpenHelpCommand
        {
            get;
        }
        private void OpenHelpDocsCommand()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.Unknown, $"Opening help window.");
            HelpWindow helpwindow = new HelpWindow(fReportLogger);
            helpwindow.Show();
        }

        public ICommand NewDatabaseCommand
        {
            get;
        }
        private void ExecuteNewDatabase()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.AddingData, $"ExecuteNewDatabase called.");
            MessageBoxResult result;
            if (DataStore.IsAlteredSinceSave)
            {
                result = fUiGlobals.DialogCreationService.ShowMessageBox("Current database has unsaved alterations. Are you sure you want to load a new database?", "New Database?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }
            else
            {
                result = fUiGlobals.DialogCreationService.ShowMessageBox("Do you want to load a new database?", "New Database?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }
            if (result == MessageBoxResult.Yes)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.FilePath = "");
                DataUpdateCallback(programPortfolio => programPortfolio.LoadPortfolio("", fUiGlobals.CurrentFileSystem, fReportLogger));
            }
        }

        public ICommand SaveDatabaseCommand
        {
            get;
        }
        private void ExecuteSaveDatabase()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.Saving, $"Saving database {fFileName} called.");
            FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile("xml", fFileName, fDirectory, "XML Files|*.xml|All Files|*.*");
            if (result.Success != null && (bool)result.Success)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.FilePath = result.FilePath);
                DataUpdateCallback(programPortfolio => programPortfolio.SavePortfolio(result.FilePath, fUiGlobals.CurrentFileSystem, fReportLogger));
            }
        }

        public ICommand LoadDatabaseCommand
        {
            get;
        }
        private void ExecuteLoadDatabase()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.Loading, $"Loading database called.");
            FileInteractionResult result = fUiGlobals.FileInteractionService.OpenFile("xml", filter: "XML Files|*.xml|All Files|*.*");
            if (result.Success != null && (bool)result.Success)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.Clear());
                DataUpdateCallback(programPortfolio => programPortfolio.FilePath = result.FilePath);
                DataUpdateCallback(programPortfolio => programPortfolio.LoadPortfolio(result.FilePath, fUiGlobals.CurrentFileSystem, fReportLogger));
            }
        }

        public ICommand UpdateDataCommand
        {
            get;
        }
        private void ExecuteUpdateData()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.Downloading, $"Execute update data for  database {fFileName} called.");
            DataUpdateCallback(async programPortfolio => await PortfolioDataUpdater.Download(Account.All, programPortfolio, null, fReportLogger).ConfigureAwait(false));
        }

        public ICommand CleanDataCommand
        {
            get;
        }
        private void ExecuteCleanData()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.EditingData, $"Execute clean database for database {fFileName} called.");
            DataUpdateCallback(programPortfolio => programPortfolio.CleanData());
        }

        public ICommand RefreshCommand
        {
            get;
        }

        private void ExecuteRefresh()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Execute refresh on the window fo database {fFileName} called.");
            DataUpdateCallback(programPortfolio => programPortfolio.OnPortfolioChanged(false, new PortfolioEventArgs(Account.All)));
        }

        public ICommand CurrencyDropDownClosed
        {
            get;
        }
        private void DropDownClosed()
        {
            DataUpdateCallback(portfolio => portfolio.BaseCurrency = BaseCurrency);
        }
    }
}
