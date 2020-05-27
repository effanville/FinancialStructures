using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;

namespace FinanceViewModels.StatsViewModels
{
    internal class SecurityInvestmentViewModel : TabViewModelBase
    {
        private List<DayValue_Named> fSecuritiesInvestments;

        public List<DayValue_Named> SecuritiesInvestments
        {
            get
            {
                return fSecuritiesInvestments;
            }
            set
            {
                fSecuritiesInvestments = value;
                OnPropertyChanged();
            }
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
