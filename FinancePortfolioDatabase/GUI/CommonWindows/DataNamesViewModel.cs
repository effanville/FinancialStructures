using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
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
        Action<ErrorReports> UpdateReports;

        public string Header { get; set; } = "Accounts";
        public bool Closable { get { return false; } }

        public ICommand DownloadCommand { get; }

        public ICommand CreateCommand { get; set; }

        public ICommand DeleteCommand { get; }

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

        private void ExecuteDownloadCommand(Object obj)
        {
            var reports = new ErrorReports();
            if (SelectedName != null)
            {
                UpdateDataCallback(async alldata => await editMethods.ExecuteFunction(FunctionType.Download, alldata.MyFunds, alldata.myBenchMarks, SelectedName, UpdateReports, reports).ConfigureAwait(false));
            }
            if (reports.Any())
            {
                UpdateReports(reports);
            }
        }

        private void ExecuteCreateEdit(Object obj)
        {
            var reports = new ErrorReports();
            if (((List<NameData>)editMethods.ExecuteFunction(FunctionType.NameUpdate, Portfolio, Sectors).Result).Count != DataNames.Count)
            {
                bool edited = false;
                if (SelectedName.NewValue)
                {
                    UpdateDataCallback(alldata => editMethods.ExecuteFunction(FunctionType.Create, alldata.MyFunds, alldata.myBenchMarks, SelectedName, reports).Wait());
                    SelectedName.NewValue = false;
                }
                if (!edited)
                {
                    reports.AddError("No Name provided on creation.", Location.AddingData);
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
                        UpdateDataCallback(alldata => editMethods.ExecuteFunction(FunctionType.Edit, alldata.MyFunds, alldata.myBenchMarks, fPreEditNames[i], name, reports).Wait());
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    reports.AddError("Was not able to edit desired.", Location.EditingData);
                }
            }

            if (reports.Any())
            {
                UpdateReports(reports);
            }
        }

        private void ExecuteDelete(Object obj)
        {
            var reports = new ErrorReports();
            if (SelectedName.Name != null)
            {
                UpdateDataCallback(alldata => editMethods.ExecuteFunction(FunctionType.Delete, alldata.MyFunds, alldata.myBenchMarks, SelectedName, reports).Wait());
            }
            else
            {
                reports.AddError("Nothing was selected when trying to delete.", Location.DeletingData);
            }
            if (reports.Any())
            {
                UpdateReports(reports);
            }
        }

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
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

        Action<Action<AllData>> UpdateDataCallback;
        private EditMethods editMethods;
        public override Action<NameData> LoadSelectedTab { get; set; }

        public DataNamesViewModel(Portfolio portfolio, List<Sector> sectors, Action<Action<AllData>> updateDataCallback, Action<ErrorReports> updateReports, Action<NameData> loadSelectedData, EditMethods updateMethods)
        {
            Portfolio = portfolio;
            Sectors = sectors;
            UpdateDataCallback = updateDataCallback;
            UpdateReports = updateReports;
            LoadSelectedTab = loadSelectedData;
            editMethods = updateMethods;

            CreateCommand = new BasicCommand(ExecuteCreateEdit);
            DeleteCommand = new BasicCommand(ExecuteDelete);
            DownloadCommand = new BasicCommand(ExecuteDownloadCommand);
        }
    }
}
