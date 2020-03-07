using FinanceCommonViewModels;
using GUISupport;
using SavingClasses;
using System;
using System.Collections.ObjectModel;

namespace FinanceWindowsViewModels
{
    internal class MainWindowViewModel : PropertyChangedBase
    {
        internal AllData allData = new AllData();

        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        public MainWindowViewModel()
        {
            OptionsToolbarCommands = new OptionsToolbarViewModel(allData.MyFunds, allData.myBenchMarks, updateDataCallback, ReportLogger);
            Tabs.Add(new BasicDataViewModel(allData.MyFunds, allData.myBenchMarks));
            Tabs.Add(new SecurityEditWindowViewModel(allData.MyFunds, allData.myBenchMarks, updateDataCallback, ReportLogger));
            Tabs.Add(new SingleValueEditWindowViewModel("Bank Account Edit", allData.MyFunds, allData.myBenchMarks, updateDataCallback, ReportLogger, allData.bankAccEditMethods));
            Tabs.Add(new SingleValueEditWindowViewModel("Sector Edit", allData.MyFunds, allData.myBenchMarks, updateDataCallback, ReportLogger, allData.sectorEditMethods));
            Tabs.Add(new SingleValueEditWindowViewModel("Currency Edit", allData.MyFunds, allData.myBenchMarks, updateDataCallback, ReportLogger, allData.currencyEditMethods));
            Tabs.Add(new StatsCreatorWindowViewModel(allData.MyFunds, allData.myBenchMarks, ReportLogger));

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
        }

        /// <summary>
        /// The callback to report an error into the reporting window.
        /// </summary>
        public Action<string, string, string> ReportLogger => (type, location, message) => AddReport(type, location, message);

        public void AddReport(string type, string location, string message)
        {
            ReportsViewModel.UpdateReport(type, location, message);
        }

        /// <summary>
        /// The mechanism by which the data in <see cref="AllData"/> is updated. This includes a GUI update action.
        /// </summary>
        Action<Action<AllData>> updateDataCallback => action => UpdateData(action);

        public void UpdateData(object obj)
        {
            if (obj is Action<AllData> updateAction)
            {
                updateAction(allData);
                AllData_portfolioChanged(obj, null);
            }
        }

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
    }
}
