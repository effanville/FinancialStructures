using FinancialStructures.ReportingStructures;
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
        public HelpWindow()
        {
            var reports = new ErrorReports();
            InitializeComponent();
            string helpPath = Path.GetFullPath("GUI\\GuiSupport\\help.html");
            if (!File.Exists(helpPath))
            {
                reports.AddError("Could not find help documentation.");
                return;
            }
            Uri path = new Uri(helpPath);
            webBrowser.Source = path;
        }
    }
}
