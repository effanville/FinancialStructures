using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.Reporting;
using GUISupport;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinanceCommonViewModels
{
    internal class DataNamesViewModel : ViewModelBase
    {
        private IPortfolio Portfolio;

        private List<NameCompDate> fPreEditNames = new List<NameCompDate>();

        private List<NameCompDate> fDataNames = new List<NameCompDate>();

        /// <summary>
        /// Name and Company data of Funds in database for view.
        /// </summary>
        public List<NameCompDate> DataNames
        {
            get { return fDataNames; }
            set { fDataNames = value; OnPropertyChanged(); }
        }

        private NameData_ChangeLogged fSelectedName;

        /// <summary>
        /// Name and Company data of the selected security in the list <see cref="DataNames"/>
        /// </summary>
        public NameData_ChangeLogged SelectedName
        {
            get { return fSelectedName; }
            set { fSelectedName = value; OnPropertyChanged(); }
        }

        private readonly Action<Action<IPortfolio>> UpdateDataCallback;
        private readonly IReportLogger ReportLogger;
        private readonly EditMethods editMethods;

        public DataNamesViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, IReportLogger reportLogger, Action<NameData_ChangeLogged> loadSelectedData, EditMethods updateMethods)
            : base("Accounts", loadSelectedData)
        {
            Portfolio = portfolio;
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            editMethods = updateMethods;

            CreateCommand = new BasicCommand(ExecuteCreateEdit);
            DeleteCommand = new BasicCommand(ExecuteDelete);
            DownloadCommand = new BasicCommand(ExecuteDownloadCommand);
        }

        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            Portfolio = portfolio;
            var currentSelectedName = SelectedName;
            DataNames = (List<NameCompDate>)editMethods.ExecuteFunction(FunctionType.NameUpdate, portfolio).Result;
            DataNames.Sort();
            fPreEditNames = (List<NameCompDate>)editMethods.ExecuteFunction(FunctionType.NameUpdate, portfolio).Result;
            fPreEditNames.Sort();

            for (int i = 0; i < DataNames.Count; i++)
            {
                if (DataNames[i].IsEqualTo(currentSelectedName))
                {
                    SelectedName = DataNames[i];
                    return;
                }
            }
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            UpdateData(portfolio, null);
        }

        public ICommand DownloadCommand { get; }
        private void ExecuteDownloadCommand(Object obj)
        {
            if (SelectedName != null)
            {
                NameData names = SelectedName as NameData;
                UpdateDataCallback(async programPortfolio => await editMethods.ExecuteFunction(FunctionType.Download, programPortfolio, names, ReportLogger).ConfigureAwait(false));
            }
        }

        public ICommand CreateCommand { get; set; }
        private void ExecuteCreateEdit(Object obj)
        {
            if (((List<NameCompDate>)editMethods.ExecuteFunction(FunctionType.NameUpdate, Portfolio).Result).Count != DataNames.Count)
            {
                bool edited = false;
                if (SelectedName.NewValue)
                {
                    NameData name_add = new NameData(SelectedName.Company, SelectedName.Name, SelectedName.Currency, SelectedName.Url, SelectedName.Sectors);
                    UpdateDataCallback(programPortfolio => editMethods.ExecuteFunction(FunctionType.Create, programPortfolio, name_add, ReportLogger).Wait());
                    edited = true;
                    if (SelectedName != null)
                    {
                        SelectedName.NewValue = false;
                    }
                }
                if (!edited)
                {
                    ReportLogger.LogWithStrings("Critical", "Error", "AddingData", "No Name provided on creation.");
                }
            }
            else
            {
                // maybe fired from editing stuff. Try that
                bool edited = false;
                for (int i = 0; i < DataNames.Count; i++)
                {
                    var name = DataNames[i];

                    if (name.NewValue && (!string.IsNullOrEmpty(name.Name) || !string.IsNullOrEmpty(name.Company)))
                    {
                        edited = true;
                        NameData name_add = new NameData(name.Company, name.Name, name.Currency, name.Url, name.Sectors);
                        UpdateDataCallback(programPortfolio => editMethods.ExecuteFunction(FunctionType.Edit, programPortfolio, fPreEditNames[i], name_add, ReportLogger).Wait());
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    ReportLogger.LogWithStrings("Critical", "Error", "EditingData", "Was not able to edit desired.");
                }
            }
        }

        public ICommand DeleteCommand { get; }
        private void ExecuteDelete(Object obj)
        {
            if (SelectedName.Name != null)
            {
                UpdateDataCallback(programPortfolio => editMethods.ExecuteFunction(FunctionType.Delete, programPortfolio, SelectedName, ReportLogger).Wait());
            }
            else
            {
                ReportLogger.LogWithStrings("Critical", "Error", "DeletingData", "Nothing was selected when trying to delete.");
            }
        }
    }
}
