using FinancialStructures.Database;
using GUISupport;

namespace FinanceViewModels.StatsViewModels
{
    public class TabViewModelBase : PropertyChangedBase
    {
        protected readonly Portfolio fPortfolio;

        private bool fDisplayValueFunds = true;
        public bool DisplayValueFunds
        {
            get { return fDisplayValueFunds; }
            set { fDisplayValueFunds = value;}
        }
        public string Header {get;set;}

        public virtual bool Closable { get { return true; } }

        public virtual void GenerateStatistics(bool displayValueFunds)
        {
        }
        public TabViewModelBase(Portfolio portfolio, bool displayValueFunds)
        {
            fPortfolio = portfolio;
            DisplayValueFunds = displayValueFunds;
        }
    }
}
