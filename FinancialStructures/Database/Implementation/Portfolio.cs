using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;

namespace FinancialStructures.Database.Implementation
{
    /// <summary>
    /// Data structure holding information about finances.
    /// </summary>
    public partial class Portfolio : IPortfolio
    {
        private string fDatabaseFilePath;

        /// <summary>
        /// Flag to state when the user has altered values in the portfolio
        /// after the last save.
        /// </summary>
        [XmlIgnoreAttribute]
        public bool IsAlteredSinceSave
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public string FilePath
        {
            get
            {
                return fDatabaseFilePath;
            }
        }

        /// <inheritdoc/>
        public string Extension
        {
            get
            {
                return string.IsNullOrEmpty(fDatabaseFilePath) ? string.Empty : Path.GetExtension(fDatabaseFilePath);
            }
        }

        /// <inheritdoc/>
        public string Directory
        {
            get
            {
                return string.IsNullOrEmpty(fDatabaseFilePath) ? string.Empty : Path.GetDirectoryName(fDatabaseFilePath);
            }
        }

        /// <inheritdoc/>
        public string DatabaseName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(fDatabaseFilePath);
            }
        }

        /// <inheritdoc/>
        public string BaseCurrency
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public List<Security> Funds
        {
            get;
            private set;
        } = new List<Security>();

        /// <inheritdoc/>
        public List<CashAccount> BankAccounts
        {
            get;
            private set;
        } = new List<CashAccount>();

        /// <inheritdoc/>
        public List<Currency> Currencies
        {
            get;
            private set;
        } = new List<Currency>();

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void CopyData(IPortfolio portfolio)
        {
            BaseCurrency = portfolio.BaseCurrency;
            fDatabaseFilePath = portfolio.FilePath;
            Funds = portfolio.Funds;
            BankAccounts = portfolio.BankAccounts;
            Currencies = portfolio.Currencies;
            BenchMarks = portfolio.BenchMarks;
        }

        /// <inheritdoc/>
        public void SetBenchMarks(List<Sector> sectors)
        {
            BenchMarks.Clear();
            BenchMarks.AddRange(sectors);
        }

        /// <summary>
        /// Event to be raised when elements are changed.
        /// </summary>
        public event EventHandler<PortfolioEventArgs> PortfolioChanged;

        /// <summary>
        /// handle the events raised in the above.
        /// </summary>
        public void OnPortfolioChanged(object obj, PortfolioEventArgs e)
        {
            IsAlteredSinceSave = true;
            EventHandler<PortfolioEventArgs> handler = PortfolioChanged;
            if (handler != null)
            {
                handler?.Invoke(obj, e);
            }

            if (obj is bool noChange)
            {
                IsAlteredSinceSave = false;
            }
        }

        /// <inheritdoc/>
        public void Saving()
        {
            IsAlteredSinceSave = false;
        }

        /// <inheritdoc/>
        public void SetFilePath(string path)
        {
            if (fDatabaseFilePath != path)
            {
                fDatabaseFilePath = path;
                OnPortfolioChanged(fDatabaseFilePath, new PortfolioEventArgs());
            }
        }

        /// <inheritdoc/>
        public int NumberOf(Account elementType)
        {
            switch (elementType)
            {
                case (Account.Security):
                {
                    return Funds.Count;
                }
                case (Account.Currency):
                {
                    return Currencies.Count;
                }
                case (Account.BankAccount):
                {
                    return BankAccounts.Count;
                }
                case (Account.Benchmark):
                {
                    break;
                }
                default:
                    break;
            }

            return 0;
        }

        /// <inheritdoc/>
        public int NumberOf(Account account, Func<IValueList, bool> selector)
        {
            switch (account)
            {
                case Account.Security:
                {
                    return Funds.Where(fund => selector(fund)).Count();
                }
                case Account.BankAccount:
                {
                    return BankAccounts.Where(fund => selector(fund)).Count();
                }
                case Account.Benchmark:
                {
                    return BenchMarks.Where(fund => selector(fund)).Count();
                }
                case Account.Currency:
                {
                    return Currencies.Where(fund => selector(fund)).Count();
                }
                default:
                    return 0;
            }
        }

        /// <inheritdoc/>
        public void CleanData()
        {
            foreach (var security in Funds)
            {
                security.CleanData();
            }
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
