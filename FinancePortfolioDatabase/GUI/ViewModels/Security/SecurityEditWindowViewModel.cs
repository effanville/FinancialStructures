using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using Common.Structure.Reporting;
using Common.UI.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels.Security
{
    public class SecurityEditWindowViewModel : DataDisplayViewModelBase
    {
        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        private readonly IReportLogger ReportLogger;
        private readonly UiGlobals fUiGlobals;

        private readonly Action<Action<IPortfolio>> UpdateDataAction;

        public SecurityEditWindowViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, IReportLogger reportLogger, UiGlobals globals)
            : base("Securities", Account.Security, portfolio)
        {
            UpdateDataAction = updateData;
            ReportLogger = reportLogger;
            fUiGlobals = globals;
            Tabs.Add(new DataNamesViewModel(DataStore, updateData, ReportLogger, (name) => LoadTabFunc(name), Account.Security));
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
                    foreach (object tab in removableTabs)
                    {
                        _ = Tabs.Remove(tab);
                    }

                    removableTabs.Clear();
                }
            }
        }

        internal void LoadTabFunc(object obj)
        {
            if (obj is NameData name)
            {
                Tabs.Add(new SelectedSecurityViewModel(DataStore, UpdateDataAction, ReportLogger, fUiGlobals, name));
            }
        }
    }
}
