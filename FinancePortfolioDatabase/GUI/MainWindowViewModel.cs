using FinanceCommonViewModels;
using FinancialStructures.ReportingStructures;
using GUISupport;
using SavingClasses;
using System;

namespace FinanceWindowsViewModels
{
    internal class MainWindowViewModel : PropertyChangedBase
    {
        internal AllData allData = new AllData();

        public MainWindowViewModel()
        {
            OptionsToolbarCommands = new OptionsToolbarViewModel(allData.MyFunds, allData.myBenchMarks, updateDataCallback, UpdateReports);
            DataView = new BasicDataViewModel(allData.MyFunds, allData.myBenchMarks);
            SecurityEditViewModel = new SecurityEditWindowViewModel(allData.MyFunds, allData.myBenchMarks, updateDataCallback, UpdateReports);
            BankAccEditViewModel = new SingleValueEditWindowViewModel(allData.MyFunds, allData.myBenchMarks, updateDataCallback, UpdateReports, allData.bankAccEditMethods);
            SectorEditViewModel = new SingleValueEditWindowViewModel(allData.MyFunds, allData.myBenchMarks, updateDataCallback, UpdateReports, allData.sectorEditMethods);
            CurrencyEditViewModel = new SingleValueEditWindowViewModel(allData.MyFunds, allData.myBenchMarks, updateDataCallback, UpdateReports, allData.currencyEditMethods);
            StatsEditViewModel = new StatsCreatorWindowViewModel(allData.MyFunds, allData.myBenchMarks, UpdateReports);
            ReportsViewModel = new ReportingWindowViewModel();

            AllData.portfolioChanged += AllData_portfolioChanged;
        }

        private void AllData_portfolioChanged(object sender, EventArgs e)
        {
            DataView.DataUpdate(allData.MyFunds, allData.myBenchMarks);
            SecurityEditViewModel.UpdateListBoxes(allData.MyFunds, allData.myBenchMarks);
            BankAccEditViewModel.UpdateListBoxes(allData.MyFunds, allData.myBenchMarks);
            SectorEditViewModel.UpdateListBoxes(allData.MyFunds, allData.myBenchMarks);
            CurrencyEditViewModel.UpdateListBoxes(allData.MyFunds, allData.myBenchMarks);
            StatsEditViewModel.GenerateStatistics(allData.MyFunds, allData.myBenchMarks);
        }

        Action<ErrorReports> UpdateReports => (val) => AddReports(val);

        public void AddReports(ErrorReports reports)
        {
            ReportsViewModel.UpdateReports(reports);
        }

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

        private BasicDataViewModel fDataView;

        public BasicDataViewModel DataView
        {
            get { return fDataView; }
            set { fDataView = value; OnPropertyChanged(); }
        }

        private SecurityEditWindowViewModel fSecurityEditViewModel;
        public SecurityEditWindowViewModel SecurityEditViewModel
        {
            get { return fSecurityEditViewModel; }
            set { fSecurityEditViewModel = value; OnPropertyChanged(); }
        }


        private SingleValueEditWindowViewModel fSectorEditViewModel;

        public SingleValueEditWindowViewModel SectorEditViewModel
        {
            get { return fSectorEditViewModel; }
            set { fSectorEditViewModel = value; OnPropertyChanged(); }
        }

        private SingleValueEditWindowViewModel fCurrencyEditViewModel;

        public SingleValueEditWindowViewModel CurrencyEditViewModel
        {
            get { return fCurrencyEditViewModel; }
            set { fCurrencyEditViewModel = value; OnPropertyChanged(); }
        }

        private SingleValueEditWindowViewModel fBankAccEditViewModel;

        public SingleValueEditWindowViewModel BankAccEditViewModel
        {
            get { return fBankAccEditViewModel; }
            set { fBankAccEditViewModel = value; OnPropertyChanged(); }
        }

        private StatsCreatorWindowViewModel fStatsViewModel;

        public StatsCreatorWindowViewModel StatsEditViewModel
        {
            get { return fStatsViewModel; }
            set { fStatsViewModel = value; OnPropertyChanged(); }
        }

        private ReportingWindowViewModel fReports;
        public ReportingWindowViewModel ReportsViewModel
        {
            get { return fReports; }
            set { fReports = value; OnPropertyChanged(); }
        }
    }
}
