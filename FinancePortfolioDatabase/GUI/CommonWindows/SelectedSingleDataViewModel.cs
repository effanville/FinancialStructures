using FinancialStructures.DataReader;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Reporting;
using GUISupport;
using GUISupport.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FinanceCommonViewModels
{
    internal class SelectedSingleDataViewModel : ViewModelBase
    {
        private AccountType TypeOfAccount;
        private IPortfolio Portfolio;

        public override bool Closable { get { return true; } }

        private NameData_ChangeLogged fSelectedName;

        /// <summary>
        /// Name and Company data of the selected security in the list <see cref="AccountNames"/>
        /// </summary>
        public NameData_ChangeLogged SelectedName
        {
            get { return fSelectedName; }
            set { fSelectedName = value; OnPropertyChanged(); }
        }

        private List<DayValue_ChangeLogged> fSelectedData;
        public List<DayValue_ChangeLogged> SelectedData
        {
            get { return fSelectedData; }
            set { fSelectedData = value; OnPropertyChanged(); }
        }

        private DayValue_ChangeLogged fSelectedValues;
        private DayValue_ChangeLogged fOldSelectedValue;
        private int SelectedIndex;
        public DayValue_ChangeLogged SelectedValue
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

        private readonly EditMethods EditMethods;

        public SelectedSingleDataViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, IReportLogger reportLogger, IFileInteractionService fileService, IDialogCreationService dialogCreation, EditMethods editMethods, NameData_ChangeLogged selectedName, AccountType accountType)
            : base(selectedName != null ? selectedName.Company + "-" + selectedName.Name : "No-Name")
        {
            SelectedName = selectedName;
            EditMethods = editMethods;
            TypeOfAccount = accountType;
            UpdateData(portfolio);

            EditDataCommand = new BasicCommand(ExecuteEditDataCommand);
            DeleteValuationCommand = new BasicCommand(ExecuteDeleteValuation);
            AddCsvData = new BasicCommand(ExecuteAddCsvData);
            ExportCsvData = new BasicCommand(ExecuteExportCsvData);
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            fFileService = fileService;
            fDialogCreationService = dialogCreation;
            fOldSelectedValue = SelectedValue?.Copy();
        }

        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            Portfolio = portfolio;
            if (SelectedName != null)
            {
                if (!((List<NameCompDate>)EditMethods.ExecuteFunction(FunctionType.NameUpdate, Portfolio).Result).Exists(name => name.IsEqualTo(SelectedName)))
                {
                    removeTab?.Invoke(this);
                    return;
                }

                SelectedData = (List<DayValue_ChangeLogged>)EditMethods.ExecuteFunction(FunctionType.SelectData, Portfolio, SelectedName, ReportLogger).Result;
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


        public ICommand EditDataCommand { get; set; }

        private void ExecuteEditDataCommand(Object obj)
        {
            if (SelectedName != null)
            {
                if (((List<DayValue_ChangeLogged>)EditMethods.ExecuteFunction(FunctionType.SelectData, Portfolio, SelectedName, ReportLogger).Result).Count() != SelectedData.Count)
                {
                    UpdateDataCallback(programPortfolio => EditMethods.ExecuteFunction(FunctionType.AddData, programPortfolio, SelectedName, SelectedValue, ReportLogger).Wait());
                    SelectedName.NewValue = false;
                }
                else
                {
                    bool edited = false;
                    for (int i = 0; i < SelectedData.Count; i++)
                    {
                        var name = SelectedData[i];

                        if (name.NewValue)
                        {
                            edited = true;
                            name.NewValue = false;
                            UpdateDataCallback(programPortfolio => EditMethods.ExecuteFunction(FunctionType.EditData, programPortfolio, SelectedName, fOldSelectedValue, SelectedValue, ReportLogger).Wait());
                        }
                    }

                    if (!edited)
                    {
                        ReportLogger.LogWithStrings("Critical", "Error", "EditingData", "Was not able to edit data.");
                    }
                }
            }
        }

        public ICommand DeleteValuationCommand { get; }

        private void ExecuteDeleteValuation(Object obj)
        {
            if (SelectedName != null)
            {
                UpdateDataCallback(programPortfolio => EditMethods.ExecuteFunction(FunctionType.DeleteData, programPortfolio, SelectedName, SelectedValue, ReportLogger).Wait());
            }
            else
            {
                ReportLogger.LogWithStrings("Critical", "Error", "DeletingData", "No Account was selected when trying to delete data.");
            }
        }

        public ICommand AddCsvData { get; }

        private void ExecuteAddCsvData(Object obj)
        {
            if (fSelectedName != null)
            {
                var result = fFileService.OpenFile("csv", filter: "Csv Files|*.csv|All Files|*.*");
                List<object> outputs = null;

                if (result.Success != null && (bool)result.Success)
                {
                    outputs = CsvDataRead.ReadFromCsv(result.FilePath, AccountType.Security, ReportLogger);
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

        public ICommand ExportCsvData { get; }

        private void ExecuteExportCsvData(Object obj)
        {
            if (fSelectedName != null)
            {
                var result = fFileService.SaveFile("csv", string.Empty, Portfolio.Directory, "Csv Files|*.csv|All Files|*.*");
                if (result.Success != null && (bool)result.Success)
                {
                    if (Portfolio.TryGetAccount(TypeOfAccount, fSelectedName, out var security))
                    {
                        CsvDataRead.WriteToCSVFile(result.FilePath, AccountType.Security, security, ReportLogger);
                    }
                    else
                    {
                        ReportLogger.LogWithStrings("Critical", "Error", "Saving", "Could not find security.");
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
