using System;
using System.Windows.Input;
using FinancialStructures.Database;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using Common.UI.Commands;
using Common.UI;
using Common.UI.Services;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    internal class HtmlStatsViewerViewModel : DataDisplayViewModelBase
    {
        private readonly UiGlobals fGlobals;
        public override bool Closable => true;

        private Uri fDisplayStats;

        public Uri DisplayStats
        {
            get => fDisplayStats;
            set => SetAndNotify(ref fDisplayStats, value, nameof(DisplayStats));
        }

        private string fStatsFilepath;

        public string StatsFilepath
        {
            get => fStatsFilepath;
            set
            {
                SetAndNotify(ref fStatsFilepath, value, nameof(StatsFilepath));
                if (value != null)
                {
                    DisplayStats = new Uri(fStatsFilepath);
                }
            }
        }

        public ICommand FileSelect
        {
            get;
        }

        private void ExecuteFileSelect()
        {
            FileInteractionResult result = fGlobals.FileInteractionService.OpenFile("html", filter: "HTML file|*.html;*.htm|All files|*.*");
            if (result.Success != null && (bool)result.Success)
            {
                StatsFilepath = result.FilePath;
            }
        }

        public HtmlStatsViewerViewModel(UiStyles styles, UiGlobals globals, string filePath)
            : base(styles, "Exported Stats", Account.All, null)
        {
            fGlobals = globals;
            StatsFilepath = filePath;
            FileSelect = new RelayCommand(ExecuteFileSelect);
        }
    }
}
