using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace FinancePortfolioDatabase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Constructor for the application.
        /// </summary>
        public App()
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                typeof(Timeline),
                new FrameworkPropertyMetadata { DefaultValue = 10 }
                );
        }

        /// <summary>
        /// This fires on startup of the application. Used to set the culture of the program.
        /// </summary>
        public void Application_Startup(object sender, StartupEventArgs e)
        {
            if (CultureInfo.CurrentUICulture.IetfLanguageTag == "en-US")
            {
                CultureInfo UKEnglishCulture = new CultureInfo("en-GB");
                FrameworkElement.LanguageProperty.OverrideMetadata(
                    typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(UKEnglishCulture.IetfLanguageTag)));
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }
            else
            {
                FrameworkElement.LanguageProperty.OverrideMetadata(
                    typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag)));
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }
        }

        /// <summary>
        /// Fires when unhandled exceptions occur. Opens a dialog box, and hopefully the program continues.
        /// </summary>
        public void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //Handling the exception within the UnhandledExcpeiton handler.
            _ = MessageBox.Show(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace, "Exception Caught", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            _ = MessageBox.Show(ex.Message, "Uncaught Thread Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
