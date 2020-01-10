﻿using FinanceWindowsViewModels.SecurityEdit;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using GUIAccessorFunctions;
using GUISupport;
using SecurityHelperFunctions;
using System;
using System.Collections.Generic;
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

        private NameComp fSelectedName;

        /// <summary>
        /// Name and Company data of the selected security in the list <see cref="FundNames"/>
        /// </summary>
        public NameComp selectedName
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
        private List<BasicDayDataView> fSelectedSecurityData;
        public List<BasicDayDataView> SelectedSecurityData
        {
            get { return fSelectedSecurityData; }
            set { fSelectedSecurityData = value; OnPropertyChanged(); }
        }

        private BasicDayDataView fSelectedValues;
        private BasicDayDataView fOldSelectedValues;
        private int selectedIndex;
        public BasicDayDataView selectedValues
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
            FundNames = DatabaseAccessor.GetSecurityNamesAndCompanies();
            FundNames.Sort();
            fPreEditFundNames = DatabaseAccessor.GetSecurityNamesAndCompanies();
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
                DatabaseAccessor.GetPortfolio().TryGetSecurity(fSelectedName.Name, fSelectedName.Company, out Security wanted);
                selectedSecurity = wanted;
                if (SecurityEditor.TryGetSecurityData(fSelectedName.Name, fSelectedName.Company, out List<BasicDayDataView> values))
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
            if (DatabaseAccessor.GetPortfolio().Funds.Count != FundNames.Count)
            {
                bool edited = false;
                foreach (var name in FundNames)
                {
                    if (name.NewValue && (!string.IsNullOrEmpty(name.Name) || !string.IsNullOrEmpty(name.Company)))
                    {
                        edited = true;
                        SecurityEditor.TryAddSecurity(name.Name, name.Company, name.Url, name.Sectors, reports);
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
                        SecurityEditor.TryEditSecurityName(fPreEditFundNames[i].Name, fPreEditFundNames[i].Company, name.Name, name.Company, name.Url, name.Sectors, reports);
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
            //UpdateFundListBox();
            ClearSelection();
            UpdateMainWindow(true);
        }

        private void ExecuteAddEditSecData(Object obj)
        {
            var reports = new ErrorReports();
            if (selectedName != null && selectedSecurity != null)
            {
                if (DatabaseAccessor.GetSecurityFromName(selectedName.Name, selectedName.Company).Count() != SelectedSecurityData.Count)
                {
                    SecurityEditor.TryAddDataToSecurity(reports, selectedName.Name, selectedName.Company, selectedValues.Date, selectedValues.ShareNo, selectedValues.UnitPrice, selectedValues.Investment);
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
                            SecurityEditor.TryEditSecurity(reports, selectedName.Name, selectedName.Company, fOldSelectedValues.Date, selectedValues.Date, selectedValues.ShareNo, selectedValues.UnitPrice, selectedValues.Investment);
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

        public SecurityEditWindowViewModel(Action<bool> updateWindow, Action<string> pageViewChoice, Action<ErrorReports> updateReports)
        {
            UpdateMainWindow = updateWindow;
            UpdateReports = updateReports;
            UserClickingVM = new UserButtonsViewModel(updateWindow, pageViewChoice, updateReports, selectedName, selectedValues);

            fFundNames = new List<NameCompDate>();
            fPreEditFundNames = new List<NameCompDate>();
            fSelectedSecurityData = new List<BasicDayDataView>();

            UpdateFundListBox();

            CreateSecurityCommand = new BasicCommand(ExecuteCreateEditCommand);
            AddEditSecurityDataCommand = new BasicCommand(ExecuteAddEditSecData);
        }
    }
}
