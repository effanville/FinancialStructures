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
        private readonly Action<Action<IPortfolio>> DataUpdateCallback;
        private readonly IReportLogger fReportLogger;
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogCreationService;
        private string fBaseCurrency;

        public string BaseCurrency
        {
            get
            {
                return fBaseCurrency;
            }
            set
            {
                if (fBaseCurrency != value)
                {
                    fBaseCurrency = value;
                    OnPropertyChanged(nameof(BaseCurrency));
                }
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
                fCurrencies = value;
                OnPropertyChanged();
            }
        }

        public OptionsToolbarViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, IReportLogger reportLogger, IFileInteractionService fileService, IDialogCreationService dialogCreation)
            : base("Options", portfolio)
        {
            fReportLogger = reportLogger;
            fFileService = fileService;
            fDialogCreationService = dialogCreation;
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
            base.UpdateData(portfolio);
            fFileName = portfolio.DatabaseName + portfolio.Extension;
            fDirectory = portfolio.Directory;
            Currencies = portfolio.Names(Account.Currency).Concat(portfolio.Companies(Account.Currency)).Distinct().ToList();
            if (!Currencies.Contains(portfolio.BaseCurrency))
            {
                Currencies.Add(portfolio.BaseCurrency);
            }

            // We have just updated the portfolio, so shouldnt be setting BaseCurrency here.
            BaseCurrency = portfolio.BaseCurrency;
        }

        public ICommand OpenHelpCommand
        {
            get;
        }
        private void OpenHelpDocsCommand()
        {
            HelpWindow helpwindow = new HelpWindow(fReportLogger);
            helpwindow.Show();
        }

        public ICommand NewDatabaseCommand
        {
            get;
        }
        private void ExecuteNewDatabase()
        {
            MessageBoxResult result;
            if (DataStore.IsAlteredSinceSave)
            {
                result = fDialogCreationService.ShowMessageBox("Current database has unsaved alterations. Are you sure you want to load a new database?", "New Database?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }
            else
            {
                result = fDialogCreationService.ShowMessageBox("Do you want to load a new database?", "New Database?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }
            if (result == MessageBoxResult.Yes)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.SetFilePath(""));
                DataUpdateCallback(programPortfolio => programPortfolio.LoadPortfolio("", fReportLogger));
            }
        }

        public ICommand SaveDatabaseCommand
        {
            get;
        }
        private void ExecuteSaveDatabase()
        {
            FileInteractionResult result = fFileService.SaveFile("xml", fFileName, fDirectory, "XML Files|*.xml|All Files|*.*");
            if (result.Success != null && (bool)result.Success)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.SetFilePath(result.FilePath));
                DataUpdateCallback(programPortfolio => programPortfolio.SavePortfolio(result.FilePath, fReportLogger));
            }
        }

        public ICommand LoadDatabaseCommand
        {
            get;
        }
        private void ExecuteLoadDatabase()
        {
            FileInteractionResult result = fFileService.OpenFile("xml", filter: "XML Files|*.xml|All Files|*.*");
            if (result.Success != null && (bool)result.Success)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.Clear());
                DataUpdateCallback(programPortfolio => programPortfolio.SetFilePath(result.FilePath));
                DataUpdateCallback(programPortfolio => programPortfolio.LoadPortfolio(result.FilePath, fReportLogger));
            }
        }

        public ICommand UpdateDataCommand
        {
            get;
        }
        private void ExecuteUpdateData()
        {
            DataUpdateCallback(async programPortfolio => await PortfolioDataUpdater.Download(Account.All, programPortfolio, null, fReportLogger).ConfigureAwait(false));
        }

        public ICommand CleanDataCommand
        {
            get;
        }
        private void ExecuteCleanData()
        {
            DataUpdateCallback(programPortfolio => programPortfolio.CleanData());
        }

        public ICommand RefreshCommand
        {
            get;
        }

        private void ExecuteRefresh()
        {
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
