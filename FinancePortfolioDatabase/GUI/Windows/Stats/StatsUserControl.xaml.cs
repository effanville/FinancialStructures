using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using Common.UI.DisplayObjects;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;

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
