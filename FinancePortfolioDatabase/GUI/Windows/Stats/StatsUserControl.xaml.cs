using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using Common.UI.DisplayObjects;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using FinancialStructures.Statistics;

namespace FinancePortfolioDatabase.GUI.Windows.Stats
{
    /// <summary>
    /// Interaction logic for StatsUserControl.xaml
    /// </summary>
    public partial class StatsUserControl : AutoGenColumnControl
    {
        /// <summary>
        /// Construct an instance.
        /// </summary>
        public StatsUserControl()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is StatsViewModel vm)
            {
                vm.PropertyChanged += UpdateDataGrid;
            }

            UpdateDataGrid(null, null);
        }

        /// <summary>
        /// Updates the data displayed in the grid.
        /// </summary>
        private void UpdateDataGrid(object sender, PropertyChangedEventArgs e)
        {
            if (DataContext is StatsViewModel vm)
            {
                DataTable dt = new DataTable();

                // First add the column header names from the statistics names.
                List<IStatistic> stat = vm.Stats.First().Statistics;
                foreach (IStatistic value in stat)
                {
                    dt.Columns.Add(new DataColumn(value.StatType.ToString(), value.IsNumeric ? typeof(double) : typeof(string)));
                }

                // Now add the statistics values.
                foreach (AccountStatistics val in vm.Stats)
                {
                    _ = dt.Rows.Add(val.StatValuesAsObjects.ToArray());
                }

                StatsBox.ItemsSource = dt.DefaultView;
            }
        }
    }
}
