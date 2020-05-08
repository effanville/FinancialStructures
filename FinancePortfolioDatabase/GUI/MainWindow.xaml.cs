using FinanceWindowsViewModels;
using FinancialStructures.PortfolioAPI;
using UICommon.Services;
using System;
using System.Reflection;
using System.Windows;

namespace FinanceWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IFileInteractionService fFileInteractionService;
        private readonly IDialogCreationService fDialogCreationService;

        public MainWindow()
        {
            fFileInteractionService = new FileInteractionService(this);
            fDialogCreationService = new DialogCreationService(this);
            var viewModel = new MainWindowViewModel(fFileInteractionService, fDialogCreationService);
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Title = "Financial Database v" + version.ToString();

            DataContext = viewModel;
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
            MessageBoxResult result = fDialogCreationService.ShowMessageBox("Data may not be saved. Would you like to save before closing?", $"Closing {Title}.", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            var VM = DataContext as MainWindowViewModel;
            if (result == MessageBoxResult.Yes)
            {
                var savingResult = fFileInteractionService.SaveFile("xml", VM.ProgramPortfolio.DatabaseName + VM.ProgramPortfolio.Extension, VM.ProgramPortfolio.Directory, "XML Files|*.xml|All Files|*.*");
                if (savingResult.Success != null && (bool)savingResult.Success)
                {
                    VM.ProgramPortfolio.SetFilePath(savingResult.FilePath);
                    var vm = DataContext as MainWindowViewModel;
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
