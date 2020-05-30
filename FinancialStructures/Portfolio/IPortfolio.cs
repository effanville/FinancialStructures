using System;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.PortfolioAPI;

namespace FinancialStructures.FinanceInterfaces
{
    /// <summary>
    /// Interface for portfolio.
    /// </summary>
    public interface IPortfolio
    {
        void SetFilePath(string path);
        string FilePath
        {
            get;
        }
        string Extension
        {
            get;
        }

        string Directory
        {
            get;
        }
        string DatabaseName
        {
            get;
        }
        string BaseCurrency
        {
            get; set;
        }

        List<Security> Funds
        {
            get;
        }
        List<CashAccount> BankAccounts
        {
            get;
        }
        List<Currency> Currencies
        {
            get;
        }
        List<Sector> BenchMarks
        {
            get;
        }

        void CopyData(IPortfolio portfolio);

        void SetBenchMarks(List<Sector> sectors);

        int NumberOf(AccountType accountType);

        /// <summary>
        /// Handler for the event that data stored in the portfolio has changed.
        /// </summary>
        event EventHandler PortfolioChanged;

        /// <summary>
        /// Raise event if something has changed.
        /// </summary>
        void OnPortfolioChanged(object obj, EventArgs e);

        /// <summary>
        /// Enacts subscriptions of data changed events when creating portfolio.
        /// </summary>
        void WireDataChangedEvents();

        /// <summary>
        /// Whether the user has changed the database since last save.
        /// </summary>
        bool IsAlteredSinceSave
        {
            get;
        }

        /// <summary>
        /// Enacts internal things in the portfolio when it is being saved.
        /// </summary>
        void Saving();
    }
}
