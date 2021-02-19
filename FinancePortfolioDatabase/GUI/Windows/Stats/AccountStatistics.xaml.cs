using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using FinanceWindows.StatsWindow;

namespace FinancePortfolioDatabase.GUI.Windows.Stats
{
    /// <summary>
    /// Interaction logic for AccountStatistics.xaml
    /// </summary>
    internal partial class AccountStatistics : AutoGenColumnControl
    {
        public AccountStatistics()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is AccountStatisticsViewModel vm)
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
            if (DataContext is AccountStatisticsViewModel vm)
            {
                var dt = new DataTable();

                // First add the column header names from the statistics names.
                var stat = vm.Stats.First().Statistics;
                foreach (var value in stat)
                {
                    dt.Columns.Add(new DataColumn(value.StatType.ToString(), value.IsNumeric ? typeof(double) : typeof(string)));
                }

                // Now add the statistics values.
                foreach (var val in vm.Stats)
                {
                    _ = dt.Rows.Add(val.StatValuesAsObjects.ToArray());
                }

                StatsBox.ItemsSource = dt.DefaultView;
            }
        }
    }
}
