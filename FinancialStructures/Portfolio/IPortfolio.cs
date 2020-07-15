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
        /// <summary>
        /// Access of the databse path.
        /// </summary>
        string FilePath
        {
            get;
        }

        /// <summary>
        /// The file extension of the path.
        /// </summary>
        string Extension
        {
            get;
        }

        /// <summary>
        /// The directory where the database is stored.
        /// </summary>
        string Directory
        {
            get;
        }

        /// <summary>
        /// The non-extension part of the filename, considered to be the databse name.
        /// </summary>
        string DatabaseName
        {
            get;
        }

        /// <summary>
        /// Whether the user has changed the database since last save.
        /// </summary>
        bool IsAlteredSinceSave
        {
            get;
        }

        /// <summary>
        /// The default currency for the portfolio.
        /// </summary>
        string BaseCurrency
        {
            get;
            set;
        }

        /// <summary>
        /// Securities stored in this database.
        /// </summary>
        List<Security> Funds
        {
            get;
        }

        /// <summary>
        /// Bank accounts stored in this database.
        /// </summary>
        List<CashAccount> BankAccounts
        {
            get;
        }

        /// <summary>
        /// The currencies other objects are held in.
        /// </summary>
        List<Currency> Currencies
        {
            get;
        }

        /// <summary>
        /// Sector benchmarks for comparison of held data.
        /// </summary>
        List<Sector> BenchMarks
        {
            get;
        }

        /// <summary>
        /// Copies references of other portfolio to this portfolio.
        /// </summary>
        void CopyData(IPortfolio portfolio);

        /// <summary>
        /// Sets the benchmark parts of this portfolio.
        /// </summary>
        void SetBenchMarks(List<Sector> sectors);

        /// <summary>
        /// Number of type in the database.
        /// </summary>
        /// <param name="elementType">The type to search for.</param>
        /// <returns>The number of type in the database.</returns>
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
        /// Set the path where the database will be stored.
        /// </summary>
        void SetFilePath(string path);

        /// <summary>
        /// Enacts internal things in the portfolio when it is being saved.
        /// </summary>
        void Saving();
    }
}
