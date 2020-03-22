using FinancialStructures.DatabaseInterfaces;
using FinancialStructures.DataStructures;
using FinancialStructures.PortfolioAPI;
using System.Collections.Generic;

namespace FinanceViewModels.StatsViewModels
{
    internal class SecurityInvestmentViewModel : TabViewModelBase
    {
        private List<DayValue_Named> fSecuritiesInvestments;

        public List<DayValue_Named> SecuritiesInvestments
        {
            get { return fSecuritiesInvestments; }
            set { fSecuritiesInvestments = value; OnPropertyChanged(); }
        }

        public override void GenerateStatistics(bool displayValueFunds)
        {
            DisplayValueFunds = displayValueFunds;
            SecuritiesInvestments = fPortfolio.AllSecuritiesInvestments();
        }
        public SecurityInvestmentViewModel(IPortfolio portfolio, bool displayValueFunds)
            : base(portfolio, displayValueFunds)
        {
            Header = "Security Investments";
            GenerateStatistics(displayValueFunds);
        }
    }
}
