using FinanceCommonViewModels;
using FinanceWindows;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Reporting;
using GUISupport;
using GUISupport.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class OptionsToolbarViewModel : ViewModelBase
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
            get { return fBaseCurrency; }
            set
            {
                if (fBaseCurrency != value)
                {
                    fBaseCurrency = value;
                    OnPropertyChanged(nameof(BaseCurrency));
                    DataUpdateCallback(portfolio => portfolio.BaseCurrency = BaseCurrency);
                }
            }
        }

        private List<string> fCurrencies;

        public List<string> Currencies
        {
            get { return fCurrencies; }
            set { fCurrencies = value; OnPropertyChanged(); }
        }

        public OptionsToolbarViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, IReportLogger reportLogger, IFileInteractionService fileService, IDialogCreationService dialogCreation)
            : base("Options")
        {
            fReportLogger = reportLogger;
            fFileService = fileService;
            fDialogCreationService = dialogCreation;
            DataUpdateCallback = updateData;
            UpdateData(portfolio);

            OpenHelpCommand = new BasicCommand(OpenHelpDocsCommand);
            NewDatabaseCommand = new BasicCommand(ExecuteNewDatabase);
            SaveDatabaseCommand = new BasicCommand(ExecuteSaveDatabase);
            LoadDatabaseCommand = new BasicCommand(ExecuteLoadDatabase);
            UpdateDataCommand = new BasicCommand(ExecuteUpdateData);
            RefreshCommand = new BasicCommand(ExecuteRefresh);
        }


        public override void UpdateData(IPortfolio portfolio)
        {
            fFileName = portfolio.DatabaseName + portfolio.Extension;
            fDirectory = portfolio.Directory;
            Currencies = portfolio.Names(AccountType.Currency).Concat(portfolio.Companies(AccountType.Currency)).Distinct().ToList();
            if (!Currencies.Contains(portfolio.BaseCurrency))
            {
                Currencies.Add(portfolio.BaseCurrency);
            }
            BaseCurrency = portfolio.BaseCurrency;
        }

        public ICommand OpenHelpCommand { get; }
        private void OpenHelpDocsCommand(Object obj)
        {
            var helpwindow = new HelpWindow(fReportLogger);
            helpwindow.Show();
        }

        public ICommand NewDatabaseCommand { get; }
        private void ExecuteNewDatabase(Object obj)
        {
            MessageBoxResult result = fDialogCreationService.ShowMessageBox("Do you want to load a new database?", "New Database?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.SetFilePath(""));
                DataUpdateCallback(programPortfolio => programPortfolio.LoadPortfolio("", fReportLogger));
            }
        }

        public ICommand SaveDatabaseCommand { get; }
        private void ExecuteSaveDatabase(Object obj)
        {
            var result = fFileService.SaveFile("xml", fFileName, fDirectory, "XML Files|*.xml|All Files|*.*");
            if (result.Success != null && (bool)result.Success)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.SetFilePath(result.FilePath));
                DataUpdateCallback(programPortfolio => programPortfolio.SavePortfolio(result.FilePath, fReportLogger));
            }
        }

        public ICommand LoadDatabaseCommand { get; }
        private void ExecuteLoadDatabase(Object obj)
        {
            var result = fFileService.OpenFile("xml", filter: "XML Files|*.xml|All Files|*.*");
            if (result.Success != null && (bool)result.Success)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.SetFilePath(result.FilePath));
                DataUpdateCallback(programPortfolio => programPortfolio.LoadPortfolio(result.FilePath, fReportLogger));
            }
        }

        public ICommand UpdateDataCommand { get; }
        private void ExecuteUpdateData(Object obj)
        {
            DataUpdateCallback(async programPortfolio => await PortfolioDataUpdater.Downloader(programPortfolio, fReportLogger).ConfigureAwait(false));
        }

        public ICommand RefreshCommand { get; }

        private void ExecuteRefresh(Object obj)
        {
            DataUpdateCallback(programPortfolio => programPortfolio.SetFilePath(fFileName));
        }
    }
}
