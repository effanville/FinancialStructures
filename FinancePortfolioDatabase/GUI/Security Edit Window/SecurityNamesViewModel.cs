using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using GUISupport;
using SavingClasses;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    public class SecurityNamesViewModel : PropertyChangedBase
    {
        private Portfolio Portfolio;
        public string Header { get; } = "Listed Securities";
        public bool Closable { get { return false; } }
        private List<NameCompDate> fPreEditFundNames = new List<NameCompDate>();

        private List<NameCompDate> fFundNames = new List<NameCompDate>();
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
            set { fSelectedName = value; OnPropertyChanged(); }
        }

        public ICommand CreateSecurityCommand { get; set; }

        private void ExecuteCreateEditCommand(Object obj)
        {
            var reports = new ErrorReports();
            if (Portfolio.Funds.Count != FundNames.Count)
            {
                bool edited = false;
                foreach (var name in FundNames)
                {
                    if (name.NewValue && (!string.IsNullOrEmpty(name.Name) || !string.IsNullOrEmpty(name.Company)))
                    {
                        edited = true;
                        UpdateData(alldata => alldata.MyFunds.TryAddSecurity(reports, name.Company, name.Name, name.Currency, name.Url, name.Sectors));
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    reports.AddError("No Name provided to create a sector.", Location.AddingData);
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
                        UpdateData(alldata => alldata.MyFunds.TryEditSecurityName(reports, fPreEditFundNames[i].Company, fPreEditFundNames[i].Name, name.Company, name.Name, name.Currency, name.Url, name.Sectors));
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    reports.AddError("Was not able to edit desired security.", Location.EditingData);
                }
            }

            if (reports.Any())
            {
                UpdateReports(reports);
            }

        }

        public ICommand DownloadCommand { get; }

        private void ExecuteDownloadCommand(Object obj)
        {
            var reports = new ErrorReports();
            if (fSelectedName != null)
            {
                UpdateData(async alldata => await DataUpdater.DownloadSecurity(alldata.MyFunds, fSelectedName.Company, fSelectedName.Name, UpdateReports, reports).ConfigureAwait(false));
            }

            if (reports.Any())
            {
                UpdateReports(reports);
            }
        }

        public void UpdateFundListBox(Portfolio portfolio)
        {
            Portfolio = portfolio;
            var currentSelectedName = selectedName;
            FundNames = Portfolio.SecurityNamesAndCompanies();
            FundNames.Sort();
            fPreEditFundNames = Portfolio.SecurityNamesAndCompanies();
            fPreEditFundNames.Sort();

            for (int i = 0; i < FundNames.Count; i++)
            {
                if (FundNames[i].CompareTo(currentSelectedName) == 0)
                {
                    selectedName = FundNames[i];
                    return;
                }
            }
        }

        public ICommand DeleteSecurityCommand { get; }

        private void ExecuteDeleteSecurity(Object obj)
        {
            var reports = new ErrorReports();
            if (fSelectedName != null)
            {
                UpdateData(alldata => alldata.MyFunds.TryRemoveSecurity(reports, fSelectedName.Company, fSelectedName.Name));
            }
            else
            {
                reports.AddError("Something went wrong when trying to delete security.", Location.DeletingData);
            }

            if (reports.Any())
            {
                UpdateReports(reports);
            }
        }

        Action<Action<AllData>> UpdateData;
        Action<ErrorReports> UpdateReports;
        public Action<NameData> LoadSelectedTab;
        public SecurityNamesViewModel(Portfolio portfolio, Action<Action<AllData>> updateData, Action<ErrorReports> updateReports, Action<NameData> loadSelectedData)
        {
            Portfolio = portfolio;
            UpdateData = updateData;
            UpdateReports = updateReports;
            LoadSelectedTab = loadSelectedData;
            FundNames = portfolio.SecurityNamesAndCompanies();
            fPreEditFundNames = portfolio.SecurityNamesAndCompanies();

            CreateSecurityCommand = new BasicCommand(ExecuteCreateEditCommand);
            DownloadCommand = new BasicCommand(ExecuteDownloadCommand);
            DeleteSecurityCommand = new BasicCommand(ExecuteDeleteSecurity);
        }
    }
}
