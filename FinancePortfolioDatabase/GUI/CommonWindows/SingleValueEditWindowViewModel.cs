using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.ReportLogging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FinanceCommonViewModels
{
    internal class SingleValueEditWindowViewModel : ViewModelBase
    {
        private Portfolio Portfolio;
        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        private readonly Action<Action<Portfolio>> UpdateDataCallback;
        private readonly LogReporter ReportLogger;
        private readonly EditMethods EditMethods;

        public SingleValueEditWindowViewModel(string title, Portfolio portfolio, Action<Action<Portfolio>> updateDataCallback, LogReporter reportLogger, EditMethods editMethods)
            : base(title)
        {
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            EditMethods = editMethods;
            UpdateData(portfolio);
            Tabs.Add(new DataNamesViewModel(Portfolio, updateDataCallback, reportLogger, LoadTab, editMethods));
        }

        public override void UpdateData(Portfolio portfolio)
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
            Tabs.Add(new SelectedSingleDataViewModel(Portfolio, UpdateDataCallback, ReportLogger, EditMethods, name));
        }
    }
}
