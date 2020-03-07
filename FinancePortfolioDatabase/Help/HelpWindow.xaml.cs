using FinancialStructures.Reporting;
using System;
using System.IO;
using System.Windows;

namespace FinanceWindows
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow(Action<string, string, string> reportLogger)
        {
            InitializeComponent();
            string helpPath = Path.GetFullPath("Help\\help.html");
            if (!File.Exists(helpPath))
            {
                reportLogger("Error", "Help","Could not find help documentation.");
                return;
            }
            Uri path = new Uri(helpPath);
            webBrowser.Source = path;
        }
    }
}
