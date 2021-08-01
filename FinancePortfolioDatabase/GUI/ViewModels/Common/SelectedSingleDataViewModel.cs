using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using Common.Structure.DataStructures;
using Common.Structure.FileAccess;
using Common.Structure.Reporting;
using Common.UI.Commands;
using Common.UI.Services;
using Common.UI.ViewModelBases;
using Common.UI;

namespace FinancePortfolioDatabase.GUI.ViewModels.Common
{
    public class SelectedSingleDataViewModel : TabViewModelBase<IPortfolio>
    {
        private readonly UiGlobals fUiGlobals;
        private readonly Account TypeOfAccount;

        public override bool Closable
        {
            get
            {
                return true;
            }
        }

        private NameData fSelectedName;

        /// <summary>
        /// Name and Company data of the selected security in the list
        /// </summary>
        public NameData SelectedName
        {
            get
            {
                return fSelectedName;
            }
            set
            {
                SetAndNotify(ref fSelectedName, value, nameof(SelectedName));
            }
        }

        private List<DailyValuation> fSelectedData;
        public List<DailyValuation> SelectedData
        {
            get
            {
                return fSelectedData;
            }
            set
            {
                SetAndNotify(ref fSelectedData, value, nameof(SelectedData));
            }
        }

        internal DailyValuation fOldSelectedValue;
        internal DailyValuation SelectedValue;

        private readonly Action<Action<IPortfolio>> UpdateDataCallback;

        private readonly IReportLogger ReportLogger;

        public SelectedSingleDataViewModel(
            IPortfolio portfolio,
            Action<Action<IPortfolio>> updateDataCallback,
            UiGlobals globals,
            NameData selectedName,
            Account accountType)
            : base(selectedName != null ? selectedName.ToString() : "No-Name", portfolio)
        {
            fUiGlobals = globals;
            ReportLogger = fUiGlobals.ReportLogger;
            UpdateDataCallback = updateDataCallback;
            SelectedName = selectedName;
            TypeOfAccount = accountType;
            UpdateData(portfolio);

            PreEditCommand = new RelayCommand(ExecutePreEdit);
            EditDataCommand = new RelayCommand(ExecuteEditDataCommand);
            DeleteValuationCommand = new RelayCommand(ExecuteDeleteValuation);
            SelectionChangedCommand = new RelayCommand<object>(ExecuteSelectionChanged);
            AddDefaultDataCommand = new RelayCommand<AddingNewItemEventArgs>(e => DataGrid_AddingNewItem(null, e));
            AddCsvData = new RelayCommand(ExecuteAddCsvData);
            ExportCsvData = new RelayCommand(ExecuteExportCsvData);

        }

        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            base.UpdateData(portfolio);
            if (SelectedName != null)
            {
                if (!portfolio.Exists(TypeOfAccount, SelectedName))
                {
                    removeTab?.Invoke(this);
                    return;
                }

                SelectedData = DataStore.NumberData(TypeOfAccount, SelectedName, ReportLogger).ToList();
            }
            else
            {
                SelectedData = null;
            }
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            UpdateData(portfolio, null);
        }

        public ICommand AddDefaultDataCommand
        {
            get;
            set;
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            if (SelectedData != null && SelectedData.Any())
            {
                double latestValue = SelectedData.Last().Value;
                e.NewItem = new DailyValuation()
                {
                    Day = DateTime.Today,
                    Value = latestValue
                };
            }
        }

        public ICommand SelectionChangedCommand
        {
            get;
            set;
        }
        private void ExecuteSelectionChanged(object obj)
        {
            if (SelectedData != null && obj is DailyValuation data)
            {
                SelectedValue = data;
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

        public ICommand EditDataCommand
        {
            get;
            set;
        }

        private void ExecuteEditDataCommand()
        {
            if (fSelectedName != null)
            {
                bool edited = false;
                UpdateDataCallback(programPortfolio => programPortfolio.TryAddOrEditData(TypeOfAccount, fSelectedName, fOldSelectedValue, SelectedValue, ReportLogger));
                if (!edited)
                {
                    _ = ReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.EditingData, "Was not able to add or edit data.");
                }
            }
        }

        public ICommand DeleteValuationCommand
        {
            get;
        }

        private void ExecuteDeleteValuation()
        {
            if (fSelectedName != null && SelectedValue != null)
            {
                UpdateDataCallback(programPortfolio => programPortfolio.TryDeleteData(TypeOfAccount, fSelectedName, SelectedValue.Day, ReportLogger));
            }
            else
            {
                _ = ReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, "No Account was selected when trying to delete data.");
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
                FileInteractionResult result = fUiGlobals.FileInteractionService.OpenFile("csv", filter: "Csv Files|*.csv|All Files|*.*");
                List<object> outputs = null;
                bool exists = DataStore.TryGetAccount(TypeOfAccount, fSelectedName, out IValueList account);
                if (result.Success != null && (bool)result.Success && exists)
                {
                    outputs = CsvReaderWriter.ReadFromCsv(account, result.FilePath, ReportLogger);
                }
                if (outputs != null)
                {
                    foreach (object objec in outputs)
                    {
                        if (objec is DailyValuation view)
                        {
                            UpdateDataCallback(programPortfolio => programPortfolio.TryAddOrEditData(TypeOfAccount, SelectedName, view, view, ReportLogger));
                        }
                        else
                        {
                            _ = ReportLogger.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, "Have the wrong type of thing");
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
                FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile("csv", string.Empty, DataStore.Directory(fUiGlobals.CurrentFileSystem), "Csv Files|*.csv|All Files|*.*");
                if (result.Success != null && (bool)result.Success)
                {
                    if (DataStore.TryGetAccount(TypeOfAccount, fSelectedName, out IValueList account))
                    {
                        CsvReaderWriter.WriteToCSVFile(account, result.FilePath, ReportLogger);
                    }
                    else
                    {
                        _ = ReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Saving, "Could not find security.");
                    }
                }
            }
        }
    }
}
