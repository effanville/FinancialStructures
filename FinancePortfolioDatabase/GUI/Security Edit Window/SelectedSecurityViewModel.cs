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

namespace FinanceWindowsViewModels
{
    internal class SelectedSecurityViewModel : TabViewModelBase<IPortfolio>
    {
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
                fSelectedSecurityData = value;
                OnPropertyChanged();
            }
        }

        internal SecurityDayData fOldSelectedValues;

        private readonly Action<Action<IPortfolio>> UpdateDataCallback;

        private readonly IReportLogger ReportLogger;
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogCreationService;

        public SelectedSecurityViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, IReportLogger reportLogger, IFileInteractionService fileService, IDialogCreationService dialogCreation, NameData selectedName)
            : base(selectedName != null ? selectedName.Company + "-" + selectedName.Name : "No-Name", portfolio)
        {
            fSelectedName = selectedName;
            DeleteValuationCommand = new RelayCommand(ExecuteDeleteValuation);
            AddCsvData = new RelayCommand(ExecuteAddCsvData);
            ExportCsvData = new RelayCommand(ExecuteExportCsvData);
            AddEditSecurityDataCommand = new RelayCommand<DataGridRowEditEndingEventArgs>(ExecuteAddEditSecData);
            SelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(ExecuteSelectionChanged);
            AddDefaultDataCommand = new RelayCommand<AddingNewItemEventArgs>(e => DataGrid_AddingNewItem(null, e));
            UpdateData(portfolio, null);
            UpdateDataCallback = updateData;
            ReportLogger = reportLogger;
            fFileService = fileService;
            fDialogCreationService = dialogCreation;
        }

        public ICommand DeleteValuationCommand
        {
            get;
        }

        private void ExecuteDeleteValuation()
        {
            if (fSelectedName != null && fOldSelectedValues != null)
            {
                UpdateDataCallback(programPortfolio => programPortfolio.TryDeleteData(Account.Security, fSelectedName, fOldSelectedValues.Date, ReportLogger));
            }
        }

        public ICommand AddCsvData
        {
            get;
        }

        private void ExecuteAddCsvData()
        {
            if (fSelectedName != null)
            {
                FileInteractionResult result = fFileService.OpenFile("csv", filter: "Csv Files|*.csv|All Files|*.*");
                List<object> outputs = null;
                bool exists = DataStore.TryGetSecurity(fSelectedName, out ISecurity security);
                if (result.Success != null && (bool)result.Success && exists)
                {
                    outputs = CsvReaderWriter.ReadFromCsv(security, result.FilePath, ReportLogger);
                }
                if (outputs != null)
                {
                    foreach (object objec in outputs)
                    {
                        if (objec is SecurityDayData view)
                        {
                            UpdateDataCallback(programPortfolio => programPortfolio.TryAddOrEditDataToSecurity(fSelectedName, view.Date, view.Date, view.ShareNo, view.UnitPrice, view.NewInvestment, ReportLogger));
                        }
                        else
                        {
                            _ = ReportLogger.LogUsefulWithStrings("Error", "StatisticsPage", "Have the wrong type of thing");
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
            if (fSelectedName != null)
            {
                FileInteractionResult result = fFileService.SaveFile("csv", string.Empty, DataStore.Directory, "Csv Files|*.csv|All Files|*.*");
                if (result.Success != null && (bool)result.Success)
                {
                    if (DataStore.TryGetSecurity(fSelectedName, out ISecurity security))
                    {
                        CsvReaderWriter.WriteToCSVFile(security, result.FilePath, ReportLogger);
                    }
                    else
                    {
                        _ = ReportLogger.LogWithStrings("Critical", "Error", "Saving", "Could not find security.");
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
            if (e.Source is DataGrid dg)
            {
                if (dg.CurrentItem != null)
                {
                    if (dg.CurrentItem is SecurityDayData data)
                    {
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
            if (fSelectedName != null)
            {
                var originRowData = e.Row.DataContext as SecurityDayData;
                bool edited = false;
                UpdateDataCallback(programPortfolio => programPortfolio.TryAddOrEditDataToSecurity(fSelectedName, fOldSelectedValues.Date, originRowData.Date, originRowData.ShareNo, originRowData.UnitPrice, originRowData.NewInvestment, ReportLogger));
                if (!edited)
                {
                    _ = ReportLogger.LogUsefulWithStrings("Error", "EditingData", "Was not able to add or edit security data.");
                }
            }
        }

        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
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
