using System;
using System.IO;
using System.Reflection;
using System.Windows;
using FinanceWindowsViewModels;
using UICommon.Services;

namespace FinanceWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IFileInteractionService fFileInteractionService;
        private readonly IDialogCreationService fDialogCreationService;

        /// <summary>
        /// Construct an instance of the main window.
        /// </summary>
        public MainWindow()
        {
            fFileInteractionService = new FileInteractionService(this);
            fDialogCreationService = new DialogCreationService(this);
            MainWindowViewModel viewModel = new MainWindowViewModel(fFileInteractionService, fDialogCreationService);
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Title = "Financial Database v" + version.ToString();

            DataContext = viewModel;
        }

        public void PrintErrorLog(Exception exception)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                var result = fFileInteractionService.SaveFile("log", string.Empty, viewModel.ProgramPortfolio.Directory, filter: "log Files|*.log|All Files|*.*");
                if (result.Success != null && (bool)result.Success)
                {
                    using (var stream = new StreamWriter(result.FilePath))
                    {
                        foreach (var report in viewModel.ApplicationLog.GetReports())
                        {
                            stream.WriteLine(report.ToString());
                        }

                        stream.WriteLine(exception.Message);
                        stream.WriteLine(exception.StackTrace);
                    }
                }
            }
        }

        /// <summary>
        /// Event fires when one closes the window. Checks if the user wishes to save or not.
        /// </summary>
        /// <remarks>
        /// This should really check if the data has changed or not, but this
        /// is not currently possible.
        /// </remarks>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindowViewModel VM = DataContext as MainWindowViewModel;
            MessageBoxResult result;
            if (VM.ProgramPortfolio.IsAlteredSinceSave)
            {
                result = fDialogCreationService.ShowMessageBox("Data has changed since last saved. Would you like to save changes before closing?", $"Closing {Title}.", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            }
            else
            {
                result = fDialogCreationService.ShowMessageBox("There is a small chance that the data has changed since last save (due to neglect on my part). Would you like to save before closing?", $"Closing {Title}.", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            }
            if (result == MessageBoxResult.Yes)
            {
                FileInteractionResult savingResult = fFileInteractionService.SaveFile("xml", VM.ProgramPortfolio.DatabaseName + VM.ProgramPortfolio.Extension, VM.ProgramPortfolio.Directory, "XML Files|*.xml|All Files|*.*");
                if (savingResult.Success != null && (bool)savingResult.Success)
                {
                    VM.ProgramPortfolio.SetFilePath(savingResult.FilePath);
                    MainWindowViewModel vm = DataContext as MainWindowViewModel;
                    vm.ProgramPortfolio.SavePortfolio(savingResult.FilePath, vm.ReportLogger);
                }
            }
            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
