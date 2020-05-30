using System.Windows.Controls;
using FinanceCommonViewModels;

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
            SingleValueEditWindowViewModel VM = DataContext as SingleValueEditWindowViewModel;
            if (TabMain.SelectedIndex != 0)
            {
                VM.Tabs.RemoveAt(TabMain.SelectedIndex);
            }
        }
    }
}
