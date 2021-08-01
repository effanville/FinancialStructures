using System.Collections.Generic;
using Common.Structure.DataStructures;
using Common.Structure.NamingStructures;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    internal class SecurityInvestmentViewModel : TabViewModelBase
    {
        private List<Labelled<TwoName, DailyValuation>> fSecuritiesInvestments;

        public List<Labelled<TwoName, DailyValuation>> SecuritiesInvestments
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
            SecuritiesInvestments = fPortfolio.TotalInvestments(Totals.Security);
        }
        public SecurityInvestmentViewModel(IPortfolio portfolio, bool displayValueFunds)
            : base(portfolio, displayValueFunds)
        {
            Header = "Security Investments";
            GenerateStatistics(displayValueFunds);
        }
    }
}
