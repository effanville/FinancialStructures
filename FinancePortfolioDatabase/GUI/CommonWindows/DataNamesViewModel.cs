using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using UICommon.Commands;
using UICommon.ViewModelBases;

namespace FinanceCommonViewModels
{
    /// <summary>
    /// Data store behind view for a list of names and associated update name methods.
    /// </summary>
    internal class DataNamesViewModel : ViewModelBase<IPortfolio>
    {
        /// <summary>
        /// List of names preceding any possible edit.
        /// </summary>
        private List<NameCompDate> fPreEditNames = new List<NameCompDate>();

        /// <summary>
        /// Backing field for <see cref="DataNames"/>.
        /// </summary>
        private List<NameCompDate> fDataNames = new List<NameCompDate>();

        /// <summary>
        /// Name data of the names to be displayed in this view.
        /// </summary>
        public List<NameCompDate> DataNames
        {
            get { return fDataNames; }
            set { fDataNames = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Backing field for <see cref="SelectedName"/>.
        /// </summary>
        private NameData_ChangeLogged fSelectedName;

        /// <summary>
        /// Name and Company data of the selected account in the list <see cref="DataNames"/>
        /// </summary>
        public NameData_ChangeLogged SelectedName
        {
            get { return fSelectedName; }
            set { fSelectedName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Function which updates the main data store.
        /// </summary>
        private readonly Action<Action<IPortfolio>> UpdateDataCallback;

        /// <summary>
        /// Logs any possible issues in the routines here back to the user.
        /// </summary>
        private readonly IReportLogger ReportLogger;

        /// <summary>
        /// Collection of functions for editing the Names stored here.
        /// </summary>
        private readonly EditMethods editMethods;

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public DataNamesViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, IReportLogger reportLogger, Action<object> loadSelectedData, EditMethods updateMethods)
            : base("Accounts", portfolio, loadSelectedData)
        {
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            editMethods = updateMethods;
            DataNames = (List<NameCompDate>)editMethods.ExecuteFunction(FunctionType.NameUpdate, portfolio).Result;
            DataNames.Sort();
            fPreEditNames = (List<NameCompDate>)editMethods.ExecuteFunction(FunctionType.NameUpdate, portfolio).Result;
            fPreEditNames.Sort();

            CreateCommand = new RelayCommand(ExecuteCreateEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete);
            DownloadCommand = new RelayCommand(ExecuteDownloadCommand);
            OpenTabCommand = new RelayCommand(OpenTab);
        }

        /// <summary>
        /// Command that opens a tab associated to the selected entry.
        /// </summary>
        public ICommand OpenTabCommand { get; }
        private void OpenTab()
        {
            LoadSelectedTab(SelectedName);
        }

        /// <summary>
        /// Updates the data in this view model from the given portfolio.
        /// </summary>
        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            base.UpdateData(portfolio);
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

        /// <summary>
        /// Updates the data in this view model from the given portfolio.
        /// </summary>
        public override void UpdateData(IPortfolio portfolio)
        {
            UpdateData(portfolio, null);
        }

        /// <summary>
        /// Downloads the latest data for the selected entry.
        /// </summary>
        public ICommand DownloadCommand { get; }
        private void ExecuteDownloadCommand()
        {
            if (SelectedName != null)
            {
                NameData names = SelectedName as NameData;
                UpdateDataCallback(async programPortfolio => await editMethods.ExecuteFunction(FunctionType.Download, programPortfolio, names, ReportLogger).ConfigureAwait(false));
            }
        }

        /// <summary>
        /// Adds a new entry if the view has more than the repository, or edits an entry if these are the same.
        /// </summary>
        public ICommand CreateCommand { get; set; }
        private void ExecuteCreateEdit()
        {
            if (((List<NameCompDate>)editMethods.ExecuteFunction(FunctionType.NameUpdate, DataStore).Result).Count != DataNames.Count)
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

        /// <summary>
        /// Deletes the selected entry.
        /// </summary>
        public ICommand DeleteCommand { get; }
        private void ExecuteDelete()
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
