using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using GUISupport;
using SavingClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FinanceCommonViewModels
{
    internal class SelectedSingleDataViewModel : ViewModelBase
    {
        private Portfolio Portfolio;
        private List<Sector> Sectors;
        public string Header { get; }
        public bool Closable { get { return true; } }

        private NameData fSelectedName;

        /// <summary>
        /// Name and Company data of the selected security in the list <see cref="AccountNames"/>
        /// </summary>
        public NameData SelectedName
        {
            get { return fSelectedName; }
            set { fSelectedName = value; OnPropertyChanged(); }
        }

        private List<AccountDayDataView> fSelectedData;
        public List<AccountDayDataView> SelectedData
        {
            get { return fSelectedData; }
            set { fSelectedData = value; OnPropertyChanged(); }
        }

        private AccountDayDataView fSelectedValues;
        private AccountDayDataView fOldSelectedValue;
        private int SelectedIndex;
        public AccountDayDataView SelectedValue
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

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
        {
            Portfolio = portfolio;
            Sectors = sectors;
            var reports = new ErrorReports();
            if (SelectedName != null)
            {
                SelectedData = (List<AccountDayDataView>)EditMethods.ExecuteFunction(FunctionType.SelectData, Portfolio, Sectors, SelectedName, reports).Result;
                SelectLatestValue();
            }
            else
            {
                SelectedData = null;
            }
        }

        public ICommand EditDataCommand { get; set; }

        private void ExecuteEditDataCommand(Object obj)
        {
            var reports = new ErrorReports();
            if (SelectedName != null)
            {
                if (((List<AccountDayDataView>)EditMethods.ExecuteFunction(FunctionType.SelectData, Portfolio, Sectors, SelectedName, reports).Result).Count() != SelectedData.Count)
                {
                    UpdateDataCallback(alldata => EditMethods.ExecuteFunction(FunctionType.AddData, alldata.MyFunds, alldata.myBenchMarks, SelectedName, SelectedValue, reports).Wait());
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
                            UpdateDataCallback(alldata => EditMethods.ExecuteFunction(FunctionType.EditData, alldata.MyFunds, alldata.myBenchMarks, SelectedName, fOldSelectedValue, SelectedValue, reports).Wait());
                            name.NewValue = false;
                        }
                    }
                    if (!edited)
                    {
                        reports.AddError("Was not able to edit data.", Location.EditingData);
                    }
                }
            }
            if (reports.Any())
            {
                UpdateReports(reports);
            }
        }

        public ICommand DeleteValuationCommand { get; }

        private void ExecuteDeleteValuation(Object obj)
        {
            var reports = new ErrorReports();
            if (SelectedName != null)
            {
                UpdateDataCallback(alldata => EditMethods.ExecuteFunction(FunctionType.DeleteData, alldata.MyFunds, alldata.myBenchMarks, SelectedName, SelectedValue, reports).Wait());
            }
            else
            {
                reports.AddError("No Account was selected when trying to delete data.", Location.DeletingData);
            }
            if (reports.Any())
            {
                UpdateReports(reports);
            }
        }

        private void SelectLatestValue()
        {
            if (SelectedData != null && SelectedData.Count > 0)
            {
                SelectedValue = SelectedData[SelectedData.Count - 1];
            }
        }

        public override Action<NameData> LoadSelectedTab { get; set; }
        Action<Action<AllData>> UpdateDataCallback;
        Action<ErrorReports> UpdateReports;

        private EditMethods EditMethods;
        public SelectedSingleDataViewModel(Portfolio portfolio, List<Sector> sectors, Action<Action<AllData>> updateDataCallback, Action<ErrorReports> updateReports, EditMethods editMethods, NameData selectedName)
        {
            if (selectedName != null)
            {
                Header = selectedName.Company + "-" + selectedName.Name;
            }
            else
            {
                Header = "No-Name";
            }
            SelectedName = selectedName;
            EditMethods = editMethods;
            UpdateData(portfolio, sectors);

            EditDataCommand = new BasicCommand(ExecuteEditDataCommand);
            DeleteValuationCommand = new BasicCommand(ExecuteDeleteValuation);
            UpdateDataCallback = updateDataCallback;
            UpdateReports = updateReports;
        }
    }
}
