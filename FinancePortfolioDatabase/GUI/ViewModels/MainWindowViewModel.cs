using System;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using FinancialStructures.Database;
using Common.Structure.Reporting;
using Common.UI.ViewModelBases;
using Common.UI;
using FinancePortfolioDatabase.GUI.Configuration;
using System.Collections.ObjectModel;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    /// <summary>
    /// View model for the entire display.
    /// </summary>
    public class MainWindowViewModel : PropertyChangedBase
    {
        /// <summary>
        /// The styles for the Ui.
        /// </summary>
        public UiStyles Styles
        {
            get;
            set;
        }

        internal IPortfolio ProgramPortfolio = PortfolioFactory.GenerateEmpty();

        /// <summary>
        /// The logging mechanism for the program.
        /// </summary>
        internal readonly IReportLogger ReportLogger;
        private readonly UiGlobals fUiGlobals;
        private readonly IConfiguration fUserConfiguration;

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
        public ObservableCollection<object> Tabs
        {
            get;
            set;
        } = new ObservableCollection<object>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindowViewModel(UiGlobals globals)
        {
            Styles = new UiStyles();
            ReportsViewModel = new ReportingWindowViewModel(globals.FileInteractionService, Styles);
            ReportLogger = new LogReporter(UpdateReport);
            fUiGlobals = globals;
            fUiGlobals.ReportLogger = ReportLogger;
            fUserConfiguration = new UserConfiguration();

            OptionsToolbarCommands = new OptionsToolbarViewModel(ProgramPortfolio, UpdateDataCallback, Styles, fUiGlobals);
            Tabs.Add(new BasicDataViewModel(ProgramPortfolio, Styles, fUiGlobals));
            Tabs.Add(new SecurityEditWindowViewModel(ProgramPortfolio, UpdateDataCallback, ReportLogger, Styles, fUiGlobals));
            Tabs.Add(new ValueListWindowViewModel("Bank Accounts", ProgramPortfolio, UpdateDataCallback, Styles, fUiGlobals, Account.BankAccount));
            Tabs.Add(new ValueListWindowViewModel("Benchmarks", ProgramPortfolio, UpdateDataCallback, Styles, fUiGlobals, Account.Benchmark));
            Tabs.Add(new ValueListWindowViewModel("Currencies", ProgramPortfolio, UpdateDataCallback, Styles, fUiGlobals, Account.Currency));
            Tabs.Add(new StatsViewModel(ProgramPortfolio, Styles, fUiGlobals, fUserConfiguration.ChildConfigurations[UserConfiguration.StatsDisplay], Account.All));
            Tabs.Add(new StatisticsChartsViewModel(ProgramPortfolio, Styles));
            Tabs.Add(new StatsCreatorWindowViewModel(ProgramPortfolio, ReportLogger, Styles, fUiGlobals, fUserConfiguration.ChildConfigurations[UserConfiguration.StatsOptions], AddObjectAsMainTab));
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
        private Action<object> AddObjectAsMainTab => obj => Tabs.Add(obj);
    }
}
