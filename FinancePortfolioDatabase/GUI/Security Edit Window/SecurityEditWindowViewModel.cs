using FinanceCommonViewModels;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using SavingClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FinanceWindowsViewModels
{
    internal class SecurityEditWindowViewModel : ViewModelBase
    {
        private Portfolio Portfolio;

        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        private readonly Action<string, string, string> ReportLogger;
        private readonly Action<Action<AllData>> UpdateDataAction;

        public SecurityEditWindowViewModel(Portfolio portfolio, Action<Action<AllData>> updateData, Action<string, string, string> reportLogger)
    : base("Security Edit")
        {
            Portfolio = portfolio;
            UpdateDataAction = updateData;
            ReportLogger = reportLogger;
            Tabs.Add(new SecurityNamesViewModel(Portfolio, updateData, ReportLogger, LoadTab));
        }

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
        {
            Portfolio = portfolio;
            if (Tabs != null)
            {
                foreach (var item in Tabs)
                {
                    if (item is ViewModelBase viewModel)
                    {
                        viewModel.UpdateData(portfolio, sectors);
                    }
                }
            }
        }

        private Action<NameData> LoadTab => (name) => LoadTabFunc(name);

        private void LoadTabFunc(NameData name)
        {
            Tabs.Add(new SelectedSecurityViewModel(Portfolio, UpdateDataAction, ReportLogger, name));
        }
    }
}
