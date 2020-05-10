using FinanceCommonViewModels;
using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Reporting;
using System;
using System.Collections.Generic;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace FinanceWindowsViewModels
{
    internal class MainWindowViewModel : PropertyChangedBase
    {
        public EditMethods securityEditMethods = EditMethods.GenerateEditMethods(AccountType.Security);

        public EditMethods bankAccEditMethods = EditMethods.GenerateEditMethods(AccountType.BankAccount);

        public EditMethods sectorEditMethods = EditMethods.GenerateEditMethods(AccountType.Sector);

        public EditMethods currencyEditMethods = EditMethods.GenerateEditMethods(AccountType.Currency);

        internal IPortfolio ProgramPortfolio = new Portfolio();

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

        public MainWindowViewModel(IFileInteractionService fileInteractionService, IDialogCreationService dialogCreationService)
        {
            ReportsViewModel = new ReportingWindowViewModel();
            ReportLogger = new LogReporter(ReportsViewModel.UpdateReport);

            OptionsToolbarCommands = new OptionsToolbarViewModel(ProgramPortfolio, UpdateDataCallback, ReportLogger, fileInteractionService, dialogCreationService);
            Tabs.Add(new BasicDataViewModel(ProgramPortfolio));
            Tabs.Add(new SecurityEditWindowViewModel(ProgramPortfolio, UpdateDataCallback, securityEditMethods, ReportLogger, fileInteractionService, dialogCreationService));
            Tabs.Add(new SingleValueEditWindowViewModel("Bank Account Edit", ProgramPortfolio, UpdateDataCallback, ReportLogger, fileInteractionService, dialogCreationService, bankAccEditMethods, AccountType.BankAccount));
            Tabs.Add(new SingleValueEditWindowViewModel("Sector Edit", ProgramPortfolio, UpdateDataCallback, ReportLogger, fileInteractionService, dialogCreationService, sectorEditMethods, AccountType.Sector));
            Tabs.Add(new SingleValueEditWindowViewModel("Currency Edit", ProgramPortfolio, UpdateDataCallback, ReportLogger, fileInteractionService, dialogCreationService, currencyEditMethods, AccountType.Currency));
            Tabs.Add(new StatsCreatorWindowViewModel(ProgramPortfolio, ReportLogger, fileInteractionService, dialogCreationService));
            ProgramPortfolio.PortfolioChanged += AllData_portfolioChanged;
        }

        private void AllData_portfolioChanged(object sender, EventArgs e)
        {
            foreach (var tab in Tabs)
            {
                if (tab is ViewModelBase<IPortfolio> vm)
                {
                    vm.UpdateData(ProgramPortfolio);
                }
            }

            OptionsToolbarCommands.UpdateData(ProgramPortfolio);
        }

        /// <summary>
        /// 
        /// </summary>
        internal readonly IReportLogger ReportLogger;

        /// <summary>
        /// The mechanism by which the data in <see cref="ProgramPortfolio"/> is updated. This includes a GUI update action.
        /// </summary>
        private Action<Action<IPortfolio>> UpdateDataCallback
        {
            get
            {
                return action => action(ProgramPortfolio);
            }
        }
    }
}
