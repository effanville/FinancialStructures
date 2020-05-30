using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.FinanceStructures;
using FinancialStructures.PortfolioAPI;

namespace FinancialStructures.Database
{
    /// <summary>
    /// Data structure holding information about finances.
    /// </summary>
    public partial class Portfolio : IPortfolio
    {
        [XmlIgnoreAttribute]
        public bool IsAlteredSinceSave
        {
            get;
            private set;
        }

        public void Saving()
        {
            IsAlteredSinceSave = false;
        }

        private string fDatabaseFilePath;

        /// <summary>
        /// Set the path where the database will be stored.
        /// </summary>
        /// <param name="path"></param>
        public void SetFilePath(string path)
        {
            fDatabaseFilePath = path;
        }

        /// <summary>
        /// Access of the databse path.
        /// </summary>
        public string FilePath
        {
            get
            {
                return fDatabaseFilePath;
            }
        }

        /// <summary>
        /// The file extension of the path.
        /// </summary>
        public string Extension
        {
            get
            {
                return string.IsNullOrEmpty(fDatabaseFilePath) ? string.Empty : Path.GetExtension(fDatabaseFilePath);
            }
        }

        /// <summary>
        /// The directory where the database is stored.
        /// </summary>
        public string Directory
        {
            get
            {
                return string.IsNullOrEmpty(fDatabaseFilePath) ? string.Empty : Path.GetDirectoryName(fDatabaseFilePath);
            }
        }

        /// <summary>
        /// The non-extension part of the filename, considered to be the databse name.
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(fDatabaseFilePath);
            }
        }

        /// <summary>
        /// The default currency for the portfolio.
        /// </summary>
        public string BaseCurrency
        {
            get;
            set;
        }

        /// <summary>
        /// Securities stored in this database.
        /// </summary>
        public List<Security> Funds
        {
            get;
            private set;
        } = new List<Security>();

        /// <summary>
        /// Bank accounts stored in this database.
        /// </summary>
        public List<CashAccount> BankAccounts
        {
            get;
            private set;
        } = new List<CashAccount>();

        /// <summary>
        /// The currencies other objects are held in.
        /// </summary>
        public List<Currency> Currencies
        {
            get;
            private set;
        } = new List<Currency>();

        /// <summary>
        /// Sector benchmarks for comparison of held data.
        /// </summary>
        public List<Sector> BenchMarks
        {
            get;
            set;
        } = new List<Sector>();

        /// <summary>
        /// Default parameterless constructor.
        /// </summary>
        public Portfolio()
        {
        }

        /// <summary>
        /// Copies references of other portfolio to this portfolio.
        /// </summary>
        public void CopyData(IPortfolio portfolio)
        {
            BaseCurrency = portfolio.BaseCurrency;
            Funds = portfolio.Funds;
            BankAccounts = portfolio.BankAccounts;
            Currencies = portfolio.Currencies;
            BenchMarks = portfolio.BenchMarks;
        }

        /// <summary>
        /// Sets the benchmark parts of this portfolio.
        /// </summary>
        /// <param name="sectors"></param>
        public void SetBenchMarks(List<Sector> sectors)
        {
            BenchMarks.Clear();
            BenchMarks.AddRange(sectors);
        }

        /// <summary>
        /// Event to be raised when elements are changed.
        /// </summary>
        public event EventHandler PortfolioChanged;

        /// <summary>
        /// handle the events raised in the above.
        /// </summary>
        public void OnPortfolioChanged(object obj, EventArgs e)
        {
            IsAlteredSinceSave = true;
            EventHandler handler = PortfolioChanged;
            if (handler != null)
            {
                handler?.Invoke(obj, e);
            }
        }

        /// <summary>
        /// Number of type in the database.
        /// </summary>
        /// <param name="elementType">The type to search for.</param>
        /// <returns>The number of type in the database.</returns>
        public int NumberOf(AccountType elementType)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    return Funds.Count;
                }
                case (AccountType.Currency):
                {
                    return Currencies.Count;
                }
                case (AccountType.BankAccount):
                {
                    return BankAccounts.Count;
                }
                case (AccountType.Sector):
                {
                    break;
                }
                default:
                    break;
            }

            return 0;
        }

        /// <inheritdoc/>
        public void WireDataChangedEvents()
        {
            foreach (Security security in Funds)
            {
                security.DataEdit += OnPortfolioChanged;
                security.SetupEventListening();
            }

            foreach (CashAccount bankAccount in BankAccounts)
            {
                bankAccount.DataEdit += OnPortfolioChanged;
                bankAccount.SetupEventListening();
            }

            foreach (Sector sector in BenchMarks)
            {
                sector.DataEdit += OnPortfolioChanged;
                sector.SetupEventListening();
            }

            foreach (Currency currency in Currencies)
            {
                currency.DataEdit += OnPortfolioChanged;
                currency.SetupEventListening();
            }
        }
    }
}
