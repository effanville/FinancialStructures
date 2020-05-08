using FinancialStructures.Reporting;
using GUISupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class ReportingWindowViewModel : PropertyChangedBase
    {
        private ObservableCollection<ErrorReport> fReportsToView;

        public ObservableCollection<ErrorReport> ReportsToView
        {
            get { return fReportsToView; }
            set { fReportsToView = value; OnPropertyChanged(nameof(ReportsToView)); }
        }

        private ErrorReports fReports;

        public ErrorReports Reports
        {
            get { return fReports; }
            set { fReports = value; OnPropertyChanged(nameof(Reports)); }
        }

        private int fIndexToDelete;

        public int IndexToDelete
        {
            get { return fIndexToDelete; }
            set { fIndexToDelete = value; OnPropertyChanged(nameof(IndexToDelete)); }
        }

        private ReportSeverity fReportingSeverity;

        public ReportSeverity ReportingSeverity
        {
            get { return fReportingSeverity; }
            set { fReportingSeverity = value; OnPropertyChanged(nameof(ReportingSeverity)); SyncReports(); }
        }

        public List<ReportSeverity> EnumValues
        {
            get { return Enum.GetValues(typeof(ReportSeverity)).Cast<ReportSeverity>().ToList(); }
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
            ReportsToView = new ObservableCollection<ErrorReport>(Reports.GetReports(ReportingSeverity));
            OnPropertyChanged(nameof(ReportsToView));
        }

        public ICommand ClearReportsCommand { get; }

        private void ExecuteClearReports(Object obj)
        {
            Reports.Clear();
            SyncReports();
        }

        public ICommand ClearSingleReportCommand { get; }

        private void ExecuteClearSelectedReport(Object obj)
        {
            Reports.RemoveReport(IndexToDelete);
            SyncReports();
        }

        public void UpdateReport(ReportSeverity severity, ReportType type, ReportLocation location, string message)
        {
            Reports.AddErrorReport(severity, type, location, message);
            SyncReports();
        }
    }
}
