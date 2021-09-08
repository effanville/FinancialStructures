using System.Windows;
using System.Windows.Controls;
using FinancePortfolioDatabase.GUI.ViewModels;
using FinancePortfolioDatabase.GUI.ViewModels.Common;

namespace FinancePortfolioDatabase.GUI.Windows
{
    /// <summary>
    /// Interaction logic for MainUserControl.xaml
    /// </summary>
    public partial class MainUserControl : UserControl
    {
        /// <summary>
        /// Construct an instance.
        /// </summary>
        public MainUserControl()
        {
            InitializeComponent();
        }

        private void CloseTabCommand(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel VM = DataContext as MainWindowViewModel;
            if (MainTabControl.SelectedIndex != 0)
            {
                if (VM.Tabs[MainTabControl.SelectedIndex] is DataDisplayViewModelBase vmBase && vmBase.Closable)
                {
                    VM.Tabs.RemoveAt(MainTabControl.SelectedIndex);
                }
            }
        }
    }
}
