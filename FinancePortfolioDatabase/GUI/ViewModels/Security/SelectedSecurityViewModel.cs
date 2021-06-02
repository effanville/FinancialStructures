using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.FileAccess;
using StructureCommon.Reporting;
using UICommon.Commands;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels.Security
{
    public class SelectedSecurityViewModel : TabViewModelBase<IPortfolio>
    {
        /// <inheritdoc/>
        public override bool Closable
        {
            get
            {
                return true;
            }
        }

        public readonly NameData SelectedName;

        /// <summary>
        /// The pricing data of the selected security.
        /// </summary>
        private List<SecurityDayData> fSelectedSecurityData = new List<SecurityDayData>();
        public List<SecurityDayData> SelectedSecurityData
        {
            get
            {
                return fSelectedSecurityData;
            }
            set
            {
                SetAndNotify(ref fSelectedSecurityData, value, nameof(SelectedSecurityData));
                _ = fReportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {SelectedName} updated selected data.");
            }
        }

        internal SecurityDayData fOldSelectedValue;
        internal SecurityDayData SelectedValue;

        private readonly Action<Action<IPortfolio>> UpdateDataCallback;

        private readonly IReportLogger fReportLogger;
        private readonly UiGlobals fUiGlobals;

        public SelectedSecurityViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, IReportLogger reportLogger, UiGlobals globals, NameData selectedName)
            : base(selectedName != null ? selectedName.ToString() : "No-Name", portfolio)
        {
            fReportLogger = reportLogger;
            fUiGlobals = globals;
            SelectedName = selectedName;
            DeleteValuationCommand = new RelayCommand(ExecuteDeleteValuation);
            PreEditCommand = new RelayCommand(ExecutePreEdit);
            AddCsvData = new RelayCommand(ExecuteAddCsvData);
            ExportCsvData = new RelayCommand(ExecuteExportCsvData);
            AddEditSecurityDataCommand = new RelayCommand(ExecuteAddEditSecData);
            SelectionChangedCommand = new RelayCommand<object>(ExecuteSelectionChanged);
            AddDefaultDataCommand = new RelayCommand<AddingNewItemEventArgs>(e => DataGrid_AddingNewItem(null, e));
            UpdateData(portfolio, null);
            UpdateDataCallback = updateData;
        }

        public ICommand DeleteValuationCommand
        {
            get;
        }

        private void ExecuteDeleteValuation()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {SelectedName} deleting valuation {fOldSelectedValue}.");
            if (SelectedName != null && SelectedValue != null)
            {
                UpdateDataCallback(programPortfolio => programPortfolio.TryDeleteData(Account.Security, SelectedName, SelectedValue.Date, fReportLogger));
            }
            else
            {
                _ = fReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, "No Account was selected when trying to delete data.");
            }
        }

        public ICommand AddCsvData
        {
            get;
        }

        private void ExecuteAddCsvData()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {SelectedName} adding data from csv.");
            if (SelectedName != null)
            {
                FileInteractionResult result = fUiGlobals.FileInteractionService.OpenFile("csv", filter: "Csv Files|*.csv|All Files|*.*");
                List<object> outputs = null;
                bool exists = DataStore.TryGetAccount(Account.Security, SelectedName, out IValueList account);
                if (result.Success != null && (bool)result.Success && exists)
                {
                    var security = account as ISecurity;
                    outputs = CsvReaderWriter.ReadFromCsv(security, result.FilePath, fReportLogger);
                }
                if (outputs != null)
                {
                    foreach (object objec in outputs)
                    {
                        if (objec is SecurityDayData view)
                        {
                            UpdateDataCallback(programPortfolio => programPortfolio.TryAddOrEditDataToSecurity(SelectedName, view.Date, view.Date, view.ShareNo, view.UnitPrice, view.NewInvestment, fReportLogger));
                        }
                        else
                        {
                            _ = fReportLogger.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, "Have the wrong type of thing");
                        }
                    }
                }
            }
        }

        public ICommand ExportCsvData
        {
            get;
        }

        private void ExecuteExportCsvData()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {SelectedName} exporting data to csv.");
            if (SelectedName != null)
            {
                FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile("csv", string.Empty, DataStore.Directory, "Csv Files|*.csv|All Files|*.*");
                if (result.Success != null && (bool)result.Success)
                {
                    if (DataStore.TryGetAccount(Account.Security, SelectedName, out IValueList account))
                    {
                        var security = account as ISecurity;
                        CsvReaderWriter.WriteToCSVFile(security, result.FilePath, fReportLogger);
                    }
                    else
                    {
                        _ = fReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Saving, "Could not find security.");
                    }
                }
            }
        }

        /// <summary>
        /// Called prior to an edit occurring in a row. This is used
        /// to record the state of the row before editing.
        /// </summary>
        public ICommand PreEditCommand
        {
            get;
            set;
        }

        private void ExecutePreEdit()
        {
            fOldSelectedValue = SelectedValue?.Copy();
        }

        public ICommand AddDefaultDataCommand
        {
            get;
            set;
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            double shareNo = 0;
            double unitPrice = 0;
            if (SelectedSecurityData != null && SelectedSecurityData.Any())
            {
                var latest = SelectedSecurityData.Last();
                shareNo = latest.ShareNo;
                unitPrice = latest.UnitPrice;
            }
            e.NewItem = new SecurityDayData()
            {
                UnitPrice = unitPrice,
                ShareNo = shareNo,
            };
        }

        public ICommand SelectionChangedCommand
        {
            get;
            set;
        }
        private void ExecuteSelectionChanged(object obj)
        {
            if (SelectedSecurityData != null && obj is SecurityDayData data)
            {
                SelectedValue = data;
            }
        }

        public ICommand AddEditSecurityDataCommand
        {
            get;
            set;
        }

        private void ExecuteAddEditSecData()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {SelectedName}adding valuation called.");
            if (SelectedName != null)
            {
                bool edited = false;
                UpdateDataCallback(programPortfolio => programPortfolio.TryAddOrEditDataToSecurity(SelectedName, fOldSelectedValue.Date, SelectedValue.Date, SelectedValue.ShareNo, SelectedValue.UnitPrice, SelectedValue.NewInvestment, fReportLogger));
                if (!edited)
                {
                    _ = fReportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.EditingData, "Was not able to add or edit security data.");
                }
            }
        }

        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            _ = fReportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {SelectedName} updating data.");
            base.UpdateData(portfolio);
            if (SelectedName != null)
            {
                if (!portfolio.Exists(Account.Security, SelectedName))
                {
                    removeTab?.Invoke(this);
                    return;
                }

                SelectedSecurityData = DataStore.SecurityData(SelectedName);
            }
            else
            {
                SelectedSecurityData = null;
            }
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            UpdateData(portfolio, null);
        }
    }
}
