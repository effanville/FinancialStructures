using FinanceCommonViewModels;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.PortfolioAPI;
using GUISupport;
using SavingClasses;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class SecurityNamesViewModel : ViewModelBase
    {
        private Portfolio Portfolio;

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

        private readonly Action<Action<AllData>> DataUpdateCallback;

        private readonly Action<string, string, string> ReportLogger;

        public SecurityNamesViewModel(Portfolio portfolio, Action<Action<AllData>> updateData, Action<string, string, string> reportLogger, Action<NameData> loadSelectedData)
            : base("Listed Securities", loadSelectedData)
        {
            Portfolio = portfolio;
            DataUpdateCallback = updateData;
            ReportLogger = reportLogger;
            FundNames = portfolio.SecurityNamesAndCompanies();
            fPreEditFundNames = portfolio.SecurityNamesAndCompanies();

            CreateSecurityCommand = new BasicCommand(ExecuteCreateEditCommand);
            DownloadCommand = new BasicCommand(ExecuteDownloadCommand);
            DeleteSecurityCommand = new BasicCommand(ExecuteDeleteSecurity);
        }

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors, Action<object> removeTab)
        {
            Portfolio = portfolio;
            var currentSelectedName = selectedName;
            FundNames = portfolio.SecurityNamesAndCompanies();
            FundNames.Sort();
            fPreEditFundNames = portfolio.SecurityNamesAndCompanies();
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


        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
        {
            UpdateData(portfolio, sectors, null);
        }

        public ICommand CreateSecurityCommand { get; set; }

        private void ExecuteCreateEditCommand(Object obj)
        {
            if (Portfolio.Funds.Count != FundNames.Count)
            {
                bool edited = false;
                if (selectedName.NewValue)
                {
                    edited = true;
                    DataUpdateCallback(alldata => alldata.MyFunds.TryAdd(PortfolioElementType.Security, selectedName, ReportLogger));
                    if (selectedName != null)
                    {
                        selectedName.NewValue = false;
                    }
                }

                if (!edited)
                {
                    ReportLogger("Error", "AddingData", "No Name provided to create a sector.");
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
                        DataUpdateCallback(alldata => alldata.MyFunds.TryEditName(PortfolioElementType.Security, fPreEditFundNames[i], name, ReportLogger));
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    ReportLogger("Error", "EditingData", "Was not able to edit desired security.");
                }
            }
        }

        public ICommand DownloadCommand { get; }

        private void ExecuteDownloadCommand(Object obj)
        {
            if (fSelectedName != null)
            {
                DataUpdateCallback(async alldata => await DataUpdater.DownloadSecurity(alldata.MyFunds, fSelectedName.Company, fSelectedName.Name, ReportLogger).ConfigureAwait(false));
            }
        }

        public ICommand DeleteSecurityCommand { get; }

        private void ExecuteDeleteSecurity(Object obj)
        {
            if (fSelectedName != null)
            {
                DataUpdateCallback(alldata => alldata.MyFunds.TryRemove(PortfolioElementType.Security, fSelectedName, ReportLogger));
            }
            else
            {
                ReportLogger("Error", "DeletingData", "Something went wrong when trying to delete security.");
            }
        }
    }
}
