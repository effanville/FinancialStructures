using FinanceWindowsViewModels;
using FinancialStructures.NamingStructures;
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
                if (current is NameData_ChangeLogged data)
                {
                    var VM = this.DataContext as SecurityNamesViewModel;
                    VM.LoadSelectedTab(data);
                }
            }
        }
    }
}
