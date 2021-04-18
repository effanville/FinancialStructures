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

        private readonly NameData fSelectedName;

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
                _ = fReportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {fSelectedName} updated selected data.");
            }
        }

        internal SecurityDayData fOldSelectedValues;

        private readonly Action<Action<IPortfolio>> UpdateDataCallback;

        private readonly IReportLogger fReportLogger;
        private readonly UiGlobals fUiGlobals;

        public SelectedSecurityViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, IReportLogger reportLogger, UiGlobals globals, NameData selectedName)
            : base(selectedName != null ? selectedName.ToString() : "No-Name", portfolio)
        {
            fReportLogger = reportLogger;
            fUiGlobals = globals;
            fSelectedName = selectedName;
            DeleteValuationCommand = new RelayCommand(ExecuteDeleteValuation);
            AddCsvData = new RelayCommand(ExecuteAddCsvData);
            ExportCsvData = new RelayCommand(ExecuteExportCsvData);
            AddEditSecurityDataCommand = new RelayCommand<DataGridRowEditEndingEventArgs>(ExecuteAddEditSecData);
            SelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(ExecuteSelectionChanged);
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
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {fSelectedName} deleting valuation {fOldSelectedValues}.");
            if (fSelectedName != null && fOldSelectedValues != null)
            {
                UpdateDataCallback(programPortfolio => programPortfolio.TryDeleteData(Account.Security, fSelectedName, fOldSelectedValues.Date, fReportLogger));
            }
        }

        public ICommand AddCsvData
        {
            get;
        }

        private void ExecuteAddCsvData()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {fSelectedName} adding data from csv.");
            if (fSelectedName != null)
            {
                FileInteractionResult result = fUiGlobals.FileInteractionService.OpenFile("csv", filter: "Csv Files|*.csv|All Files|*.*");
                List<object> outputs = null;
                bool exists = DataStore.TryGetAccount(Account.Security, fSelectedName, out IValueList account);
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
                            UpdateDataCallback(programPortfolio => programPortfolio.TryAddOrEditDataToSecurity(fSelectedName, view.Date, view.Date, view.ShareNo, view.UnitPrice, view.NewInvestment, fReportLogger));
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
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {fSelectedName} exporting data to csv.");
            if (fSelectedName != null)
            {
                FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile("csv", string.Empty, DataStore.Directory, "Csv Files|*.csv|All Files|*.*");
                if (result.Success != null && (bool)result.Success)
                {
                    if (DataStore.TryGetAccount(Account.Security, fSelectedName, out IValueList account))
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
        private void ExecuteSelectionChanged(SelectionChangedEventArgs e)
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {fSelectedName} selection changed fired.");
            if (e.Source is DataGrid dg)
            {
                if (dg.CurrentItem != null)
                {
                    if (dg.CurrentItem is SecurityDayData data)
                    {
                        _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {fSelectedName} updated selected item from {fOldSelectedValues} to {data}.");
                        fOldSelectedValues = data.Copy();
                    }
                }
            }
        }

        public ICommand AddEditSecurityDataCommand
        {
            get;
            set;
        }

        private void ExecuteAddEditSecData(DataGridRowEditEndingEventArgs e)
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {fSelectedName}adding valuation called.");
            if (fSelectedName != null)
            {
                var originRowData = e.Row.DataContext as SecurityDayData;
                bool edited = false;
                _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {fSelectedName} adding valuation {originRowData}.");
                UpdateDataCallback(programPortfolio => programPortfolio.TryAddOrEditDataToSecurity(fSelectedName, fOldSelectedValues.Date, originRowData.Date, originRowData.ShareNo, originRowData.UnitPrice, originRowData.NewInvestment, fReportLogger));
                if (!edited)
                {
                    _ = fReportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, "Was not able to add or edit security data.");
                }
            }
        }

        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            _ = fReportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Selected Security {fSelectedName} updating data.");
            base.UpdateData(portfolio);
            if (fSelectedName != null)
            {
                if (!portfolio.Exists(Account.Security, fSelectedName))
                {
                    removeTab?.Invoke(this);
                    return;
                }

                SelectedSecurityData = DataStore.SecurityData(fSelectedName);
            }
            else
            {
                SelectedSecurityData = null;
            }
        }
    }
}
