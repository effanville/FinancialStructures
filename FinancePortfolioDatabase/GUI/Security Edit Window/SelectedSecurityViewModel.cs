using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using StructureCommon.DataStructures;
using StructureCommon.FileAccess;
using StructureCommon.Reporting;
using System;
using System.Collections.Generic;
using System.Windows.Input;
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

        private SecurityDayData fSelectedValues;
        private SecurityDayData fOldSelectedValues;
        private int selectedIndex;
        public SecurityDayData selectedValues
        {
            get
            {
                return fSelectedValues;
            }
            set
            {
                fSelectedValues = value;
                if (SelectedSecurityData != null)
                {
                    int index = SelectedSecurityData.IndexOf(value);
                    if (selectedIndex != index)
                    {
                        selectedIndex = index;
                        fOldSelectedValues = fSelectedValues?.Copy();
                    }
                }

                OnPropertyChanged();
            }
        }

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
            AddEditSecurityDataCommand = new RelayCommand(ExecuteAddEditSecData);
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
            if (fSelectedName != null && fSelectedValues != null)
            {
                UpdateDataCallback(programPortfolio => programPortfolio.TryDeleteData(AccountType.Security, fSelectedName, new DailyValuation(fSelectedValues.Date, 0.0), ReportLogger));
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
                var result = fFileService.OpenFile("csv", filter: "Csv Files|*.csv|All Files|*.*");
                List<object> outputs = null;
                bool exists = DataStore.TryGetSecurity(fSelectedName, out ISecurity security);
                if (result.Success != null && (bool)result.Success && exists)
                {
                    outputs = CsvReaderWriter.ReadFromCsv(security, result.FilePath, ReportLogger);
                }
                if (outputs != null)
                {
                    foreach (var objec in outputs)
                    {
                        if (objec is SecurityDayData view)
                        {
                            UpdateDataCallback(programPortfolio => programPortfolio.TryAddDataToSecurity(fSelectedName, view.Date, view.ShareNo, view.UnitPrice, view.NewInvestment, ReportLogger));
                        }
                        else
                        {
                            ReportLogger.LogUsefulWithStrings("Error", "StatisticsPage", "Have the wrong type of thing");
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
                var result = fFileService.SaveFile("csv", string.Empty, DataStore.Directory, "Csv Files|*.csv|All Files|*.*");
                if (result.Success != null && (bool)result.Success)
                {
                    if (DataStore.TryGetSecurity(fSelectedName, out var security))
                    {
                        CsvReaderWriter.WriteToCSVFile(security, result.FilePath, ReportLogger);
                    }
                    else
                    {
                        ReportLogger.LogWithStrings("Critical", "Error", "Saving", "Could not find security.");
                    }
                }
            }
        }

        public ICommand AddEditSecurityDataCommand
        {
            get; set;
        }

        private void ExecuteAddEditSecData()
        {
            if (fSelectedName != null)
            {
                DataStore.TryGetSecurity(fSelectedName, out var desired);
                if (desired.Count() != SelectedSecurityData.Count)
                {
                    UpdateDataCallback(programPortfolio => programPortfolio.TryAddDataToSecurity(fSelectedName, selectedValues.Date, selectedValues.ShareNo, selectedValues.UnitPrice, selectedValues.NewInvestment, ReportLogger));
                }
                else
                {
                    bool edited = false;
                    UpdateDataCallback(programPortfolio => edited = programPortfolio.TryEditSecurityData(fSelectedName, fOldSelectedValues.Date, selectedValues.Date, selectedValues.ShareNo, selectedValues.UnitPrice, selectedValues.NewInvestment, ReportLogger));

                    if (!edited)
                    {
                        ReportLogger.LogUsefulWithStrings("Error", "EditingData", "Was not able to edit security data.");
                    }
                }
            }
        }

        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            base.UpdateData(portfolio);
            if (fSelectedName != null)
            {
                if (!portfolio.Exists(AccountType.Security, fSelectedName))
                {
                    removeTab?.Invoke(this);
                    return;
                }

                SelectedSecurityData = DataStore.SecurityData(fSelectedName);
                SelectLatestValue();
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

        private void SelectLatestValue()
        {
            if (SelectedSecurityData != null && SelectedSecurityData.Count > 0)
            {
                selectedValues = SelectedSecurityData[SelectedSecurityData.Count - 1];
                fOldSelectedValues = selectedValues.Copy();
            }
        }
    }
}
