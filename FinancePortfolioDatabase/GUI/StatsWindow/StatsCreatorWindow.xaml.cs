using FinanceWindowsViewModels;
using System.Windows.Controls;

namespace FinanceWindows
{
    /// <summary>
    /// Interaction logic for StatsCreatorWindow.xaml
    /// </summary>
    public partial class StatsCreatorWindow : Grid
    {
        public StatsCreatorWindow()
        {
            InitializeComponent();
        }

        private void CloseTabCommand(object sender, System.Windows.RoutedEventArgs e)
        {
            var VM = this.DataContext as StatsCreatorWindowViewModel;
            if (TabMain.SelectedIndex != 0)
            {
                VM.StatsTabs.RemoveAt(TabMain.SelectedIndex);
            }
        }
    }
}
