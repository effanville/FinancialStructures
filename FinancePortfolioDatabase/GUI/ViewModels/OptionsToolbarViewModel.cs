using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Common.Structure.Reporting;
using Common.UI;
using Common.UI.Commands;
using Common.UI.Services;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinanceWindows;
using FinancialStructures.Database;
using FinancialStructures.Database.Download;
using FinancialStructures.Database.Extensions;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    /// <summary>
    /// View model for the top toolbar.
    /// </summary>
    public class OptionsToolbarViewModel : DataDisplayViewModelBase
    {
        private string fFileName;
        private string fDirectory;
        private readonly Action<Action<IPortfolio>> DataUpdateCallback;
        private string fBaseCurrency;

        /// <summary>
        /// The base currency to display in the top dropdown.
        /// </summary>
        public string BaseCurrency
        {
            get => fBaseCurrency;
            set
            {
                SetAndNotify(ref fBaseCurrency, value, nameof(BaseCurrency));
                _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.DatabaseAccess, $"Editing BaseCurrency.");
            }
        }

        private List<string> fCurrencies;

        /// <summary>
        /// The currencies to populate the dropdown with.
        /// </summary>
        public List<string> Currencies
        {
            get => fCurrencies;
            set => SetAndNotify(ref fCurrencies, value, nameof(Currencies));
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public OptionsToolbarViewModel(UiGlobals globals, UiStyles styles, IPortfolio portfolio, Action<Action<IPortfolio>> updateData)
            : base(globals, styles, portfolio, "Options")
        {
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

        /// <inheritdoc/>
        public override void UpdateData(IPortfolio portfolio)
        {
            base.UpdateData(portfolio);
            fFileName = fUiGlobals.CurrentFileSystem.Path.GetFileName(portfolio.FilePath);
            fDirectory = portfolio.Directory(fUiGlobals.CurrentFileSystem);
            Currencies = portfolio.Names(Account.Currency).Concat(portfolio.Companies(Account.Currency)).Distinct().ToList();
            if (!Currencies.Contains(portfolio.BaseCurrency))
            {
                Currencies.Add(portfolio.BaseCurrency);
            }

            BaseCurrency = portfolio.BaseCurrency;
        }

        /// <summary>
        /// Command to open the help documentation.
        /// </summary>
        public ICommand OpenHelpCommand
        {
            get;
        }

        private void OpenHelpDocsCommand()
        {
            _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.Unknown, $"Opening help window.");
            HelpWindow helpwindow = new HelpWindow(ReportLogger);
            helpwindow.Show();
        }

        /// <summary>
        /// Command to reset database and load empty one.
        /// </summary>
        public ICommand NewDatabaseCommand
        {
            get;
        }
        private void ExecuteNewDatabase()
        {
            _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.AddingData, $"ExecuteNewDatabase called.");
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
                DataUpdateCallback(programPortfolio => programPortfolio.LoadPortfolio("", fUiGlobals.CurrentFileSystem, ReportLogger));
                fUiGlobals.CurrentWorkingDirectory = "";
            }
        }

        /// <summary>
        /// Command to save the current database to file.
        /// </summary>
        public ICommand SaveDatabaseCommand
        {
            get;
        }
        private void ExecuteSaveDatabase()
        {
            _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.Saving, $"Saving database {fFileName} called.");
            FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile("xml", fFileName, fDirectory, "XML Files|*.xml|All Files|*.*");
            if (result.Success)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.FilePath = result.FilePath);
                DataUpdateCallback(programPortfolio => programPortfolio.SavePortfolio(result.FilePath, fUiGlobals.CurrentFileSystem, ReportLogger));
                fUiGlobals.CurrentWorkingDirectory = fUiGlobals.CurrentFileSystem.Path.GetDirectoryName(result.FilePath);
            }
        }

        /// <summary>
        /// Command to open file load dialog and load database from file.
        /// </summary>
        public ICommand LoadDatabaseCommand
        {
            get;
        }
        private void ExecuteLoadDatabase()
        {
            _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.Loading, $"Loading database called.");
            FileInteractionResult result = fUiGlobals.FileInteractionService.OpenFile("xml", filter: "XML Files|*.xml|All Files|*.*");
            if (result.Success)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.Clear());
                DataUpdateCallback(programPortfolio => programPortfolio.FilePath = result.FilePath);
                DataUpdateCallback(programPortfolio => programPortfolio.LoadPortfolio(result.FilePath, fUiGlobals.CurrentFileSystem, ReportLogger));
                fUiGlobals.CurrentWorkingDirectory = fUiGlobals.CurrentFileSystem.Path.GetDirectoryName(result.FilePath);
            }
        }

        /// <summary>
        /// Command to instantiate the auto update of database values.
        /// </summary>
        public ICommand UpdateDataCommand
        {
            get;
        }
        private void ExecuteUpdateData()
        {
            _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.Downloading, $"Execute update data for  database {fFileName} called.");
            DataUpdateCallback(async programPortfolio => await PortfolioDataUpdater.Download(Account.All, programPortfolio, null, ReportLogger).ConfigureAwait(false));
        }

        /// <summary>
        /// Command to remove unnecessary data from the database.
        /// </summary>
        public ICommand CleanDataCommand
        {
            get;
        }
        private void ExecuteCleanData()
        {
            _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.EditingData, $"Execute clean database for database {fFileName} called.");
            DataUpdateCallback(programPortfolio => programPortfolio.CleanData());
        }

        /// <summary>
        /// Command to call refresh on the ui windows.
        /// </summary>
        public ICommand RefreshCommand
        {
            get;
        }

        private void ExecuteRefresh()
        {
            _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.DatabaseAccess, $"Execute refresh on the window fo database {fFileName} called.");
            DataUpdateCallback(programPortfolio => programPortfolio.OnPortfolioChanged(false, new PortfolioEventArgs(Account.All)));
        }

        /// <summary>
        /// Command to update the base currency of the database.
        /// </summary>
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
