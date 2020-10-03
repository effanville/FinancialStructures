using System.Collections.Generic;
using FinancialStructures.Database.Statistics;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;

namespace FinanceViewModels.StatsViewModels
{
    internal class BankAccStatsViewModel : TabViewModelBase
    {
        private List<DayValue_Named> fBankAccountStats;
        public List<DayValue_Named> BankAccountStats
        {
            get
            {
                return fBankAccountStats;
            }
            set
            {
                fBankAccountStats = value;
                OnPropertyChanged();
            }
        }

        public override void GenerateStatistics(bool displayValueFunds)
        {
            DisplayValueFunds = displayValueFunds;
            BankAccountStats = fPortfolio.GenerateBankAccountStatistics(DisplayValueFunds);
            if (BankAccountStats.Count > 0)
            {
                BankAccountStats.Add(fPortfolio.GenerateBankAccountTotalStatistics());
            }
        }

        public BankAccStatsViewModel(IPortfolio portfolio, bool displayValueFunds)
            : base(portfolio, displayValueFunds)
        {
            Header = "Bank Account Stats";
            GenerateStatistics(displayValueFunds);
        }
    }
}
