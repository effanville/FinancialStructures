using FinanceCommonViewModels;
using FinancialStructures.Database;
using FinancialStructures.DataReader;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.GUIFinanceStructures;
using GUISupport;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using FinancialStructures.ReportLogging;
using FinancialStructures.NamingStructures;

namespace FinanceWindowsViewModels
{
    internal class SelectedSecurityViewModel : ViewModelBase
    {
        private Portfolio Portfolio;

        public override bool Closable { get { return true; } }

        private readonly NameData_ChangeLogged fSelectedName;

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

        private readonly Action<Action<Portfolio>> UpdateDataCallback;

        private readonly LogReporter ReportLogger;

        public SelectedSecurityViewModel(Portfolio portfolio, Action<Action<Portfolio>> updateData, LogReporter reportLogger, NameData_ChangeLogged selectedName)
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
                UpdateDataCallback(programPortfolio => programPortfolio.TryDeleteData(AccountType.Security, fSelectedName, new DayValue_ChangeLogged(fSelectedValues.Date, 0.0), ReportLogger));
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
                    outputs = CsvDataRead.ReadFromCsv(openFile.FileName, AccountType.Security, ReportLogger.Log);
                }
                if (outputs != null)
                {
                    foreach (var objec in outputs)
                    {
                        if (objec is DayDataView view)
                        {
                            UpdateDataCallback(programPortfolio => programPortfolio.TryAddDataToSecurity(ReportLogger, fSelectedName.Company, fSelectedName.Name, view.Date, view.ShareNo, view.UnitPrice, view.NewInvestment));
                        }
                        else
                        {
                            ReportLogger.Log("Error", "StatisticsPage", "Have the wrong type of thing");
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
                Portfolio.TryGetSecurity(fSelectedName.Company, fSelectedName.Name, out var desired);
                if (desired.Count() != SelectedSecurityData.Count)
                {
                    UpdateDataCallback(programPortfolio => programPortfolio.TryAddDataToSecurity(ReportLogger, fSelectedName.Company, fSelectedName.Name, selectedValues.Date, selectedValues.ShareNo, selectedValues.UnitPrice, selectedValues.NewInvestment));
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
                            UpdateDataCallback(programPortfolio => programPortfolio.TryEditSecurityData(ReportLogger, fSelectedName.Company, fSelectedName.Name, fOldSelectedValues.Date, selectedValues.Date, selectedValues.ShareNo, selectedValues.UnitPrice, selectedValues.NewInvestment));
                            name.NewValue = false;
                        }
                    }
                    if (!edited)
                    {
                        ReportLogger.Log("Error", "EditingData", "Was not able to edit security data.");
                    }
                }
            }
        }

        public override void UpdateData(Portfolio portfolio, Action<object> removeTab)
        {
            Portfolio = portfolio;
            if (fSelectedName != null)
            {
                if (!portfolio.TryGetSecurity(fSelectedName.Company, fSelectedName.Name, out _))
                {
                    removeTab(this);
                    return;
                }

                SelectedSecurityData = Portfolio.SecurityData(fSelectedName.Company, fSelectedName.Name);
                SelectLatestValue();
            }
            else
            {
                SelectedSecurityData = null;
            }
        }

        public override void UpdateData(Portfolio portfolio)
        {
            UpdateData(portfolio, null);
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
