using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
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

        public override bool Closable { get { return true; } }

        private NameData fSelectedName;

        /// <summary>
        /// Name and Company data of the selected security in the list <see cref="AccountNames"/>
        /// </summary>
        public NameData SelectedName
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

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
        {
            Portfolio = portfolio;
            Sectors = sectors;
            if (SelectedName != null)
            {
                SelectedData = (List<DayValue_ChangeLogged>)EditMethods.ExecuteFunction(FunctionType.SelectData, Portfolio, Sectors, SelectedName, ReportLogger).Result;
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
            if (SelectedName != null)
            {
                if (((List<DayValue_ChangeLogged>)EditMethods.ExecuteFunction(FunctionType.SelectData, Portfolio, Sectors, SelectedName, ReportLogger).Result).Count() != SelectedData.Count)
                {
                    UpdateDataCallback(alldata => EditMethods.ExecuteFunction(FunctionType.AddData, alldata.MyFunds, alldata.myBenchMarks, SelectedName, SelectedValue, ReportLogger).Wait());
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
                            UpdateDataCallback(alldata => EditMethods.ExecuteFunction(FunctionType.EditData, alldata.MyFunds, alldata.myBenchMarks, SelectedName, fOldSelectedValue, SelectedValue, ReportLogger).Wait());
                            name.NewValue = false;
                        }
                    }
                    if (!edited)
                    {
                        ReportLogger("Error", "EditingData", "Was not able to edit data.");
                    }
                }
            }
        }

        public ICommand DeleteValuationCommand { get; }

        private void ExecuteDeleteValuation(Object obj)
        {
            if (SelectedName != null)
            {
                UpdateDataCallback(alldata => EditMethods.ExecuteFunction(FunctionType.DeleteData, alldata.MyFunds, alldata.myBenchMarks, SelectedName, SelectedValue, ReportLogger).Wait());
            }
            else
            {
                ReportLogger("Error", "DeletingData", "No Account was selected when trying to delete data.");
            }
        }

        private void SelectLatestValue()
        {
            if (SelectedData != null && SelectedData.Count > 0)
            {
                SelectedValue = SelectedData[SelectedData.Count - 1];
            }
        }

        Action<Action<AllData>> UpdateDataCallback;
        Action<string, string, string> ReportLogger;

        private EditMethods EditMethods;
        public SelectedSingleDataViewModel(Portfolio portfolio, List<Sector> sectors, Action<Action<AllData>> updateDataCallback, Action<string, string, string> reportLogger, EditMethods editMethods, NameData selectedName)
            : base(selectedName !=null ? selectedName.Company + "-" + selectedName.Name : "No-Name")
        {
            SelectedName = selectedName;
            EditMethods = editMethods;
            UpdateData(portfolio, sectors);

            EditDataCommand = new BasicCommand(ExecuteEditDataCommand);
            DeleteValuationCommand = new BasicCommand(ExecuteDeleteValuation);
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
        }
    }
}
