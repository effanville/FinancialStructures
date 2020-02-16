using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using GUISupport;
using SavingClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FinanceCommonViewModels
{
    public class SingleValueEditWindowViewModel : PropertyChangedBase
    {
        private Portfolio Portfolio;
        private List<Sector> Sectors;
        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        public void UpdateListBoxes(Portfolio portfolio, List<Sector> sectors)
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
            Tabs.Add(new SelectedSingleDataViewModel(Portfolio, Sectors, UpdateDataCallback, UpdateReports, EditMethods, name));
        }

        Action<Action<AllData>> UpdateDataCallback;
        Action<ErrorReports> UpdateReports;
        EditMethods EditMethods;

        public SingleValueEditWindowViewModel(Portfolio portfolio, List<Sector> sectors, Action<Action<AllData>> updateDataCallback, Action<ErrorReports> updateReports, EditMethods editMethods)
        {
            UpdateDataCallback = updateDataCallback;
            UpdateReports = updateReports;
            EditMethods = editMethods;
            UpdateListBoxes(portfolio, sectors);
            Tabs.Add(new DataNamesViewModel(Portfolio, sectors, updateDataCallback, updateReports, loadTab, editMethods));
        }
    }
}
