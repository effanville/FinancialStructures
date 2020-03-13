using FinanceCommonViewModels;
using FinancialStructures.Database;
using FinancialStructures.DataReader;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using GUISupport;
using SavingClasses;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class SelectedSecurityViewModel : ViewModelBase
    {
        private Portfolio Portfolio;

        public override bool Closable { get { return true; } }

        private readonly NameData fSelectedName;

        /// <summary>
        /// The pricing data of the selected security.
        /// </summary>
        private List<DayDataView> fSelectedSecurityData = new List<DayDataView>();
        public List<DayDataView> SelectedSecurityData
        {
            get { return fSelectedSecurityData; }
            set { fSelectedSecurityData = value; OnPropertyChanged(); }
        }

        private DayDataView fSelectedValues;
        private DayDataView fOldSelectedValues;
        private int selectedIndex;
        public DayDataView selectedValues
        {
            get
            {
                return fSelectedValues;
            }
            set
            {
                fSelectedValues = value;
                if (SelectedSecurityData != null)
                {
                    int index = SelectedSecurityData.IndexOf(value);
                    if (selectedIndex != index)
                    {
                        selectedIndex = index;
                        fOldSelectedValues = fSelectedValues?.Copy();
                    }
                }
                OnPropertyChanged();
            }
        }

        private readonly Action<Action<AllData>> UpdateDataCallback;

        private readonly Action<string, string, string> ReportLogger;

        public SelectedSecurityViewModel(Portfolio portfolio, Action<Action<AllData>> updateData, Action<string, string, string> reportLogger, NameData selectedName)
            : base(selectedName != null ? selectedName.Company + "-" + selectedName.Name : "No-Name")
        {
            fSelectedName = selectedName;
            DeleteValuationCommand = new BasicCommand(ExecuteDeleteValuation);
            AddCsvData = new BasicCommand(ExecuteAddCsvData);
            AddEditSecurityDataCommand = new BasicCommand(ExecuteAddEditSecData);
            UpdateData(portfolio, null);
            UpdateDataCallback = updateData;
            ReportLogger = reportLogger;
        }

        public ICommand DeleteValuationCommand { get; }

        private void ExecuteDeleteValuation(Object obj)
        {
            if (fSelectedName != null && fSelectedValues != null)
            {
                UpdateDataCallback(alldata => alldata.MyFunds.TryRemoveSecurityData(ReportLogger, fSelectedName.Company, fSelectedName.Name, fSelectedValues.Date, fSelectedValues.ShareNo, fSelectedValues.UnitPrice, fSelectedValues.NewInvestment));
            }
        }

        public ICommand AddCsvData { get; }

        private void ExecuteAddCsvData(Object obj)
        {
            if (fSelectedName != null)
            {
                OpenFileDialog openFile = new OpenFileDialog() { DefaultExt = "xml" };
                openFile.Filter = "Csv Files|*.csv|All Files|*.*";
                List<object> outputs = null;
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    outputs = CsvDataRead.ReadFromCsv(openFile.FileName, PortfolioElementType.Security, ReportLogger);
                }
                if (outputs != null)
                {
                    foreach (var objec in outputs)
                    {
                        if (objec is DayDataView view)
                        {
                            UpdateDataCallback(alldata => alldata.MyFunds.TryAddDataToSecurity(ReportLogger, fSelectedName.Company, fSelectedName.Name, view.Date, view.ShareNo, view.UnitPrice, view.NewInvestment));
                        }
                        else
                        {
                            ReportLogger("Error", "StatisticsPage", "Have the wrong type of thing");
                        }
                    }
                }
            }
        }

        public ICommand AddEditSecurityDataCommand { get; set; }

        private void ExecuteAddEditSecData(Object obj)
        {
            if (fSelectedName != null)
            {
                if (Portfolio.GetSecurityFromName(fSelectedName.Name, fSelectedName.Company).Count() != SelectedSecurityData.Count)
                {
                    UpdateDataCallback(alldata => alldata.MyFunds.TryAddDataToSecurity(ReportLogger, fSelectedName.Company, fSelectedName.Name, selectedValues.Date, selectedValues.ShareNo, selectedValues.UnitPrice, selectedValues.NewInvestment));
                    fSelectedName.NewValue = false;
                }
                else
                {
                    bool edited = false;
                    for (int i = 0; i < SelectedSecurityData.Count; i++)
                    {
                        var name = SelectedSecurityData[i];

                        if (name.NewValue)
                        {
                            edited = true;
                            UpdateDataCallback(alldata => alldata.MyFunds.TryEditSecurityData(ReportLogger, fSelectedName.Company, fSelectedName.Name, fOldSelectedValues.Date, selectedValues.Date, selectedValues.ShareNo, selectedValues.UnitPrice, selectedValues.NewInvestment));
                            name.NewValue = false;
                        }
                    }
                    if (!edited)
                    {
                        ReportLogger("Error", "EditingData", "Was not able to edit security data.");
                    }
                }
            }
        }

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors, Action<object> removeTab)
        {
            Portfolio = portfolio;
            if (fSelectedName != null)
            {
                if (!portfolio.TryGetSecurity(fSelectedName.Company, fSelectedName.Name, out _))
                {
                    removeTab(this);
                    return;
                }

                if (Portfolio.TryGetSecurityData(fSelectedName.Company, fSelectedName.Name, out List<DayDataView> values))
                {
                    SelectedSecurityData = values;
                }

                SelectLatestValue();
            }
            else
            {
                SelectedSecurityData = null;
            }
        }

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
        {
            UpdateData(portfolio, sectors, null);
        }

        private void SelectLatestValue()
        {
            if (SelectedSecurityData != null && SelectedSecurityData.Count > 0)
            {
                selectedValues = SelectedSecurityData[SelectedSecurityData.Count - 1];
            }
        }
    }
}
