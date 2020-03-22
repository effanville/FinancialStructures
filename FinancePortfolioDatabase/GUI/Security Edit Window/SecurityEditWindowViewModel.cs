using FinanceCommonViewModels;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.ReportLogging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FinanceWindowsViewModels
{
    internal class SecurityEditWindowViewModel : ViewModelBase
    {
        private IPortfolio Portfolio;

        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        private readonly LogReporter ReportLogger;
        private readonly Action<Action<IPortfolio>> UpdateDataAction;

        public SecurityEditWindowViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, LogReporter reportLogger)
    : base("Security Edit")
        {
            Portfolio = portfolio;
            UpdateDataAction = updateData;
            ReportLogger = reportLogger;
            Tabs.Add(new SecurityNamesViewModel(Portfolio, updateData, ReportLogger, LoadTab));
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            Portfolio = portfolio;
            if (Tabs != null)
            {
                for (int tabIndex = 0; tabIndex < Tabs.Count; tabIndex++)
                {
                    if (Tabs[tabIndex] is ViewModelBase viewModel)
                    {
                        viewModel.UpdateData(portfolio, removeTab);
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

        private List<object> removableTabs = new List<object>();

        private Action<object> removeTab => tabItem => removableTabs.Add(tabItem);

        private Action<NameData_ChangeLogged> LoadTab => (name) => LoadTabFunc(name);

        private void LoadTabFunc(NameData_ChangeLogged name)
        {
            Tabs.Add(new SelectedSecurityViewModel(Portfolio, UpdateDataAction, ReportLogger, name));
        }
    }
}
