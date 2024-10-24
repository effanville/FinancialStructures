﻿using System;
using System.Collections.Generic;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database
{
    /// <summary>
    /// Interface for portfolio.
    /// </summary>
    public interface IPortfolio
    {
        /// <summary>
        /// The name of the portfolio.
        /// </summary>
        string Name
        {
            get;
            set;
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
        /// A collection of notes for the portfolio.
        /// </summary>
        IReadOnlyList<Note> Notes
        {
            get;
        }

        /// <summary>
        /// Add a note to the list of notes <see cref="Notes"/>.
        /// </summary>
        void AddNote(DateTime timeStamp, string note);

        /// <summary>
        /// Delete a note from the list of notes <see cref="Notes"/>.
        /// </summary>
        bool RemoveNote(Note note);

        /// <summary>
        /// Delete a note from the list of notes <see cref="Notes"/>
        /// at the index specified.
        /// </summary>
        void RemoveNote(int noteIndex);

        /// <summary>
        /// Securities stored in this database.
        /// This is a shallow copy of the actual list, accessed in a
        /// threadsafe manner.
        /// </summary>
        IReadOnlyList<ISecurity> Funds
        {
            get;
        }

        /// <summary>
        /// Bank accounts stored in this database.
        /// <para>
        /// This is a shallow copy of the actual list, accessed in a
        /// thread safe manner.
        /// </para>
        /// </summary>
        IReadOnlyList<IExchangableValueList> BankAccounts
        {
            get;
        }

        /// <summary>
        /// The currencies other objects are held in.
        /// </summary>
        IReadOnlyList<ICurrency> Currencies
        {
            get;
        }

        /// <summary>
        /// Sector benchmarks for comparison of held data.
        /// </summary>
        IReadOnlyList<IValueList> BenchMarks
        {
            get;
        }

        /// <summary>
        /// Extra assets that are held as part of this portfolio.
        /// </summary>
        IReadOnlyList<IAmortisableAsset> Assets
        {
            get;
        }

        /// <summary>
        /// Accounts that are held as pensions.
        /// </summary>
        IReadOnlyList<ISecurity> Pensions
        {
            get;
        }
        
        /// <summary>
        /// Outputs the account if it exists.
        /// </summary>
        /// <param name="accountType">The type of element to find.</param>
        /// <param name="name">The name of the element to find.</param>
        /// <param name="valueList">The account if it exists.</param>
        bool TryGetAccount(Account accountType, TwoName name, out IValueList valueList);

        /// <summary>
        /// Returns a copy of all accounts related to the account type.
        /// </summary>
        IReadOnlyList<IValueList> Accounts(Account account);

        /// <summary>
        /// Returns a copy of all accounts related to the total.
        /// </summary>
        IReadOnlyList<IValueList> Accounts(Totals totals, TwoName name = null);

        /// <summary>
        /// Number of type in the database.
        /// </summary>
        /// <param name="accountType">The type to search for.</param>
        /// <returns>The number of type in the database.</returns>
        int NumberOf(Account accountType);

        /// <summary>
        /// Number of type satisfying a certain condition in the database.
        /// </summary>
        /// <param name="account">The type to search for.</param>
        /// <param name="selector">A fucntion to select certain accounts.</param>
        /// <returns>The number of type in the database.</returns>
        int NumberOf(Account account, Func<IValueList, bool> selector);

        /// <summary>
        /// Removes unnecessary data from the database.
        /// </summary>
        void CleanData();

        /// <summary>
        /// Handler for the event that data stored in the portfolio has changed.
        /// </summary>
        event EventHandler<PortfolioEventArgs> PortfolioChanged;

        /// <summary>
        /// Raise event if something has changed.
        /// </summary>
        void OnPortfolioChanged(object obj, PortfolioEventArgs e);

        /// <summary>
        /// Edits the name of the data currently held.
        /// </summary>
        /// <param name="elementType">The type of data to edit.</param>
        /// <param name="oldName">The existing name of the data.</param>
        /// <param name="newName">The new name of the data.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure of editing.</returns>
        bool TryEditName(Account elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null);

        /// <summary>
        /// Adds data to the portfolio, unless data already exists.
        /// </summary>
        /// <param name="elementType">The type of data to add.</param>
        /// <param name="name">The name data to add.</param>
        /// <param name="reportLogger">Report callback action.</param>
        /// <returns>Success or failure of adding.</returns>
        bool TryAdd(Account elementType, NameData name, IReportLogger reportLogger = null);

        /// <summary>
        /// Removes the account from the database if it can.
        /// </summary>
        /// <param name="elementType">The type of account to remove.</param>
        /// <param name="name">The name of the account to remove.</param>
        /// <param name="reportLogger">(optional) A report callback.</param>
        /// <returns>Success or failure.</returns>
        bool TryRemove(Account elementType, TwoName name, IReportLogger reportLogger = null);

        /// <summary>
        /// Queries whether database contains item.
        /// </summary>
        /// <param name="elementType">The type of item to search for.</param>
        /// <param name="name">The names of the item to find.</param>
        /// <returns>Whether exists or not.</returns>
        bool Exists(Account elementType, TwoName name);

        /// <summary>
        /// Queries whether database contains item.
        /// </summary>
        /// <param name="elementType">The type of item to search for.</param>
        /// <param name="company">The company of the item to find.</param>
        /// <returns>Whether exists or not.</returns>
        bool CompanyExists(Account elementType, string company);
        
        /// <summary>
        /// Clears all data in the portfolio.
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns a list of all companes of the desired type in the database.
        /// </summary>
        /// <param name="account">Type of object to search for.</param>
        /// <returns>List of names of the desired type.</returns>
        IReadOnlyList<string> Companies(Account account);

        /// <summary>
        /// Returns a list of all sector names of the desired type in the database.
        /// </summary>
        /// <param name="account">Type of object to search for.</param>
        /// <returns>List of names of the desired type.</returns>
        IReadOnlyList<string> Sectors(Account account);

        /// <summary>
        /// Returns a list of all names of the desired type in the database.
        /// </summary>
        /// <param name="account">Type of object to search for.</param>
        /// <returns>List of names of the desired type.</returns>
        IReadOnlyList<string> Names(Account account);

        /// <summary>
        /// Returns a list of all namedata in the database.
        /// </summary>
        /// <param name="account">Type of object to search for.</param>
        /// <returns>List of names of the desired type.</returns>
        IReadOnlyList<NameData> NameDataForAccount(Account account);

        /// <summary>
        /// Queries for data for the security of name and company.
        /// </summary>
        IReadOnlyList<SecurityDayData> SecurityData(TwoName name, IReportLogger reportLogger = null);

        /// <summary>
        /// Returns the valuations of the account.
        /// </summary>
        IReadOnlyList<DailyValuation> NumberData(Account account, TwoName name, IReportLogger reportLogger = null);

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
        /// returns the currency associated to the account.
        /// </summary>
        ICurrency Currency(IValueList valueList);

        /// <summary>
        /// returns the currency associated to the name.
        /// </summary>
        ICurrency Currency(string currencyName);

        /// <summary>
        /// Adds values to the current portfolio from another specified portfolio.
        /// Matches on entity name only. If other 
        /// </summary>
        void ImportValuesFrom(IPortfolio other, IReportLogger reportLogger = null);
    }
}
