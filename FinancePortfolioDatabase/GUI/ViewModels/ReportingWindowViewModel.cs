using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Common.Structure.Reporting;
using Common.UI.Commands;
using Common.UI.Services;
using Common.UI.ViewModelBases;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    /// <summary>
    /// View model for the user control that displays reports.
    /// </summary>
    public class ReportingWindowViewModel : PropertyChangedBase
    {
        private ErrorReports fReportsToView;
        private readonly IFileInteractionService fFileInteractionService;

        /// <summary>
        /// The styles to display in the UI.
        /// </summary>
        public UiStyles Styles
        {
            get; set;
        }

        /// <summary>
        /// The reports to display in the control. This is a sublist of <see cref="Reports"/> filtered by <see cref="ReportingSeverity"/>.
        /// </summary>
        public ErrorReports ReportsToView
        {
            get => fReportsToView;
            set => SetAndNotify(ref fReportsToView, value, nameof(ReportsToView));
        }

        private bool fIsExpanded;
        /// <summary>
        /// Whether the view is expanded or not.
        /// </summary>
        public bool IsExpanded
        {
            get => fIsExpanded;
            set => SetAndNotify(ref fIsExpanded, value, nameof(IsExpanded));
        }

        /// <summary>
        /// Complete list of all reports stored.
        /// </summary>
        public ErrorReports Reports
        {
            get;
            set;
        }

        /// <summary>
        /// Selected index of the reports.
        /// </summary>
        public int IndexToDelete
        {
            get;
            set;
        }

        private ReportSeverity fReportingSeverity;

        /// <summary>
        /// The selected reporting severity.
        /// </summary>
        public ReportSeverity ReportingSeverity
        {
            get => fReportingSeverity;
            set
            {
                SetAndNotify(ref fReportingSeverity, value, nameof(ReportingSeverity));
                SyncReports();
            }
        }

        /// <summary>
        /// List of all types of ReportSeverity.
        /// </summary>
        public static List<ReportSeverity> ReportSeverityValues => Enum.GetValues(typeof(ReportSeverity)).Cast<ReportSeverity>().ToList();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ReportingWindowViewModel(IFileInteractionService fileInteractionService, UiStyles styles)
        {
            Styles = styles;
            IsExpanded = false;
            fFileInteractionService = fileInteractionService;
            Reports = new ErrorReports();
            ReportsToView = new ErrorReports();
            ClearReportsCommand = new RelayCommand(ExecuteClearReports);
            DeleteCommand = new RelayCommand<KeyEventArgs>(ExecuteDeleteReport);
            ExportReportsCommand = new RelayCommand(ExecuteExportReportsCommand);
            SyncReports();
        }

        internal void SyncReports()
        {
            ReportsToView = null;
            ReportsToView = new ErrorReports(Reports.GetReports(ReportingSeverity));
            if (ReportsToView != null && (ReportsToView?.Any() ?? false))
            {
                IsExpanded = true;
            }
        }

        /// <summary>
        /// Command to clear all reports in the control.
        /// </summary>
        public ICommand ClearReportsCommand
        {
            get;
        }

        private void ExecuteClearReports()
        {
            Reports.Clear();
            SyncReports();
        }

        /// <summary>
        /// Command to export reports to a csv.
        /// </summary>
        public ICommand ExportReportsCommand
        {
            get;
        }

        private void ExecuteExportReportsCommand()
        {
            try
            {
                FileInteractionResult result = fFileInteractionService.SaveFile(".csv", "errorReports.csv");
                if (result.Success != null && (bool)result.Success)
                {
                    StreamWriter writer = new StreamWriter(result.FilePath);
                    writer.WriteLine("Severity,ErrorType,Location,Message");
                    foreach (ErrorReport report in Reports.GetReports())
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

        /// <summary>
        /// Command to delete a selected report.
        /// </summary>
        public ICommand DeleteCommand
        {
            get;
            set;
        }

        private void ExecuteDeleteReport(KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (IndexToDelete >= 0)
                {
                    Reports.RemoveReport(IndexToDelete);
                    SyncReports();
                }
            }
        }

        /// <summary>
        /// Updates the reports in the window.
        /// </summary>
        public void UpdateReport(ReportSeverity severity, ReportType type, ReportLocation location, string message)
        {
            Reports.AddErrorReport(severity, type, location, message);
            SyncReports();
        }
    }
}
