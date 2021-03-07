using System;
using System.Collections.Generic;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using System.Windows;
using FinancialStructures.Database;
using StructureCommon.Reporting;
using UICommon.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        internal IPortfolio ProgramPortfolio = PortfolioFactory.GenerateEmpty();

        /// <summary>
        /// The logging mechanism for the program. This both records into the 
        /// reporting window and to the <see cref="ApplicationLog"/>.
        /// </summary>
        internal readonly IReportLogger ReportLogger;
        private readonly UiGlobals fUiGlobals;

        /// <summary>
        /// The log of the application.
        /// </summary>
        internal ErrorReports ApplicationLog = new ErrorReports();

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

        public MainWindowViewModel(UiGlobals globals)
        {
            ReportsViewModel = new ReportingWindowViewModel(globals.FileInteractionService);
            ReportLogger = new LogReporter(UpdateReport);
            fUiGlobals = globals;

            OptionsToolbarCommands = new OptionsToolbarViewModel(ProgramPortfolio, UpdateDataCallback, ReportLogger, fUiGlobals);
            Tabs.Add(new BasicDataViewModel(ProgramPortfolio));
            Tabs.Add(new StatsCreatorWindowViewModel(ProgramPortfolio, ReportLogger, globals));
            Tabs.Add(new SecurityEditWindowViewModel(ProgramPortfolio, UpdateDataCallback, ReportLogger, globals));
            Tabs.Add(new ValueListWindowViewModel("Bank Accounts", ProgramPortfolio, UpdateDataCallback, ReportLogger, globals.FileInteractionService, globals.DialogCreationService, Account.BankAccount));
            Tabs.Add(new ValueListWindowViewModel("Benchmarks", ProgramPortfolio, UpdateDataCallback, ReportLogger, globals.FileInteractionService, globals.DialogCreationService, Account.Benchmark));
            Tabs.Add(new ValueListWindowViewModel("Currencies", ProgramPortfolio, UpdateDataCallback, ReportLogger, globals.FileInteractionService, globals.DialogCreationService, Account.Currency));

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
                        fUiGlobals.CurrentDispatcher?.Invoke(() => vm.UpdateData(ProgramPortfolio));
                    }
                }
            }

            OptionsToolbarCommands.UpdateData(ProgramPortfolio);
        }

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
