﻿using FinanceWindowsViewModels.SecurityEdit;
using FinancialStructures.FinanceStructures;
using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using GlobalHeldData;
using GUIAccessorFunctions;
using GUISupport;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    public class SecurityEditWindowViewModel : PropertyChangedBase
    {
        private List<NameCompDate> fPreEditFundNames;

        private List<NameCompDate> fFundNames;
        /// <summary>
        /// Name and Company data of Funds in database for view.
        /// </summary>
        public List<NameCompDate> FundNames
        {
            get { return fFundNames; }
            set { fFundNames = value; OnPropertyChanged(); }
        }

        private NameData fSelectedName;

        /// <summary>
        /// Name and Company data of the selected security in the list <see cref="FundNames"/>
        /// </summary>
        public NameData selectedName
        {
            get { return fSelectedName; }
            set { fSelectedName = value; OnPropertyChanged(); UpdateSelectedSecurityListBox(); }
        }


        private Security fSelectedSecurity;
        /// <summary>
        /// The Complete data on the security selected
        /// </summary>
        public Security selectedSecurity
        {
            get { return fSelectedSecurity; }
            set { fSelectedSecurity = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The pricing data of the selected security.
        /// </summary>
        private List<DayDataView> fSelectedSecurityData;
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
                int index = SelectedSecurityData.IndexOf(value);
                if (selectedIndex != index)
                {
                    selectedIndex = index;
                    fOldSelectedValues = fSelectedValues?.Copy();
                }

                OnPropertyChanged(); 
                UpdateSubWindows(); 
            }
        }

        private UserButtonsViewModel fUserClickingVM;

        public UserButtonsViewModel UserClickingVM
        {
            get { return fUserClickingVM; }
            set { fUserClickingVM = value; OnPropertyChanged(); }
        }

        public ICommand CreateSecurityCommand { get; set; }

        public ICommand AddEditSecurityDataCommand { get; set; }

        public void UpdateFundListBox()
        {
            var currentSelectedName = selectedName;
            FundNames = GlobalData.Finances.SecurityNamesAndCompanies();
            FundNames.Sort();
            fPreEditFundNames = GlobalData.Finances.SecurityNamesAndCompanies();
            fPreEditFundNames.Sort();

            for (int i = 0; i < FundNames.Count; i++)
            {
                if (FundNames[i].CompareTo(currentSelectedName) == 0)
                {
                    selectedName = FundNames[i];
                }
            }
        }

        private void UpdateSelectedSecurityListBox()
        {
            if (fSelectedName != null)
            {
                GlobalData.Finances.GetPortfolio().TryGetSecurity(fSelectedName.Company, fSelectedName.Name, out Security wanted);
                selectedSecurity = wanted;
                if (GlobalData.Finances.TryGetSecurityData(fSelectedName.Company, fSelectedName.Name, out List<DayDataView> values))
                {
                    SelectedSecurityData = values;
                }

                SelectLatestValue();
                UpdateSubWindows();
            }
        }

        private void UpdateSubWindows()
        {
            UserClickingVM.UpdateButtonViewData(selectedName, selectedValues);
        }

        private void SelectLatestValue()
        {
            if (SelectedSecurityData != null && SelectedSecurityData.Count > 0)
            {
                selectedValues = SelectedSecurityData[SelectedSecurityData.Count - 1];
            }
        }

        /// <summary>
        /// Clears selected data in both Gridviews
        /// </summary>
        private void ClearSelection()
        {
            selectedSecurity = null;
            UpdateSubWindows();
        }

        private void ExecuteCreateEditCommand(Object obj)
        {
            var reports = new ErrorReports();
            if (GlobalData.Finances.GetPortfolio().Funds.Count != FundNames.Count)
            {
                bool edited = false;
                foreach (var name in FundNames)
                {
                    if (name.NewValue && (!string.IsNullOrEmpty(name.Name) || !string.IsNullOrEmpty(name.Company)))
                    {
                        edited = true;
                        GlobalData.Finances.TryAddSecurity(reports, name.Company, name.Name, name.Currency, name.Url, name.Sectors);
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    reports.AddError("No Name provided to create a sector.");
                }
            }
            else
            {
                // maybe fired from editing stuff. Try that
                bool edited = false;
                for (int i = 0; i < FundNames.Count; i++)
                {
                    var name = FundNames[i];

                    if (name.NewValue && (!string.IsNullOrEmpty(name.Name) || !string.IsNullOrEmpty(name.Company)))
                    {
                        edited = true;
                        GlobalData.Finances.TryEditSecurityName(reports, fPreEditFundNames[i].Company, fPreEditFundNames[i].Name, name.Company, name.Name, name.Currency, name.Url, name.Sectors);
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    reports.AddError("Was not able to edit desired security.");
                }
            }

            if (reports.Any())
            {
                UpdateReports(reports);
            }

            ClearSelection();
            UpdateMainWindow(true);
        }

        private void ExecuteAddEditSecData(Object obj)
        {
            var reports = new ErrorReports();
            if (selectedName != null && selectedSecurity != null)
            {
                if (GlobalData.Finances.GetSecurityFromName(selectedName.Name, selectedName.Company).Count() != SelectedSecurityData.Count)
                {
                    GlobalData.Finances.TryAddDataToSecurity(reports, selectedName.Company, selectedName.Name, selectedValues.Date, selectedValues.ShareNo, selectedValues.UnitPrice, selectedValues.Investment);
                    selectedName.NewValue = false;
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
                            GlobalData.Finances.TryEditSecurityData(reports, selectedName.Company, selectedName.Name, fOldSelectedValues.Date, selectedValues.Date, selectedValues.ShareNo, selectedValues.UnitPrice, selectedValues.Investment);
                            name.NewValue = false;
                        }
                    }
                    if (!edited)
                    {
                        reports.AddError("Was not able to edit security data.");
                    }
                }
            }
            if (reports.Any())
            {
                UpdateReports(reports);
            }
            UpdateSelectedSecurityListBox();
        }

        Action<bool> UpdateMainWindow;
        Action<ErrorReports> UpdateReports;

        public SecurityEditWindowViewModel(Action<bool> updateWindow, Action<ErrorReports> updateReports)
        {
            UpdateMainWindow = updateWindow;
            UpdateReports = updateReports;
            UserClickingVM = new UserButtonsViewModel(updateWindow, updateReports, selectedName, selectedValues);

            fFundNames = new List<NameCompDate>();
            fPreEditFundNames = new List<NameCompDate>();
            fSelectedSecurityData = new List<DayDataView>();

            UpdateFundListBox();

            CreateSecurityCommand = new BasicCommand(ExecuteCreateEditCommand);
            AddEditSecurityDataCommand = new BasicCommand(ExecuteAddEditSecData);
        }
    }
}
