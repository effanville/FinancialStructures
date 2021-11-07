using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Common.Structure.Reporting;
using Common.UI;
using Common.UI.ViewModelBases;
using FinancePortfolioDatabase.GUI.Configuration;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using FinancialStructures.Database;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    /// <summary>
    /// View model for the entire display.
    /// </summary>
    public class MainWindowViewModel : PropertyChangedBase
    {
        /// <summary>
        /// The mechanism by which the data in <see cref="ProgramPortfolio"/> is updated. This includes a GUI update action.
        /// </summary>
        private Action<Action<IPortfolio>> UpdateDataCallback => action => action(ProgramPortfolio);
        private Action<object> AddObjectAsMainTab => obj => Tabs.Add(obj);

        internal IPortfolio ProgramPortfolio = PortfolioFactory.GenerateEmpty();

        /// <summary>
        /// The logging mechanism for the program.
        /// </summary>
        internal readonly IReportLogger ReportLogger;
        private readonly UiGlobals fUiGlobals;
        private UserConfiguration fUserConfiguration;
        private string fConfigLocation;

        private UiStyles fStyles;

        /// <summary>
        /// The styles for the Ui.
        /// </summary>
        public UiStyles Styles
        {
            get => fStyles;
            set => SetAndNotify(ref fStyles, value, nameof(Styles));
        }

        private OptionsToolbarViewModel fOptionsToolbarCommands;

        /// <summary>
        /// view model for the top toolbar.
        /// </summary>
        public OptionsToolbarViewModel OptionsToolbarCommands
        {
            get => fOptionsToolbarCommands;
            set => SetAndNotify(ref fOptionsToolbarCommands, value, nameof(OptionsToolbarCommands));
        }

        private ReportingWindowViewModel fReports;

        /// <summary>
        /// View model for the reports view.
        /// </summary>
        public ReportingWindowViewModel ReportsViewModel
        {
            get => fReports;
            set => SetAndNotify(ref fReports, value, nameof(ReportsViewModel));
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

            LoadConfig();

            OptionsToolbarCommands = new OptionsToolbarViewModel(fUiGlobals, Styles, ProgramPortfolio, UpdateDataCallback);
            Tabs.Add(new BasicDataViewModel(fUiGlobals, Styles, ProgramPortfolio, UpdateDataCallback));
            Tabs.Add(new SecurityEditWindowViewModel(fUiGlobals, Styles, ProgramPortfolio, UpdateDataCallback));
            Tabs.Add(new ValueListWindowViewModel(fUiGlobals, Styles, ProgramPortfolio, "Bank Accounts", Account.BankAccount, UpdateDataCallback));
            Tabs.Add(new ValueListWindowViewModel(fUiGlobals, Styles, ProgramPortfolio, "Benchmarks", Account.Benchmark, UpdateDataCallback));
            Tabs.Add(new ValueListWindowViewModel(fUiGlobals, Styles, ProgramPortfolio, "Currencies", Account.Currency, UpdateDataCallback));
            Tabs.Add(new StatsViewModel(fUiGlobals, Styles, fUserConfiguration.ChildConfigurations[UserConfiguration.StatsDisplay], ProgramPortfolio, Account.All));
            Tabs.Add(new StatisticsChartsViewModel(ProgramPortfolio, Styles));
            Tabs.Add(new StatsCreatorWindowViewModel(fUiGlobals, Styles, fUserConfiguration.ChildConfigurations[UserConfiguration.StatsCreator], ProgramPortfolio, AddObjectAsMainTab));
            ProgramPortfolio.PortfolioChanged += AllData_portfolioChanged;
        }

        private void LoadConfig()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName name = assembly.GetName();
            fConfigLocation = fUiGlobals.CurrentFileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), name.Name, "user.config");
            fUserConfiguration = UserConfiguration.LoadFromUserConfigFile(fConfigLocation, fUiGlobals.CurrentFileSystem, ReportLogger);
        }

        /// <summary>
        /// Saves the user configuration to the local appData folder.
        /// </summary>
        public void SaveConfig()
        {
            fUserConfiguration.SaveConfiguration(fConfigLocation, fUiGlobals.CurrentFileSystem);
        }

        private void AllData_portfolioChanged(object sender, PortfolioEventArgs e)
        {
            foreach (object tab in Tabs)
            {
                if (tab is DataDisplayViewModelBase vm && e.ShouldUpdate(vm.DataType))
                {
                    fUiGlobals.CurrentDispatcher?.Invoke(() => vm.UpdateData(ProgramPortfolio));
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
    }
}
