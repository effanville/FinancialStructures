using System.Windows;
using System.Windows.Controls;
using FinancePortfolioDatabase.GUI.ViewModels.Security;

namespace FinancePortfolioDatabase.GUI.Windows.Security
{
    /// <summary>
    /// Interaction logic for SecurityEditWindow.xaml
    /// </summary>
    public partial class SecurityEditWindow : Grid
    {
        /// <summary>
        /// Construct an instance.
        /// </summary>
        public SecurityEditWindow()
        {
            InitializeComponent();
        }

        private void CloseTabCommand(object sender, RoutedEventArgs e)
        {
            SecurityEditWindowViewModel VM = DataContext as SecurityEditWindowViewModel;
            if (TabMain.SelectedIndex != 0)
            {
                VM.Tabs.RemoveAt(TabMain.SelectedIndex);
            }
        }
    }
}
