﻿using FinancialStructures.Database;
using Common.UI.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels.Stats
{
    internal class TabViewModelBase : PropertyChangedBase
    {
        protected readonly IPortfolio fPortfolio;

        private bool fDisplayValueFunds = true;
        public bool DisplayValueFunds
        {
            get
            {
                return fDisplayValueFunds;
            }
            set
            {
                fDisplayValueFunds = value;
            }
        }

        public string Header
        {
            get; set;
        }

        public virtual bool Closable
        {
            get
            {
                return true;
            }
        }

        public virtual void GenerateStatistics(bool displayValueFunds)
        {
        }
        public TabViewModelBase(IPortfolio portfolio, bool displayValueFunds)
        {
            fPortfolio = portfolio;
            DisplayValueFunds = displayValueFunds;
        }
    }
}
