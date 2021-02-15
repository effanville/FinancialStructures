using System.Windows.Controls;
using FinancialStructures.NamingStructures;

namespace FinanceCommonWindows
{
    /// <summary>
    /// Interaction logic for DataNamesView.xaml
    /// </summary>
    public partial class DataNamesView : UserControl
    {
        /// <summary>
        /// Construct an instance.
        /// </summary>
        public DataNamesView()
        {
            InitializeComponent();
        }

        private void MyList_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (sender is DataGrid dg)
            {
                if (dg.CurrentItem != null && dg.CurrentItem is NameCompDate name)
                {
                    if (!name.Equals(new NameCompDate()))
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
    }
}
