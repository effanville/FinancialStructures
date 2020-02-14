using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using GUISupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FinanceCommonViewModels
{
    public class SelectedSingleDataViewModel : ViewModelBase
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
                SelectedData = EditMethods.SelectedDataMethod(Portfolio, Sectors, SelectedName, reports);
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
                if (EditMethods.SelectedDataMethod(Portfolio, Sectors, SelectedName, reports).Count() != SelectedData.Count)
                {
                    EditMethods.AddDataMethod(Portfolio, Sectors, SelectedName, SelectedValue, reports);
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
                            EditMethods.EditDataMethod(Portfolio, Sectors, SelectedName, fOldSelectedValue, SelectedValue, reports);
                            name.NewValue = false;
                        }
                    }
                    if (!edited)
                    {
                        reports.AddError("Was not able to edit sector data.");
                    }
                }

                UpdateMainWindow();
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
                EditMethods.DeleteDataMethod(Portfolio, Sectors, SelectedName, SelectedValue, reports);
            }
            else
            {
                reports.AddError("No Bank Account was selected when trying to delete data.");
            }
            if (reports.Any())
            {
                UpdateReports(reports);
            }
            UpdateMainWindow();
        }

        private void SelectLatestValue()
        {
            if (SelectedData != null && SelectedData.Count > 0)
            {
                SelectedValue = SelectedData[SelectedData.Count - 1];
            }
        }

        public override Action<NameData> LoadSelectedTab { get; set; }
        Action UpdateMainWindow;
        Action<ErrorReports> UpdateReports;

        private EditMethods EditMethods;
        public SelectedSingleDataViewModel(Portfolio portfolio, List<Sector> sectors, Action updateWindow, Action<ErrorReports> updateReports, EditMethods editMethods, NameData selectedName)
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
            UpdateMainWindow = updateWindow;
            UpdateReports = updateReports;
        }
    }
}
