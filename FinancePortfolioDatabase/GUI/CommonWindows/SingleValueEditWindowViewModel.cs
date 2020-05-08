using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Reporting;
using UICommon.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FinanceCommonViewModels
{
    internal class SingleValueEditWindowViewModel : ViewModelBase
    {
        private AccountType TypeOfAccount;
        internal IPortfolio Portfolio;
        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        private readonly Action<Action<IPortfolio>> UpdateDataCallback;
        private readonly IReportLogger ReportLogger;
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogCreationService;
        private readonly EditMethods EditMethods;

        public SingleValueEditWindowViewModel(string title, IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, IReportLogger reportLogger, IFileInteractionService fileService, IDialogCreationService dialogCreation, EditMethods editMethods, AccountType accountType)
            : base(title)
        {
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            fFileService = fileService;
            fDialogCreationService = dialogCreation;
            EditMethods = editMethods;
            TypeOfAccount = accountType;
            UpdateData(portfolio);
            LoadSelectedTab = (name) => LoadTabFunc(name);
            Tabs.Add(new DataNamesViewModel(Portfolio, updateDataCallback, reportLogger, LoadSelectedTab, editMethods));
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            Portfolio = portfolio;
            List<object> removableTabs = new List<object>();
            if (Tabs != null)
            {
                foreach (var item in Tabs)
                {
                    if (item is ViewModelBase viewModel)
                    {
                        viewModel.UpdateData(portfolio, tabItem => removableTabs.Add(tabItem));
                    }
                }

                if (removableTabs.Any())
                {
                    foreach (var tab in removableTabs)
                    {
                        Tabs.Remove(tab);
                    }

                    removableTabs.Clear();
                }
            }
        }

        private void LoadTabFunc(NameData_ChangeLogged name)
        {
            Tabs.Add(new SelectedSingleDataViewModel(Portfolio, UpdateDataCallback, ReportLogger, fFileService, fDialogCreationService, EditMethods, name, TypeOfAccount));
        }
    }
}
