using System;
using System.Collections.Generic;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using FinancialStructures.Database;
using Common.Structure.Reporting;
using Common.UI.ViewModelBases;
using Common.UI;
using FinancePortfolioDatabase.GUI.Configuration;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    /// <summary>
    /// View model for the entire display.
    /// </summary>
    public class MainWindowViewModel : PropertyChangedBase
    {
        internal IPortfolio ProgramPortfolio = PortfolioFactory.GenerateEmpty();

        /// <summary>
        /// The logging mechanism for the program.
        /// </summary>
        internal readonly IReportLogger ReportLogger;
        private readonly UiGlobals fUiGlobals;
        private UserConfiguration fUserConfiguration;

        private OptionsToolbarViewModel fOptionsToolbarCommands;

        /// <summary>
        /// view model for the top toolbar.
        /// </summary>
        public OptionsToolbarViewModel OptionsToolbarCommands
        {
            get => fOptionsToolbarCommands;
            set
            {
                fOptionsToolbarCommands = value;
                OnPropertyChanged();
            }
        }

        private ReportingWindowViewModel fReports;

        /// <summary>
        /// View model for the reports view.
        /// </summary>
        public ReportingWindowViewModel ReportsViewModel
        {
            get => fReports;
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

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindowViewModel(UiGlobals globals)
        {
            ReportsViewModel = new ReportingWindowViewModel(globals.FileInteractionService);
            ReportLogger = new LogReporter(UpdateReport);
            fUiGlobals = globals;
            fUiGlobals.ReportLogger = ReportLogger;
            fUserConfiguration = new UserConfiguration();

            OptionsToolbarCommands = new OptionsToolbarViewModel(ProgramPortfolio, UpdateDataCallback, fUiGlobals);
            Tabs.Add(new BasicDataViewModel(ProgramPortfolio, fUiGlobals));
            Tabs.Add(new StatsCreatorWindowViewModel(ProgramPortfolio, ReportLogger, fUiGlobals, fUserConfiguration));
            Tabs.Add(new SecurityEditWindowViewModel(ProgramPortfolio, UpdateDataCallback, ReportLogger, fUiGlobals));
            Tabs.Add(new ValueListWindowViewModel("Bank Accounts", ProgramPortfolio, UpdateDataCallback, fUiGlobals, Account.BankAccount));
            Tabs.Add(new ValueListWindowViewModel("Benchmarks", ProgramPortfolio, UpdateDataCallback, fUiGlobals, Account.Benchmark));
            Tabs.Add(new ValueListWindowViewModel("Currencies", ProgramPortfolio, UpdateDataCallback, fUiGlobals, Account.Currency));

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

            if (e.ChangedPortfolio)
            {
                ReportsViewModel?.ClearReportsCommand.Execute(null);
            }
        }

        private void UpdateReport(ReportSeverity severity, ReportType type, ReportLocation location, string message)
        {
            ReportsViewModel?.UpdateReport(severity, type, location, message);
        }

        /// <summary>
        /// The mechanism by which the data in <see cref="ProgramPortfolio"/> is updated. This includes a GUI update action.
        /// </summary>
        private Action<Action<IPortfolio>> UpdateDataCallback => action => action(ProgramPortfolio);
    }
}
