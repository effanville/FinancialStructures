using FinancialStructures.ReportLogging;
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
        public HelpWindow(LogReporter reportLogger)
        {
            InitializeComponent();
            string helpPath = Path.GetFullPath("Help\\help.html");
            if (!File.Exists(helpPath))
            {
                reportLogger.Log("Error", "Help","Could not find help documentation.");
                return;
            }
            Uri path = new Uri(helpPath);
            webBrowser.Source = path;
        }
    }
}
