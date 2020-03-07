using FinancialStructures.Reporting;
using GUISupport;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class ReportingWindowViewModel : PropertyChangedBase
    {
        private ObservableCollection<ErrorReport> fReportsToView;

        public ObservableCollection<ErrorReport> ReportsToView
        {
            get { return fReportsToView; }
            set { fReportsToView = value; OnPropertyChanged(); }
        }

        private ErrorReports fReports;

        public ErrorReports Reports
        {
            get { return fReports; }
            set { fReports = value; OnPropertyChanged(); }
        }

        private int fIndexToDelete;

        public int IndexToDelete
        {
            get { return fIndexToDelete; }
            set { fIndexToDelete = value; OnPropertyChanged(); }
        }

        public ReportingWindowViewModel()
        {
            Reports = new ErrorReports();
            ReportsToView = new ObservableCollection<ErrorReport>();
            ClearReportsCommand = new BasicCommand(ExecuteClearReports);
            ClearSingleReportCommand = new BasicCommand(ExecuteClearSelectedReport);
            SyncReports();
        }

        internal void SyncReports()
        {
            ReportsToView = null;
            ReportsToView = new ObservableCollection<ErrorReport>(Reports.GetReports());
            OnPropertyChanged(nameof(ReportsToView));
        }

        public ICommand ClearReportsCommand { get; }
        
        private void ExecuteClearReports(Object obj)
        {
            Reports = new ErrorReports();
            SyncReports();
        }

        public ICommand ClearSingleReportCommand { get; }

        private void ExecuteClearSelectedReport(Object obj)
        {
            Reports.RemoveReport(IndexToDelete);
            SyncReports();
        }

        public void UpdateReport(string type, string location, string message)
        {
            Reports.AddReport(type, location, message);
            SyncReports();
        }
    }
}
