using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using GUISupport;
using SavingClasses;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinanceCommonViewModels
{
    internal class DataNamesViewModel : ViewModelBase
    {
        private Portfolio Portfolio;

        private List<Sector> Sectors;

        private List<NameData> fPreEditNames = new List<NameData>();

        private List<NameData> fDataNames = new List<NameData>();

        /// <summary>
        /// Name and Company data of Funds in database for view.
        /// </summary>
        public List<NameData> DataNames
        {
            get { return fDataNames; }
            set { fDataNames = value; OnPropertyChanged(); }
        }

        private NameData fSelectedName;

        /// <summary>
        /// Name and Company data of the selected security in the list <see cref="DataNames"/>
        /// </summary>
        public NameData SelectedName
        {
            get { return fSelectedName; }
            set { fSelectedName = value; OnPropertyChanged(); }
        }

        private readonly Action<Action<AllData>> UpdateDataCallback;
        private readonly Action<string, string, string> ReportLogger;
        private readonly EditMethods editMethods;

        public DataNamesViewModel(Portfolio portfolio, List<Sector> sectors, Action<Action<AllData>> updateDataCallback, Action<string, string, string> reportLogger, Action<NameData> loadSelectedData, EditMethods updateMethods)
            : base("Accounts", loadSelectedData)
        {
            Portfolio = portfolio;
            Sectors = sectors;
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            editMethods = updateMethods;

            CreateCommand = new BasicCommand(ExecuteCreateEdit);
            DeleteCommand = new BasicCommand(ExecuteDelete);
            DownloadCommand = new BasicCommand(ExecuteDownloadCommand);
        }

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors, Action<object> removeTab)
        {
            Portfolio = portfolio;
            Sectors = sectors;
            var currentSelectedName = SelectedName;
            DataNames = (List<NameData>)editMethods.ExecuteFunction(FunctionType.NameUpdate, portfolio, sectors).Result;
            DataNames.Sort();
            fPreEditNames = (List<NameData>)editMethods.ExecuteFunction(FunctionType.NameUpdate, portfolio, sectors).Result;
            fPreEditNames.Sort();

            for (int i = 0; i < DataNames.Count; i++)
            {
                if (DataNames[i].CompareTo(currentSelectedName) == 0)
                {
                    SelectedName = DataNames[i];
                    return;
                }
            }
        }

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
        {
            UpdateData(portfolio, sectors, null);
        }

        public ICommand DownloadCommand { get; }
        private void ExecuteDownloadCommand(Object obj)
        {
            if (SelectedName != null)
            {
                UpdateDataCallback(async alldata => await editMethods.ExecuteFunction(FunctionType.Download, alldata.MyFunds, alldata.myBenchMarks, SelectedName, ReportLogger).ConfigureAwait(false));
            }
        }

        public ICommand CreateCommand { get; set; }
        private void ExecuteCreateEdit(Object obj)
        {
            if (((List<NameData>)editMethods.ExecuteFunction(FunctionType.NameUpdate, Portfolio, Sectors).Result).Count != DataNames.Count)
            {
                bool edited = false;
                if (SelectedName.NewValue)
                {
                    UpdateDataCallback(alldata => editMethods.ExecuteFunction(FunctionType.Create, alldata.MyFunds, alldata.myBenchMarks, SelectedName, ReportLogger).Wait());
                    if (SelectedName != null)
                    {
                        SelectedName.NewValue = false;
                    }
                }
                if (!edited)
                {
                    ReportLogger("Error", "AddingData", "No Name provided on creation.");
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
                        UpdateDataCallback(alldata => editMethods.ExecuteFunction(FunctionType.Edit, alldata.MyFunds, alldata.myBenchMarks, fPreEditNames[i], name, ReportLogger).Wait());
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    ReportLogger("Error", "EditingData", "Was not able to edit desired.");
                }
            }
        }

        public ICommand DeleteCommand { get; }
        private void ExecuteDelete(Object obj)
        {
            if (SelectedName.Name != null)
            {
                UpdateDataCallback(alldata => editMethods.ExecuteFunction(FunctionType.Delete, alldata.MyFunds, alldata.myBenchMarks, SelectedName, ReportLogger).Wait());
            }
            else
            {
                ReportLogger("Error", "DeletingData", "Nothing was selected when trying to delete.");
            }
        }
    }
}
