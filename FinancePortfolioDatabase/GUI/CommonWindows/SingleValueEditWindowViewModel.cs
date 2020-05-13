using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using StructureCommon.Reporting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace FinanceCommonViewModels
{
    internal class SingleValueEditWindowViewModel : ViewModelBase<IPortfolio>
    {
        private AccountType TypeOfAccount;
        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        private readonly Action<Action<IPortfolio>> UpdateDataCallback;
        private readonly IReportLogger ReportLogger;
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogCreationService;

        public SingleValueEditWindowViewModel(string title, IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, IReportLogger reportLogger, IFileInteractionService fileService, IDialogCreationService dialogCreation, AccountType accountType)
            : base(title, portfolio)
        {
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            fFileService = fileService;
            fDialogCreationService = dialogCreation;
            TypeOfAccount = accountType;
            UpdateData(portfolio);
            LoadSelectedTab = (name) => LoadTabFunc(name);
            Tabs.Add(new DataNamesViewModel(DataStore, updateDataCallback, reportLogger, LoadSelectedTab, accountType));
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            base.UpdateData(portfolio);
            List<object> removableTabs = new List<object>();
            if (Tabs != null)
            {
                foreach (var item in Tabs)
                {
                    if (item is ViewModelBase<IPortfolio> viewModel)
                    {
                        viewModel.UpdateData(portfolio, tabItem => removableTabs.Add(tabItem));
                    }
                }

                if (removableTabs.Any())
                {
                    foreach (var tab in removableTabs)
                    {
                        _ = Tabs.Remove(tab);
                    }

                    removableTabs.Clear();
                }
            }
        }

        private void LoadTabFunc(Object obj)
        {
            if (obj is NameData name)
            {
                Tabs.Add(new SelectedSingleDataViewModel(DataStore, UpdateDataCallback, ReportLogger, fFileService, fDialogCreationService, name, TypeOfAccount));
            }
        }
    }
}
