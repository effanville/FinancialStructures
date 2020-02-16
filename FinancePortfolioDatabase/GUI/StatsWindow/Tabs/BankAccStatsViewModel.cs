using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using System.Collections.Generic;

namespace FinanceViewModels.StatsViewModels
{
    internal class BankAccStatsViewModel : TabViewModelBase
    {
        private List<BankAccountStatsHolder> fBankAccountStats;
        public List<BankAccountStatsHolder> BankAccountStats
        {
            get { return fBankAccountStats; }
            set { fBankAccountStats = value; OnPropertyChanged(); }
        }

        public override void GenerateStatistics(bool displayValueFunds)
        {
            DisplayValueFunds = displayValueFunds;
            BankAccountStats = fPortfolio.GenerateBankAccountStatistics(DisplayValueFunds);
        }

        public BankAccStatsViewModel(Portfolio portfolio, bool displayValueFunds)
            : base(portfolio, displayValueFunds)
        {
            Header = "Bank Account Stats";
            GenerateStatistics(displayValueFunds);
        }
    }
}
