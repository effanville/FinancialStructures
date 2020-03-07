using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using SavingClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FinanceCommonViewModels
{
    internal class SingleValueEditWindowViewModel : ViewModelBase
    {
        private Portfolio Portfolio;
        private List<Sector> Sectors;
        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
        {
            Portfolio = portfolio;
            Sectors = sectors;
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

        Action<NameData> loadTab => (name) => LoadTabFunc(name);

        private void LoadTabFunc(NameData name)
        {
            Tabs.Add(new SelectedSingleDataViewModel(Portfolio, Sectors, UpdateDataCallback, ReportLogger, EditMethods, name));
        }

        Action<Action<AllData>> UpdateDataCallback;
        Action<string, string, string> ReportLogger;
        EditMethods EditMethods;

        public SingleValueEditWindowViewModel(string title, Portfolio portfolio, List<Sector> sectors, Action<Action<AllData>> updateDataCallback, Action<string, string, string> reportLogger, EditMethods editMethods)
            : base(title)
        {
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            EditMethods = editMethods;
            UpdateData(portfolio, sectors);
            Tabs.Add(new DataNamesViewModel(Portfolio, sectors, updateDataCallback, reportLogger, loadTab, editMethods));
        }
    }
}
