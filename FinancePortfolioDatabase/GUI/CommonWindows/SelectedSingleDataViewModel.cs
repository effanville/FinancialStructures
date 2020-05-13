using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using StructureCommon.DataStructures;
using StructureCommon.FileAccess;
using StructureCommon.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UICommon.Commands;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace FinanceCommonViewModels
{
    internal class SelectedSingleDataViewModel : ViewModelBase<IPortfolio>
    {
        private AccountType TypeOfAccount;

        public override bool Closable
        {
            get
            {
                return true;
            }
        }

        private NameData_ChangeLogged fSelectedName;

        /// <summary>
        /// Name and Company data of the selected security in the list <see cref="AccountNames"/>
        /// </summary>
        public NameData_ChangeLogged SelectedName
        {
            get
            {
                return fSelectedName;
            }
            set
            {
                fSelectedName = value;
                OnPropertyChanged();
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
                fSelectedData = value;
                OnPropertyChanged();
            }
        }

        private DailyValuation fSelectedValues;
        private DailyValuation fOldSelectedValue;
        private int SelectedIndex;
        public DailyValuation SelectedValue
        {
            get
            {
                return fSelectedValues;
            }
            set
            {
                fSelectedValues = value;
                if (SelectedData != null)
                {
                    int index = SelectedData.IndexOf(value);
                    if (SelectedIndex != index)
                    {
                        SelectedIndex = index;
                        fOldSelectedValue = fSelectedValues?.Copy();
                    }
                }
                OnPropertyChanged();
            }
        }

        private readonly Action<Action<IPortfolio>> UpdateDataCallback;

        private readonly IReportLogger ReportLogger;
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogCreationService;

        public SelectedSingleDataViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, IReportLogger reportLogger, IFileInteractionService fileService, IDialogCreationService dialogCreation, NameData_ChangeLogged selectedName, AccountType accountType)
            : base(selectedName != null ? selectedName.Company + "-" + selectedName.Name : "No-Name", portfolio)
        {
            SelectedName = selectedName;
            TypeOfAccount = accountType;
            UpdateData(portfolio);

            EditDataCommand = new RelayCommand(ExecuteEditDataCommand);
            DeleteValuationCommand = new RelayCommand(ExecuteDeleteValuation);
            AddCsvData = new RelayCommand(ExecuteAddCsvData);
            ExportCsvData = new RelayCommand(ExecuteExportCsvData);
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            fFileService = fileService;
            fDialogCreationService = dialogCreation;
            fOldSelectedValue = SelectedValue?.Copy();
        }

        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            base.UpdateData(portfolio);
            if (SelectedName != null)
            {
                if (!portfolio.NameData(TypeOfAccount).Exists(name => name.IsEqualTo(SelectedName)))
                {
                    removeTab?.Invoke(this);
                    return;
                }

                SelectedData = DataStore.NumberData(TypeOfAccount, SelectedName, ReportLogger);
                SelectLatestValue();
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


        public ICommand EditDataCommand
        {
            get; set;
        }

        private void ExecuteEditDataCommand()
        {
            if (SelectedName != null)
            {
                if (DataStore.NumberData(TypeOfAccount, SelectedName, ReportLogger).Count() != SelectedData.Count)
                {
                    UpdateDataCallback(programPortfolio => programPortfolio.TryAddData(TypeOfAccount, SelectedName, SelectedValue, ReportLogger));
                    SelectedName.NewValue = false;
                }
                else
                {
                    bool edited = false;
                    UpdateDataCallback(programPortfolio => edited = programPortfolio.TryEditData(TypeOfAccount, SelectedName, fOldSelectedValue, SelectedValue, ReportLogger));

                    if (!edited)
                    {
                        _ = ReportLogger.LogWithStrings("Critical", "Error", "EditingData", "Was not able to edit data.");
                    }
                }
            }
        }

        public ICommand DeleteValuationCommand
        {
            get;
        }

        private void ExecuteDeleteValuation()
        {
            if (SelectedName != null)
            {
                UpdateDataCallback(programPortfolio => programPortfolio.TryDeleteData(TypeOfAccount, SelectedName, SelectedValue, ReportLogger));
            }
            else
            {
                _ = ReportLogger.LogWithStrings("Critical", "Error", "DeletingData", "No Account was selected when trying to delete data.");
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
                bool exists = DataStore.TryGetAccount(TypeOfAccount, fSelectedName, out var account);
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
                            UpdateDataCallback(programPortfolio => programPortfolio.TryAddData(TypeOfAccount, SelectedName, view, ReportLogger));
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
                var result = fFileService.SaveFile("csv", string.Empty, DataStore.Directory, "Csv Files|*.csv|All Files|*.*");
                if (result.Success != null && (bool)result.Success)
                {
                    if (DataStore.TryGetAccount(TypeOfAccount, fSelectedName, out var account))
                    {
                        CsvReaderWriter.WriteToCSVFile(account, result.FilePath, ReportLogger);
                    }
                    else
                    {
                        _ = ReportLogger.LogWithStrings("Critical", "Error", "Saving", "Could not find security.");
                    }
                }
            }
        }

        private void SelectLatestValue()
        {
            if (SelectedData != null && SelectedData.Count > 0)
            {
                SelectedValue = SelectedData[SelectedData.Count - 1];
            }
        }
    }
}
