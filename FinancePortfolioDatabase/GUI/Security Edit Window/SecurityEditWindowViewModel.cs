using FinanceCommonViewModels;
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

namespace FinanceWindowsViewModels
{
    internal class SecurityEditWindowViewModel : ViewModelBase<IPortfolio>
    {
        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        private readonly IReportLogger ReportLogger;
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogCreationService;

        private readonly Action<Action<IPortfolio>> UpdateDataAction;

        public SecurityEditWindowViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, IReportLogger reportLogger, IFileInteractionService fileService, IDialogCreationService dialogCreation)
            : base("Security Edit", portfolio)
        {
            UpdateDataAction = updateData;
            ReportLogger = reportLogger;
            fFileService = fileService;
            fDialogCreationService = dialogCreation;
            Tabs.Add(new DataNamesViewModel(DataStore, updateData, ReportLogger, (name) => LoadTabFunc(name), AccountType.Security));
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            base.UpdateData(portfolio);
            List<object> removableTabs = new List<object>();
            if (Tabs != null)
            {
                for (int tabIndex = 0; tabIndex < Tabs.Count; tabIndex++)
                {
                    if (Tabs[tabIndex] is TabViewModelBase<IPortfolio> viewModel)
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

        internal void LoadTabFunc(Object obj)
        {
            if (obj is NameData name)
            {
                Tabs.Add(new SelectedSecurityViewModel(DataStore, UpdateDataAction, ReportLogger, fFileService, fDialogCreationService, name));
            }
        }
    }
}
