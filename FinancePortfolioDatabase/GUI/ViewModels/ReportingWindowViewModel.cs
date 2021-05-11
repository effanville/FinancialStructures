using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using StructureCommon.Reporting;
using UICommon.Commands;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    public class ReportingWindowViewModel : PropertyChangedBase
    {
        private ObservableCollection<ErrorReport> fReportsToView;
        private readonly IFileInteractionService fFileInteractionService;
        public ObservableCollection<ErrorReport> ReportsToView
        {
            get
            {
                return fReportsToView;
            }
            set
            {
                fReportsToView = value;
                OnPropertyChanged(nameof(ReportsToView));
            }
        }

        private ErrorReports fReports;

        public ErrorReports Reports
        {
            get
            {
                return fReports;
            }
            set
            {
                fReports = value;
                OnPropertyChanged(nameof(Reports));
            }
        }

        private int fIndexToDelete;

        public int IndexToDelete
        {
            get
            {
                return fIndexToDelete;
            }
            set
            {
                fIndexToDelete = value;
                OnPropertyChanged(nameof(IndexToDelete));
            }
        }

        private ReportSeverity fReportingSeverity;

        public ReportSeverity ReportingSeverity
        {
            get
            {
                return fReportingSeverity;
            }
            set
            {
                fReportingSeverity = value;
                OnPropertyChanged(nameof(ReportingSeverity));
                SyncReports();
            }
        }

        public List<ReportSeverity> EnumValues
        {
            get
            {
                return Enum.GetValues(typeof(ReportSeverity)).Cast<ReportSeverity>().ToList();
            }
        }

        public ReportingWindowViewModel(IFileInteractionService fileInteractionService)
        {
            fFileInteractionService = fileInteractionService;
            Reports = new ErrorReports();
            ReportsToView = new ObservableCollection<ErrorReport>();
            ClearReportsCommand = new RelayCommand(ExecuteClearReports);
            ClearSingleReportCommand = new RelayCommand(ExecuteClearSelectedReport);
            ExportReportsCommand = new RelayCommand(ExecuteExportReportsCommand);
            SyncReports();
        }

        internal void SyncReports()
        {
            ReportsToView = null;
            ReportsToView = new ObservableCollection<ErrorReport>(Reports.GetReports(ReportingSeverity));
            OnPropertyChanged(nameof(ReportsToView));
        }

        public ICommand ClearReportsCommand
        {
            get;
        }

        private void ExecuteClearReports()
        {
            Reports.Clear();
            SyncReports();
        }

        public ICommand ClearSingleReportCommand
        {
            get;
        }

        private void ExecuteClearSelectedReport()
        {
            Reports.RemoveReport(IndexToDelete);
            SyncReports();
        }

        public ICommand ExportReportsCommand
        {
            get;
        }

        private void ExecuteExportReportsCommand()
        {
            try
            {
                var result = fFileInteractionService.SaveFile(".csv", "errorReports.csv");
                if (result.Success != null && (bool)result.Success)
                {
                    StreamWriter writer = new StreamWriter(result.FilePath);
                    writer.WriteLine("Severity,ErrorType,Location,Message");
                    foreach (var report in Reports.GetReports())
                    {
                        writer.WriteLine(report.ErrorSeverity.ToString() + "," + report.ErrorType + "," + report.ErrorLocation.ToString() + "," + report.Message);
                    }

                    writer.Close();
                }
            }
            catch (IOException)
            {
            }
        }

        public void UpdateReport(ReportSeverity severity, ReportType type, ReportLocation location, string message)
        {
            Reports.AddErrorReport(severity, type, location, message);
            SyncReports();
        }
    }
}
