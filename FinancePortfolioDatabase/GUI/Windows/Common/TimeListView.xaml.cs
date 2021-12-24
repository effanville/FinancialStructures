using System.Windows.Controls;
using System.Windows.Input;
using FinancePortfolioDatabase.GUI.ViewModels.Common;

namespace FinancePortfolioDatabase.GUI.Windows
{
    /// <summary>
    /// Interaction logic for TimeListView.xaml
    /// </summary>
    public partial class TimeListView : ContentControl
    {
        /// <summary>
        /// Construct an instance.
        /// </summary>
        public TimeListView()
        {
            InitializeComponent();
        }

        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (e.OriginalSource is DataGridCell)
                {
                    if (DataContext != null && DataContext is TimeListViewModel vm)
                    {
                        vm.DeleteValuation();
                    }
                }
            }
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            if (DataContext != null && DataContext is TimeListViewModel vm)
            {
                e.NewItem = vm.DefaultNewItem();
            }
        }
    }
}
