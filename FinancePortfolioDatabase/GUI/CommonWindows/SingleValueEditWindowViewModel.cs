using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Reporting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FinanceCommonViewModels
{
    internal class SingleValueEditWindowViewModel : ViewModelBase
    {
        private AccountType TypeOfAccount;
        private IPortfolio Portfolio;
        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        private readonly Action<Action<IPortfolio>> UpdateDataCallback;
        private readonly IReportLogger ReportLogger;
        private readonly EditMethods EditMethods;

        public SingleValueEditWindowViewModel(string title, IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, IReportLogger reportLogger, EditMethods editMethods, AccountType accountType)
            : base(title)
        {
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            EditMethods = editMethods;
            TypeOfAccount = accountType;
            UpdateData(portfolio);
            Tabs.Add(new DataNamesViewModel(Portfolio, updateDataCallback, reportLogger, LoadTab, editMethods));
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            Portfolio = portfolio;
            if (Tabs != null)
            {
                foreach (var item in Tabs)
                {
                    if (item is ViewModelBase viewModel)
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
            Tabs.Add(new SelectedSingleDataViewModel(Portfolio, UpdateDataCallback, ReportLogger, EditMethods, name, TypeOfAccount));
        }
    }
}
