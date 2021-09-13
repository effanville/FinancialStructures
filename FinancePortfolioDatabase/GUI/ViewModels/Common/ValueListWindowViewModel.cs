using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common.UI;
using Common.UI.ViewModelBases;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancePortfolioDatabase.GUI.ViewModels.Common
{
    /// <summary>
    /// A view model for displaying a collect of <see cref="FinancialStructures.FinanceStructures.IValueList"/>
    /// </summary>
    public class ValueListWindowViewModel : DataDisplayViewModelBase
    {
        private readonly UiGlobals fUiGlobals;
        private readonly Action<Action<IPortfolio>> UpdateDataCallback;

        /// <summary>
        /// The tabs to display.
        /// </summary>
        public ObservableCollection<object> Tabs { get; set; } = new ObservableCollection<object>();

        private int fSelectedIndex;

        /// <summary>
        /// Index of the selected tab.
        /// </summary>
        public int SelectedIndex
        {
            get => fSelectedIndex;
            set => SetAndNotify(ref fSelectedIndex, value, nameof(SelectedIndex));
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ValueListWindowViewModel(string title, IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, UiStyles styles, UiGlobals globals, Account accountType)
            : base(styles, title, accountType, portfolio)
        {
            fUiGlobals = globals;
            UpdateDataCallback = updateDataCallback;
            UpdateData(portfolio);
            Tabs.Add(new DataNamesViewModel(DataStore, updateDataCallback, fUiGlobals.ReportLogger, styles, (name) => LoadTabFunc(name), accountType));

            SelectedIndex = 0;
        }

        /// <inheritdoc/>
        public override void UpdateData(IPortfolio dataToDisplay)
        {
            base.UpdateData(dataToDisplay);
            List<object> removableTabs = new List<object>();
            if (Tabs != null)
            {
                foreach (object item in Tabs)
                {
                    if (item is TabViewModelBase<IPortfolio> viewModel)
                    {
                        viewModel.UpdateData(dataToDisplay, tabItem => removableTabs.Add(tabItem));
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
                Tabs.Add(new SelectedSingleDataViewModel(DataStore, UpdateDataCallback, Styles, fUiGlobals, name, DataType));
            }
        }
    }
}
