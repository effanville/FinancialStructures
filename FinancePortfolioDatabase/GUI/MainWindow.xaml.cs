using FinanceWindowsViewModels;
using FinancialStructures.PortfolioAPI;
using GUISupport.Services;
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
        private static class AssemblyCreationDate
        {
            public static readonly DateTime Value;

            static AssemblyCreationDate()
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                Value = new DateTime(2000, 1, 1, 20, 24, 30).AddDays(version.Build).AddSeconds(version.MinorRevision * 2);
            }
        }

        private readonly IFileInteractionService fFileInteractionService;
        private readonly IDialogCreationService fDialogCreationService;

        public MainWindow()
        {
            fFileInteractionService = new FileInteractionService(this);
            fDialogCreationService = new DialogCreationService(this);
            var viewModel = new MainWindowViewModel(fFileInteractionService, fDialogCreationService);
            InitializeComponent();

            Title = "Financial Database v" + AssemblyCreationDate.Value.ToString("yyyy.MM.dd.HHmmss");

            DataContext = viewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = fDialogCreationService.ShowMessageBox(this, "Data may not be saved. Would you like to save before closing?", $"Closing {Title}.", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
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

                // saving.Dispose();
            }
            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
