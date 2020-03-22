using FinancialStructures.DatabaseInterfaces;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.ReportLogging;
using GUISupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FinanceCommonViewModels
{
    internal class SelectedSingleDataViewModel : ViewModelBase
    {
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

        private readonly LogReporter ReportLogger;

        private readonly EditMethods EditMethods;

        public SelectedSingleDataViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, LogReporter reportLogger, EditMethods editMethods, NameData_ChangeLogged selectedName)
            : base(selectedName != null ? selectedName.Company + "-" + selectedName.Name : "No-Name")
        {
            SelectedName = selectedName;
            EditMethods = editMethods;
            UpdateData(portfolio);

            EditDataCommand = new BasicCommand(ExecuteEditDataCommand);
            DeleteValuationCommand = new BasicCommand(ExecuteDeleteValuation);
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
        }

        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            Portfolio = portfolio;
            if (SelectedName != null)
            {
                if (!((List<NameCompDate>)EditMethods.ExecuteFunction(FunctionType.NameUpdate, Portfolio).Result).Exists(name => name.IsEqualTo(SelectedName)))
                {
                    if (removeTab != null)
                    {
                        removeTab(this);

                    }
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
                            UpdateDataCallback(programPortfolio => EditMethods.ExecuteFunction(FunctionType.EditData, programPortfolio, SelectedName, fOldSelectedValue, SelectedValue, ReportLogger).Wait());
                            name.NewValue = false;
                        }
                    }
                    if (!edited)
                    {
                        ReportLogger.LogDetailed("Critical", "Error", "EditingData", "Was not able to edit data.");
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
                ReportLogger.LogDetailed("Critical", "Error", "DeletingData", "No Account was selected when trying to delete data.");
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
