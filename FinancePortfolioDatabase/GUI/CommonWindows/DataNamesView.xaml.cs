using FinanceCommonViewModels;
using FinancialStructures.NamingStructures;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinanceCommonWindows
{
    /// <summary>
    /// Interaction logic for DataNamesView.xaml
    /// </summary>
    public partial class DataNamesView : UserControl
    {
        public DataNamesView()
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
                    var VM = this.DataContext as ViewModelBase;
                    VM.LoadSelectedTab(data);
                }
            }
        }
    }
}
