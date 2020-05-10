using FinanceCommonViewModels;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.Reporting;
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

        public SecurityEditWindowViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, EditMethods securityEditMethods, IReportLogger reportLogger, IFileInteractionService fileService, IDialogCreationService dialogCreation)
            : base("Security Edit", portfolio)
        {
            UpdateDataAction = updateData;
            ReportLogger = reportLogger;
            fFileService = fileService;
            fDialogCreationService = dialogCreation;
            LoadSelectedTab = (name) => LoadTabFunc(name);
            Tabs.Add(new DataNamesViewModel(DataStore, updateData, ReportLogger, LoadSelectedTab, securityEditMethods));
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            base.UpdateData(portfolio);
            List<object> removableTabs = new List<object>();
            if (Tabs != null)
            {
                for (int tabIndex = 0; tabIndex < Tabs.Count; tabIndex++)
                {
                    if (Tabs[tabIndex] is ViewModelBase<IPortfolio> viewModel)
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
            if (obj is NameData_ChangeLogged name)
            {
                Tabs.Add(new SelectedSecurityViewModel(DataStore, UpdateDataAction, ReportLogger, fFileService, fDialogCreationService, name));
            }
        }
    }
}
