using System;
using System.Windows.Input;
using Common.UI;
using Common.UI.Commands;
using Common.UI.Services;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    internal class HtmlStatsViewerViewModel : DataDisplayViewModelBase
    {
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
            FileInteractionResult result = fUiGlobals.FileInteractionService.OpenFile("html", filter: "HTML file|*.html;*.htm|All files|*.*");
            if (result.Success != null && (bool)result.Success)
            {
                StatsFilepath = result.FilePath;
            }
        }

        public HtmlStatsViewerViewModel(UiStyles styles, UiGlobals globals, string filePath)
            : base(globals, styles, null, "Exported Stats", Account.All)
        {
            StatsFilepath = filePath;
            FileSelect = new RelayCommand(ExecuteFileSelect);
        }
    }
}
