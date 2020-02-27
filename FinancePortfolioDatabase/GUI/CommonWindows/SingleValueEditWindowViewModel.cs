﻿using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
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
            Tabs.Add(new SelectedSingleDataViewModel(Portfolio, Sectors, UpdateDataCallback, UpdateReports, EditMethods, name));
        }

        Action<Action<AllData>> UpdateDataCallback;
        Action<ErrorReports> UpdateReports;
        EditMethods EditMethods;

        public SingleValueEditWindowViewModel(string title, Portfolio portfolio, List<Sector> sectors, Action<Action<AllData>> updateDataCallback, Action<ErrorReports> updateReports, EditMethods editMethods)
            : base(title)
        {
            UpdateDataCallback = updateDataCallback;
            UpdateReports = updateReports;
            EditMethods = editMethods;
            UpdateData(portfolio, sectors);
            Tabs.Add(new DataNamesViewModel(Portfolio, sectors, updateDataCallback, updateReports, loadTab, editMethods));
        }
    }
}
