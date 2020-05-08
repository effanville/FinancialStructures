using FinancialStructures.FinanceInterfaces;
using GUISupport;
using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace FinanceViewModels.StatsViewModels
{
    class HtmlStatsViewerViewModel : TabViewModelBase
    {

        private Uri fDisplayStats;

        public Uri DisplayStats
        {
            get { return fDisplayStats; }
            set { fDisplayStats = value; OnPropertyChanged(); }
        }

        private string fStatsFilepath;

        public string StatsFilepath
        {
            get { return fStatsFilepath; }
            set { fStatsFilepath = value; OnPropertyChanged(); if (value != null) { DisplayStats = new Uri(fStatsFilepath); } }
        }

        public ICommand FileSelect { get; }

        private void ExecuteFileSelect(Object obj)
        {
            OpenFileDialog fileSelect = new OpenFileDialog();
            fileSelect.Filter = "HTML file|*.html;*.htm|All files|*.*";

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
            FileSelect = new BasicCommand(ExecuteFileSelect);
        }
    }
}
