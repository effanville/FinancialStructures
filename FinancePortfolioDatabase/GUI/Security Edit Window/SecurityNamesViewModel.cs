using FinanceCommonViewModels;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Reporting;
using GUISupport;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class SecurityNamesViewModel : ViewModelBase
    {
        internal IPortfolio Portfolio;

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

        private NameData_ChangeLogged fSelectedName;

        /// <summary>
        /// Name and Company data of the selected security in the list <see cref="FundNames"/>
        /// </summary>
        public NameData_ChangeLogged selectedName
        {
            get { return fSelectedName; }
            set { fSelectedName = value; OnPropertyChanged(); }
        }

        private readonly Action<Action<IPortfolio>> DataUpdateCallback;

        private readonly IReportLogger ReportLogger;

        public SecurityNamesViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, IReportLogger reportLogger, Action<NameData_ChangeLogged> loadSelectedData)
            : base("Listed Securities", loadSelectedData)
        {
            Portfolio = portfolio;
            DataUpdateCallback = updateData;
            ReportLogger = reportLogger;
            FundNames = portfolio.NameData(AccountType.Security);
            fPreEditFundNames = portfolio.NameData(AccountType.Security);

            CreateSecurityCommand = new BasicCommand(ExecuteCreateEditCommand);
            DownloadCommand = new BasicCommand(ExecuteDownloadCommand);
            DeleteSecurityCommand = new BasicCommand(ExecuteDeleteSecurity);
        }

        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            Portfolio = portfolio;
            var currentSelectedName = selectedName;
            FundNames = portfolio.NameData(AccountType.Security);
            FundNames.Sort();
            fPreEditFundNames = portfolio.NameData(AccountType.Security);
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

        public override void UpdateData(IPortfolio portfolio)
        {
            UpdateData(portfolio, null);
        }

        public ICommand CreateSecurityCommand { get; set; }

        private void ExecuteCreateEditCommand(Object obj)
        {
            if (Portfolio.NumberOf(AccountType.Security) != FundNames.Count)
            {
                bool edited = false;
                if (selectedName.NewValue)
                {
                    edited = true;
                    NameData name_new = new NameData(selectedName.Company, selectedName.Name, selectedName.Currency, selectedName.Url, selectedName.Sectors);
                    DataUpdateCallback(programPortfolio => programPortfolio.TryAdd(AccountType.Security, name_new, ReportLogger));
                    if (selectedName != null)
                    {
                        selectedName.NewValue = false;
                    }
                }

                if (!edited)
                {
                    ReportLogger.LogUsefulWithStrings("Error", "AddingData", "No Name provided to create a sector.");
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
                        NameData name_new = new NameData(name.Company, name.Name, name.Currency, name.Url, name.Sectors);
                        DataUpdateCallback(programPortfolio => programPortfolio.TryEditName(AccountType.Security, fPreEditFundNames[i], name_new, ReportLogger));
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    ReportLogger.LogUsefulWithStrings("Error", "EditingData", "Was not able to edit desired security.");
                }
            }
        }

        public ICommand DownloadCommand { get; }

        private void ExecuteDownloadCommand(Object obj)
        {
            if (fSelectedName != null)
            {
                DataUpdateCallback(async programPortfolio => await PortfolioDataUpdater.DownloadOfType(AccountType.Security, programPortfolio, fSelectedName, ReportLogger).ConfigureAwait(false));
            }
        }

        public ICommand DeleteSecurityCommand { get; }

        private void ExecuteDeleteSecurity(Object obj)
        {
            if (fSelectedName != null)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.TryRemove(AccountType.Security, fSelectedName, ReportLogger));
            }
            else
            {
                ReportLogger.LogUsefulWithStrings("Error", "DeletingData", "Something went wrong when trying to delete security.");
            }
        }
    }
}
