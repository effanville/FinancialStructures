using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using GUISupport;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinanceCommonViewModels
{
    public class DataNamesViewModel : ViewModelBase
    {
        private Portfolio Portfolio;
        private List<Sector> Sectors;
        Action UpdateMainWindow;
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

        private async void ExecuteDownloadCommand(Object obj)
        {
            var reports = new ErrorReports();
            if (SelectedName != null)
            {
                await editMethods.DownloadMethod(Portfolio, Sectors, SelectedName, UpdateReports, reports).ConfigureAwait(false);
            }
            UpdateMainWindow();
            if (reports.Any())
            {
                UpdateReports(reports);
            }
        }

        private void ExecuteCreateEdit(Object obj)
        {
            var reports = new ErrorReports();
            if (editMethods.UpdateNameMethod(Portfolio, Sectors).Count != DataNames.Count)
            {
                bool edited = false;
                foreach (var name in DataNames)
                {
                    if (name.NewValue && (!string.IsNullOrEmpty(name.Name) || !string.IsNullOrEmpty(name.Company)))
                    {
                        edited = true;
                        editMethods.CreateMethod(Portfolio, Sectors, name, reports);
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    reports.AddError("No Name provided on creation.");
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
                        editMethods.EditMethod(Portfolio, Sectors, fPreEditNames[i], name, reports);
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    reports.AddError("Was not able to edit desired.");
                }
            }

            UpdateMainWindow();
            if (reports.Any())
            {
                UpdateReports(reports);
            }

            UpdateMainWindow();
        }

        private void ExecuteDelete(Object obj)
        {
            var reports = new ErrorReports();
            if (SelectedName.Name != null)
            {
                editMethods.DeleteMethod(Portfolio, Sectors, SelectedName, reports);
            }
            else
            {
                reports.AddError("Nothing was selected when trying to delete.");
            }
            if (reports.Any())
            {
                UpdateReports(reports);
            }

            UpdateMainWindow();
        }

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
        {
            Portfolio = portfolio;
            Sectors = sectors;
            var currentSelectedName = SelectedName;
            DataNames = editMethods.UpdateNameMethod(portfolio, sectors);
            DataNames.Sort();
            fPreEditNames = editMethods.UpdateNameMethod(portfolio, sectors);
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

        private EditMethods editMethods;
        public override Action<NameData> LoadSelectedTab { get; set; }

        public DataNamesViewModel(Portfolio portfolio, List<Sector> sectors, Action updateWindow, Action<ErrorReports> updateReports, Action<NameData> loadSelectedData, EditMethods updateMethods)
        {
            Portfolio = portfolio;
            Sectors = sectors;
            UpdateMainWindow = updateWindow;
            UpdateReports = updateReports;
            LoadSelectedTab = loadSelectedData;
            editMethods = updateMethods;

            CreateCommand = new BasicCommand(ExecuteCreateEdit);
            DeleteCommand = new BasicCommand(ExecuteDelete);
            DownloadCommand = new BasicCommand(ExecuteDownloadCommand);
        }
    }
}
