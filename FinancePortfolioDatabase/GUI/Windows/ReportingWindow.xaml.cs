using System.Windows.Controls;
using System.Windows.Input;
using FinancePortfolioDatabase.GUI.ViewModels;

namespace FinancePortfolioDatabase.GUI.Windows
{
    /// <summary>
    /// Interaction logic for ReportingWindow.xaml
    /// </summary>
    public partial class ReportingWindow : Expander
    {
        /// <summary>
        /// Construct an instance.
        /// </summary>
        public ReportingWindow()
        {
            InitializeComponent();
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (DataContext != null && DataContext is ReportingWindowViewModel vm)
                {
                    vm.DeleteReport();
                }
            }
        }
    }
}
