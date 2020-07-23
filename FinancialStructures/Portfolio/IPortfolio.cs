using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using StructureCommon.Reporting;

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
        /// Set the path where the database will be stored.
        /// </summary>
        void SetFilePath(string path);

        /// <summary>
        /// Edits the name of the data currently held.
        /// </summary>
        /// <param name="elementType">The type of data to edit.</param>
        /// <param name="oldName">The existing name of the data.</param>
        /// <param name="newName">The new name of the data.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure of editing.</returns>
        bool TryEditName(AccountType elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null);

        /// <summary>
        /// Adds data to the portfolio, unless data already exists.
        /// </summary>
        /// <param name="elementType">The type of data to add.</param>
        /// <param name="name">The name data to add.</param>
        /// <param name="reportLogger">Report callback action.</param>
        /// <returns>Success or failure of adding.</returns>
        bool TryAdd(AccountType elementType, NameData name, IReportLogger reportLogger = null);

        /// <summary>
        /// Removes the account from the database if it can.
        /// </summary>
        /// <param name="elementType">The type of account to remove.</param>
        /// <param name="name">The name of the account to remove.</param>
        /// <param name="reportLogger">(optional) A report callback.</param>
        /// <returns>Success or failure.</returns>
        bool TryRemove(AccountType elementType, TwoName name, IReportLogger reportLogger = null);

        /// <summary>
        /// Queries whether database contains item.
        /// </summary>
        /// <param name="elementType">The type of item to search for.</param>
        /// <param name="name">The names of the item to find.</param>
        /// <returns>Whether exists or not.</returns>
        bool Exists(AccountType elementType, TwoName name);

        /// <summary>
        /// Queries whether database contains item.
        /// </summary>
        /// <param name="elementType">The type of item to search for.</param>
        /// <param name="company">The company of the item to find.</param>
        /// <returns>Whether exists or not.</returns>
        bool CompanyExists(AccountType elementType, string company);

        /// <summary>
        /// Load database from xml file.
        /// </summary>
        /// <param name="filePath">The path to load from.</param>
        /// <param name="reportLogger">Callback to report information.</param>
        void LoadPortfolio(string filePath, IReportLogger reportLogger = null);

        /// <summary>
        /// Save database to xml file.
        /// </summary>
        /// <param name="filePath">The path to save to.</param>
        /// <param name="reportLogger">Callback to report information.</param>
        void SavePortfolio(string filePath, IReportLogger reportLogger = null);

        /// <summary>
        /// Adds the desired data to the security if it can.
        /// </summary>
        bool TryAddOrEditDataToSecurity(TwoName names, DateTime oldDate, DateTime date, double shares, double unitPrice, double Investment, IReportLogger reportLogger = null);

        /// <summary>
        /// Attempts to add data to the account.
        /// </summary>
        /// <param name="elementType">The type of data to add to.</param>
        /// <param name="name">The name to add to.</param>
        /// <param name="data">The data to add.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        /// <remarks> This cannot currently be used to add to securities due to different type of data.</remarks>
        bool TryAddOrEditData(AccountType elementType, TwoName name, DailyValuation oldData, DailyValuation data, IReportLogger reportLogger = null);

        /// <summary>
        /// Attempts to remove data from the account.
        /// </summary>
        /// <param name="elementType">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="data">The data to remove.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        bool TryDeleteData(AccountType elementType, TwoName name, DateTime date, IReportLogger reportLogger = null);

        /// <summary>
        /// Returns a list of all companes of the desired type in the databse.
        /// </summary>
        /// <param name="elementType">Type of object to search for.</param>
        /// <returns>List of names of the desired type.</returns>
        List<string> Companies(AccountType elementType);


        /// <summary>
        /// Returns a list of all names of the desired type in the databse.
        /// </summary>
        /// <param name="elementType">Type of object to search for.</param>
        /// <returns>List of names of the desired type.</returns>
        List<string> Names(AccountType elementType);

        /// <summary>
        /// Returns a list of all namedata in the databse.
        /// </summary>
        /// <param name="elementType">Type of object to search for.</param>
        /// <returns>List of names of the desired type.</returns>
        List<NameCompDate> NameData(AccountType elementType);

        /// <summary>
        /// Queries for data for the security of name and company. 
        /// </summary>
        List<SecurityDayData> SecurityData(TwoName name, IReportLogger reportLogger = null);

        /// <summary>
        /// Returns the 
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="name"></param>
        /// <param name="reportLogger"></param>
        /// <returns></returns>
        List<DailyValuation> NumberData(AccountType elementType, TwoName name, IReportLogger reportLogger = null);

        /// <summary>
        /// Outputs a copy of the security if it exists.
        /// </summary>
        bool TryGetSecurity(TwoName names, out ISecurity desired);

        /// <summary>
        /// Outputs the account if it exists.
        /// </summary>
        /// <param name="elementType">The type of element to find.</param>
        /// <param name="name">The name of the element to find.</param>
        bool TryGetAccount(AccountType accountType, TwoName name, out ISingleValueDataList desired);

        /// <summary>
        /// Returns a copy of all securities with the company as specified.
        /// </summary>
        List<ISecurity> CompanySecurities(string company);

        List<ICashAccount> CompanyBankAccounts(string company);

        List<ISecurity> SectorSecurities(string sectorName);

        /// <summary>
        /// Returns a copy of the currently held portfolio. 
        /// Note one cannot use this portfolio to edit as it makes a copy.
        /// </summary>
        /// <remarks> 
        /// This is in theory dangerous. I know thought that a security copied 
        /// returns a genuine security, so I can case without trouble.
        /// </remarks>
        IPortfolio Copy();

        /// <summary>
        /// Get the latest value of the selected element.
        /// </summary>
        /// <param name="elementType">The type of element to find.</param>
        /// <param name="name">The name of the element to find.</param>
        /// <returns>The latest value if it exists.</returns>
        double LatestValue(AccountType elementType, TwoName name);

        /// <summary>
        /// Get the value of the selected element on the date provided. For a sector the name is only the surname
        /// </summary>
        /// <param name="elementType">The type of element to find.</param>
        /// <param name="name">The name of the element to find.</param>
        /// <param name="date">The date on which to find the value.</param>
        /// <returns>The  value if it exists.</returns>
        double Value(AccountType elementType, TwoName name, DateTime date);

        /// <summary>
        /// Total value of all accounts of type specified today.
        /// </summary>
        /// <param name="elementType">The type to find the total of.</param>
        /// <returns>The total value held on today.</returns>
        double TotalValue(AccountType elementType);

        /// <summary>
        /// Total value of all accounts of type specified on date given.
        /// </summary>
        /// <param name="elementType">The type to find the total of.</param>
        /// <param name="date">The date to find the total on.</param>
        /// <returns>The total value held.</returns>
        double TotalValue(AccountType elementType, DateTime date);

        /// <summary>
        /// Total value of all accounts of all types on date given.
        /// </summary>
        /// <param name="elementType">The type to find the total of.</param>
        /// <param name="date">The date to find the total on.</param>
        /// <returns>The total value held.</returns>
        double TotalValue(DateTime date);

        /// <summary>
        /// Calculates the value held in the company.
        /// </summary>
        /// <param name="elementType">The type of account to find.</param>
        /// <param name="company">The company name to search for.</param>
        /// <param name="date">The date to calculate value on.</param>
        /// <returns>The value held in the company.</returns>
        double CompanyValue(AccountType elementType, string company, DateTime date);

        /// <summary>
        /// returns the currency associated to the account.
        /// </summary>
        ICurrency Currency(AccountType elementType, object account);
    }
}
