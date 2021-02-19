using System;
using System.Collections.Generic;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using System.Windows;
using FinancialStructures.Database;
using StructureCommon.Reporting;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        internal IPortfolio ProgramPortfolio = PortfolioFactory.GenerateEmpty();

        private OptionsToolbarViewModel fOptionsToolbarCommands;

        public OptionsToolbarViewModel OptionsToolbarCommands
        {
            get
            {
                return fOptionsToolbarCommands;
            }
            set
            {
                fOptionsToolbarCommands = value;
                OnPropertyChanged();
            }
        }

        private ReportingWindowViewModel fReports;

        public ReportingWindowViewModel ReportsViewModel
        {
            get
            {
                return fReports;
            }
            set
            {
                fReports = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The collection of tabs to hold the data and interactions for the various subwindows.
        /// </summary>
        public List<object> Tabs { get; } = new List<object>(6);

        public MainWindowViewModel(IFileInteractionService fileInteractionService, IDialogCreationService dialogCreationService)
        {
            ReportsViewModel = new ReportingWindowViewModel(fileInteractionService);
            ReportLogger = new LogReporter(UpdateReport);

            OptionsToolbarCommands = new OptionsToolbarViewModel(ProgramPortfolio, UpdateDataCallback, ReportLogger, fileInteractionService, dialogCreationService);
            Tabs.Add(new BasicDataViewModel(ProgramPortfolio));
            Tabs.Add(new StatsCreatorWindowViewModel(ProgramPortfolio, ReportLogger, fileInteractionService, dialogCreationService));
            Tabs.Add(new SecurityEditWindowViewModel(ProgramPortfolio, UpdateDataCallback, ReportLogger, fileInteractionService, dialogCreationService));
            Tabs.Add(new ValueListWindowViewModel("Bank Accounts", ProgramPortfolio, UpdateDataCallback, ReportLogger, fileInteractionService, dialogCreationService, Account.BankAccount));
            Tabs.Add(new ValueListWindowViewModel("Benchmarks", ProgramPortfolio, UpdateDataCallback, ReportLogger, fileInteractionService, dialogCreationService, Account.Benchmark));
            Tabs.Add(new ValueListWindowViewModel("Currencies", ProgramPortfolio, UpdateDataCallback, ReportLogger, fileInteractionService, dialogCreationService, Account.Currency));

            ProgramPortfolio.PortfolioChanged += AllData_portfolioChanged;
        }

        private void AllData_portfolioChanged(object sender, PortfolioEventArgs e)
        {
            foreach (object tab in Tabs)
            {
                if (tab is DataDisplayViewModelBase vm)
                {
                    if (e.ShouldUpdate(vm.DataType))
                    {
                        Application.Current.Dispatcher?.Invoke(() => vm.UpdateData(ProgramPortfolio));
                    }
                }
            }

            OptionsToolbarCommands.UpdateData(ProgramPortfolio);
        }

        /// <summary>
        ///
        /// </summary>
        internal readonly IReportLogger ReportLogger;

        internal ErrorReports ApplicationLog = new ErrorReports();

        public void UpdateReport(ReportSeverity severity, ReportType type, ReportLocation location, string message)
        {
            ApplicationLog.AddErrorReport(severity, type, location, message);
            ReportsViewModel?.UpdateReport(severity, type, location, message);
        }

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
