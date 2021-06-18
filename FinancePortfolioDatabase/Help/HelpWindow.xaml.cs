using System;
using System.IO;
using System.Windows;
using StructureCommon.Reporting;

namespace FinanceWindows
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        /// <summary>
        /// Construct an instance.
        /// </summary>
        /// <param name="reportLogger"></param>
        public HelpWindow(IReportLogger reportLogger)
        {
            InitializeComponent();
            string helpPath = Path.GetFullPath("Help\\help.html");
            if (!File.Exists(helpPath))
            {
                _ = reportLogger.LogUseful(ReportType.Error, ReportLocation.Help, "Could not find help documentation.");
                return;
            }

            Uri path = new Uri(helpPath);
            webBrowser.Source = path;
        }
    }
}
