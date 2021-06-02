using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels.Common
{
    /// <summary>
    /// A view model for displaying a collect of <see cref="FinancialStructures.FinanceStructures.IValueList"/>
    /// </summary>
    public class ValueListWindowViewModel : DataDisplayViewModelBase
    {
        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        private readonly Action<Action<IPortfolio>> UpdateDataCallback;
        private readonly IReportLogger ReportLogger;
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogCreationService;

        public ValueListWindowViewModel(string title, IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, IReportLogger reportLogger, UiGlobals globals, Account accountType)
            : base(title, accountType, portfolio)
        {
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            fFileService = globals.FileInteractionService;
            fDialogCreationService = globals.DialogCreationService;
            UpdateData(portfolio);
            Tabs.Add(new DataNamesViewModel(DataStore, updateDataCallback, reportLogger, (name) => LoadTabFunc(name), accountType));
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            base.UpdateData(portfolio);
            List<object> removableTabs = new List<object>();
            if (Tabs != null)
            {
                foreach (object item in Tabs)
                {
                    if (item is TabViewModelBase<IPortfolio> viewModel)
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
                Tabs.Add(new SelectedSingleDataViewModel(DataStore, UpdateDataCallback, ReportLogger, fFileService, fDialogCreationService, name, DataType));
            }
        }
    }
}
