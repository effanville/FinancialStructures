using System;
using System.Windows.Input;
using FinancialStructures.FinanceInterfaces;
using Microsoft.Win32;
using UICommon.Commands;

namespace FinanceViewModels.StatsViewModels
{
    internal class HtmlStatsViewerViewModel : TabViewModelBase
    {

        private Uri fDisplayStats;

        public Uri DisplayStats
        {
            get
            {
                return fDisplayStats;
            }
            set
            {
                fDisplayStats = value;
                OnPropertyChanged();
            }
        }

        private string fStatsFilepath;

        public string StatsFilepath
        {
            get
            {
                return fStatsFilepath;
            }
            set
            {
                fStatsFilepath = value;
                OnPropertyChanged();
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

        public HtmlStatsViewerViewModel(IPortfolio portfolio, bool displayValueFunds, string filePath)
: base(portfolio, displayValueFunds)
        {
            StatsFilepath = filePath;
            Header = "Exported Stats";
            GenerateStatistics(displayValueFunds);
            FileSelect = new RelayCommand(ExecuteFileSelect);
        }
    }
}
