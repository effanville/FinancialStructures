using System;
using System.Windows.Input;
using FinancialStructures.Database;
using Microsoft.Win32;
using Common.UI.Commands;
using FinancePortfolioDatabase.GUI.ViewModels.Common;

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
            OpenFileDialog fileSelect = new OpenFileDialog
            {
                Filter = "HTML file|*.html;*.htm|All files|*.*"
            };

            bool? saved = fileSelect.ShowDialog();
            if (saved != null && (bool)saved)
            {
                StatsFilepath = fileSelect.FileName;
            }
        }

        public HtmlStatsViewerViewModel(IPortfolio portfolio, string filePath)
            : base("Exported Stats", Account.All, portfolio)
        {
            StatsFilepath = filePath;
            FileSelect = new RelayCommand(ExecuteFileSelect);
        }
    }
}
