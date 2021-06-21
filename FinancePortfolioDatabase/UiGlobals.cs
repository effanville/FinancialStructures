using System.IO.Abstractions;
using StructureCommon.Reporting;
using UICommon.Services;

namespace FinancePortfolioDatabase
{
    public class UiGlobals
    {
        /// <summary>
        /// The current working directory for the application.
        /// </summary>
        public string CurrentWorkingDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// The current dispatcher for the system.
        /// </summary>
        public IDispatcher CurrentDispatcher
        {
            get;
        }

        /// <summary>
        /// The current filesystem for the application.
        /// </summary>
        public IFileSystem CurrentFileSystem
        {
            get;
        }

        public IFileInteractionService FileInteractionService
        {
            get;
        }

        public IDialogCreationService DialogCreationService
        {
            get;
        }

        public IReportLogger ReportLogger
        {
            get;
            set;
        }

        public UiGlobals(string currentWorkingDirectory, IDispatcher currentDispatcher, IFileSystem currentFileSystem, IFileInteractionService fileInteractionService, IDialogCreationService dialogCreationService, IReportLogger reportLogger)
        {
            CurrentWorkingDirectory = currentWorkingDirectory;
            CurrentDispatcher = currentDispatcher;
            CurrentFileSystem = currentFileSystem;
            FileInteractionService = fileInteractionService;
            DialogCreationService = dialogCreationService;
            ReportLogger = reportLogger;
        }
    }
}
