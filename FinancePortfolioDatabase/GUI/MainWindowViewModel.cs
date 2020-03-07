using FinanceCommonViewModels;
using GUISupport;
using SavingClasses;
using System;
using System.Collections.Generic;

namespace FinanceWindowsViewModels
{
    internal class MainWindowViewModel : PropertyChangedBase
    {
        /// <summary>
        /// The main store of the database for the program.
        /// </summary>
        internal AllData allData = new AllData();

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
        public List<object> Tabs { get; set; } = new List<object>(6);

        public MainWindowViewModel()
        {
            OptionsToolbarCommands = new OptionsToolbarViewModel(allData.MyFunds, UpdateDataCallback, ReportLogger);

            Tabs[0] = new BasicDataViewModel(allData.MyFunds, allData.myBenchMarks);
            Tabs[1] = new SecurityEditWindowViewModel(allData.MyFunds, UpdateDataCallback, ReportLogger);
            Tabs[2] = new SingleValueEditWindowViewModel("Bank Account Edit", allData.MyFunds, allData.myBenchMarks, UpdateDataCallback, ReportLogger, allData.bankAccEditMethods);
            Tabs[3] = new SingleValueEditWindowViewModel("Sector Edit", allData.MyFunds, allData.myBenchMarks, UpdateDataCallback, ReportLogger, allData.sectorEditMethods);
            Tabs[4] = new SingleValueEditWindowViewModel("Currency Edit", allData.MyFunds, allData.myBenchMarks, UpdateDataCallback, ReportLogger, allData.currencyEditMethods);
            Tabs[5] = new StatsCreatorWindowViewModel(allData.MyFunds, allData.myBenchMarks, ReportLogger);

            ReportsViewModel = new ReportingWindowViewModel();

            AllData.portfolioChanged += AllData_portfolioChanged;
        }

        private void AllData_portfolioChanged(object sender, EventArgs e)
        {
            foreach (var tab in Tabs)
            {
                if (tab is ViewModelBase vm)
                {
                    vm.UpdateData(allData.MyFunds, allData.myBenchMarks);
                }
            }

            OptionsToolbarCommands.UpdateData(allData.MyFunds, allData.myBenchMarks);
        }

        /// <summary>
        /// The callback to report an error into the reporting window.
        /// </summary>
        public Action<string, string, string> ReportLogger => (type, location, message) => AddReport(type, location, message);

        private void AddReport(string type, string location, string message)
        {
            ReportsViewModel.UpdateReport(type, location, message);
        }

        /// <summary>
        /// The mechanism by which the data in <see cref="AllData"/> is updated. This includes a GUI update action.
        /// </summary>
        private Action<Action<AllData>> UpdateDataCallback => action => UpdateData(action);

        private void UpdateData(object obj)
        {
            if (obj is Action<AllData> updateAction)
            {
                updateAction(allData);
                AllData_portfolioChanged(obj, null);
            }
        }
    }
}
