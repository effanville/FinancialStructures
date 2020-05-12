using FinanceCommonViewModels;
using System.Windows.Controls;

namespace FinanceWindows
{
    /// <summary>
    /// Interaction logic for SingleValueEditWindow.xaml
    /// </summary>
    public partial class SingleValueEditWindow : Grid
    {
        public SingleValueEditWindow()
        {
            InitializeComponent();
        }

        private void CloseTabCommand(object sender, System.Windows.RoutedEventArgs e)
        {
            var VM = DataContext as SingleValueEditWindowViewModel;
            if (TabMain.SelectedIndex != 0)
            {
                VM.Tabs.RemoveAt(TabMain.SelectedIndex);
            }
        }
    }
}
