using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using Common.Structure.Reporting;
using Common.UI.ViewModelBases;
using Common.UI;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;

namespace FinancePortfolioDatabase.GUI.ViewModels.Security
{
    /// <summary>
    /// View model 
    /// </summary>
    public class SecurityEditWindowViewModel : DataDisplayViewModelBase
    {
        /// <summary>
        /// The tabs to display, with the Security names, and 
        /// selected security data.
        /// </summary>
        public ObservableCollection<object> Tabs
        {
            get;
            set;
        } = new ObservableCollection<object>();

        private int fSelectedIndex;

        /// <summary>
        /// Index of the selected tab.
        /// </summary>
        public int SelectedIndex
        {
            get => fSelectedIndex;
            set => SetAndNotify(ref fSelectedIndex, value, nameof(SelectedIndex));
        }

        private readonly IReportLogger ReportLogger;
        private readonly UiGlobals fUiGlobals;

        private readonly Action<Action<IPortfolio>> UpdateDataAction;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SecurityEditWindowViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, IReportLogger reportLogger, UiStyles styles, UiGlobals globals)
            : base(styles, "Securities", Account.Security, portfolio)
        {
            UpdateDataAction = updateData;
            ReportLogger = reportLogger;
            fUiGlobals = globals;
            Tabs.Add(new DataNamesViewModel(DataStore, updateData, ReportLogger, styles, (name) => LoadTabFunc(name), Account.Security));
            SelectedIndex = 0;
        }

        /// <inheritdoc/>
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
                Tabs.Add(new SelectedSecurityViewModel(DataStore, UpdateDataAction, ReportLogger, Styles, fUiGlobals, name));
            }
        }
    }
}
