using System.Collections.Generic;
using Common.Structure.DataStructures;
using Common.Structure.NamingStructures;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    internal class SecurityInvestmentViewModel : DataDisplayViewModelBase
    {
        public override bool Closable => true;
        private List<Labelled<TwoName, DailyValuation>> fSecuritiesInvestments;

        public List<Labelled<TwoName, DailyValuation>> SecuritiesInvestments
        {
            get => fSecuritiesInvestments;
            set => SetAndNotify(ref fSecuritiesInvestments, value, nameof(SecuritiesInvestments));
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            base.UpdateData(portfolio);
            SecuritiesInvestments = DataStore.TotalInvestments(Totals.Security);
        }

        public SecurityInvestmentViewModel(IPortfolio portfolio)
            : base("Investments", portfolio)
        {
            UpdateData(portfolio);
        }
    }
}
