using FinanceWindowsViewModels;
using FinancialStructures.GUIFinanceStructures;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinanceWindows
{
    /// <summary>
    /// Interaction logic for SecurityNamesView.xaml
    /// </summary>
    public partial class SecurityNamesView : UserControl
    {
        public SecurityNamesView()
        {
            InitializeComponent();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid namesGrid)
            {
                var current = namesGrid.CurrentItem;
                if (current is NameData data)
                {
                    var VM = this.DataContext as SecurityNamesViewModel;
                    VM.LoadSelectedTab(data);
                }
            }
        }
    }
}
