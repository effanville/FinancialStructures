using FinanceCommonViewModels;
using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.ReportLogging;
using GUISupport;
using System;
using System.Collections.Generic;

namespace FinanceWindowsViewModels
{
    internal class MainWindowViewModel : PropertyChangedBase
    {
        public EditMethods bankAccEditMethods = new EditMethods(
              (portfolio, name, reportUpdate) => PortfolioDataUpdater.DownloadBankAccount(portfolio, name, reportUpdate),
              (portfolio) => portfolio.NameData(AccountType.BankAccount, null),
              (portfolio, name, reports) => portfolio.TryAdd(AccountType.BankAccount, name, reports),
              (portfolio, oldName, newName, reports) => portfolio.TryEditName(AccountType.BankAccount, oldName, newName, reports),
              (portfolio, name, reports) => portfolio.TryRemove(AccountType.BankAccount, name, reports),
              (portfolio, name, reports) => portfolio.NumberData(AccountType.BankAccount, name, reports),
              (portfolio, name, data, reports) => portfolio.TryAddData(AccountType.BankAccount, name, data, reports),
              (portfolio, name, oldData, newData, reports) => portfolio.TryEditData(AccountType.BankAccount, name, oldData, newData, reports),
              (portfolio, name, data, reports) => portfolio.TryDeleteData(AccountType.BankAccount, name, data, reports));

        public EditMethods sectorEditMethods = new EditMethods(
                (portfolio, name, reportUpdate) => PortfolioDataUpdater.DownloadSector(portfolio, name, reportUpdate),
                (portfolio) => portfolio.NameData(AccountType.Sector, null),
                (portfolio, name, reports) => portfolio.TryAdd(AccountType.Sector, name, reports),
                (portfolio, oldName, newName, reports) => portfolio.TryEditName(AccountType.Sector, oldName, newName, reports),
                (portfolio, name, reports) => portfolio.TryRemove(AccountType.Sector, name, reports),
                (portfolio, name, reports) => portfolio.NumberData(AccountType.Sector, name, reports),
                (portfolio, name, data, reports) => portfolio.TryAddData(AccountType.Sector, name, data, reports),
                (portfolio, name, oldData, newData, reports) => portfolio.TryEditData(AccountType.Sector, name, oldData, newData, reports),
                (portfolio, name, data, reports) => portfolio.TryDeleteData(AccountType.Sector, name, data, reports));

        public EditMethods currencyEditMethods = new EditMethods(
                (portfolio, name, reportUpdate) => PortfolioDataUpdater.DownloadCurrency(portfolio, name, reportUpdate),
                (portfolio) => portfolio.NameData(AccountType.Currency, null),
                (portfolio, name, reports) => portfolio.TryAdd(AccountType.Currency, name, reports),
                (portfolio, oldName, newName, reports) => portfolio.TryEditName(AccountType.Currency, oldName, newName, reports),
                (portfolio, name, reports) => portfolio.TryRemove(AccountType.Currency, name, reports),
                (portfolio, name, reports) => portfolio.NumberData(AccountType.Currency, name, reports),
                (portfolio, name, data, reports) => portfolio.TryAddData(AccountType.Currency, name, data, reports),
                (portfolio, name, oldData, newData, reports) => portfolio.TryEditData(AccountType.Currency, name, oldData, newData, reports),
                (portfolio, name, data, reports) => portfolio.TryDeleteData(AccountType.Currency, name, data, reports));

        internal Portfolio ProgramPortfolio = new Portfolio();

        private OptionsToolbarViewModel fOptionsToolbarCommands;

        public OptionsToolbarViewModel OptionsToolbarCommands
        {
            get { return fOptionsToolbarCommands; }
            set { fOptionsToolbarCommands = value; OnPropertyChanged(); }
        }

        private ReportingWindowViewModel fReports;

        public ReportingWindowViewModel ReportsViewModel
        {
            get { return fReports; }
            set { fReports = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The collection of tabs to hold the data and interactions for the various subwindows.
        /// </summary>
        public List<object> Tabs { get; } = new List<object>(6);

        public MainWindowViewModel()
        {
            ReportsViewModel = new ReportingWindowViewModel();
            ReportLogger = new LogReporter(ReportsViewModel.UpdateReport);

            OptionsToolbarCommands = new OptionsToolbarViewModel(ProgramPortfolio, UpdateDataCallback, ReportLogger);
            Tabs.Add(new BasicDataViewModel(ProgramPortfolio));
            Tabs.Add(new SecurityEditWindowViewModel(ProgramPortfolio, UpdateDataCallback, ReportLogger));
            Tabs.Add(new SingleValueEditWindowViewModel("Bank Account Edit", ProgramPortfolio, UpdateDataCallback, ReportLogger, bankAccEditMethods));
            Tabs.Add(new SingleValueEditWindowViewModel("Sector Edit", ProgramPortfolio, UpdateDataCallback, ReportLogger, sectorEditMethods));
            Tabs.Add(new SingleValueEditWindowViewModel("Currency Edit", ProgramPortfolio, UpdateDataCallback, ReportLogger, currencyEditMethods));
            Tabs.Add(new StatsCreatorWindowViewModel(ProgramPortfolio, ReportLogger));


        }

        private void AllData_portfolioChanged(object sender, EventArgs e)
        {
            foreach (var tab in Tabs)
            {
                if (tab is ViewModelBase vm)
                {
                    vm.UpdateData(ProgramPortfolio);
                }
            }

            OptionsToolbarCommands.UpdateData(ProgramPortfolio);
        }

        /// <summary>
        /// 
        /// </summary>
        internal readonly LogReporter ReportLogger;

        /// <summary>
        /// The mechanism by which the data in <see cref="Portfolio"/> is updated. This includes a GUI update action.
        /// </summary>
        private Action<Action<Portfolio>> UpdateDataCallback => action => UpdateData(action);

        private void UpdateData(object obj)
        {
            if (obj is Action<Portfolio> updateAction)
            {
                updateAction(ProgramPortfolio);
                AllData_portfolioChanged(obj, null);
            }
        }
    }
}
