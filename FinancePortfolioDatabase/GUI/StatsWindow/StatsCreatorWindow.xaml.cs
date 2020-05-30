using System.Windows.Controls;
using FinanceWindowsViewModels;

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
            StatsCreatorWindowViewModel VM = DataContext as StatsCreatorWindowViewModel;
            if (TabMain.SelectedIndex != 0)
            {
                VM.StatsTabs.RemoveAt(TabMain.SelectedIndex);
            }
        }
    }
}
