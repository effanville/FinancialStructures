using System.Windows.Controls;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;

namespace FinancePortfolioDatabase.GUI.Windows.Stats
{
    /// <summary>
    /// Interaction logic for StatsCreatorWindow.xaml
    /// </summary>
    public partial class StatsCreatorWindow : Grid
    {
        /// <summary>
        /// Construct an instance.
        /// </summary>
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
