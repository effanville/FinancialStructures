using System.Windows;
using UICommon.Interfaces;

namespace UICommon.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public sealed partial class DialogWindow : Window, ICloseable
    {
        public DialogWindow()
        {
            InitializeComponent();
        }
    }
}
