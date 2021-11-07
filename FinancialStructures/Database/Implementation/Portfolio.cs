using System;
using System.Collections.Generic;
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
        private readonly object FundsLock = new object();
        private readonly object BankAccountsLock = new object();
        private readonly object CurrenciesLock = new object();
        private readonly object BenchmarksLock = new object();

        /// <summary>
        /// Flag to state when the user has altered values in the portfolio
        /// after the last save.
        /// </summary>
        [XmlIgnore]
        public bool IsAlteredSinceSave
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        [XmlElement(ElementName = "FilePath")]
        public string FilePath
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [XmlElement(ElementName = "BaseCurrency")]
        public string BaseCurrency
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [XmlArray(ElementName = "Funds")]
        public List<Security> Funds
        {
            get;
            private set;
        } = new List<Security>();


        /// <inheritdoc/>
        [XmlIgnore]
        public IReadOnlyList<ISecurity> FundsThreadSafe
        {
            get
            {
                lock (FundsLock)
                {
                    return Funds.ToList();
                }
            }
        }

        /// <inheritdoc/>
        [XmlArray(ElementName = "BankAccounts")]
        public List<CashAccount> BankAccounts
        {
            get;
            private set;
        } = new List<CashAccount>();

        /// <inheritdoc/>
        [XmlIgnore]
        public IReadOnlyList<IExchangableValueList> BankAccountsThreadSafe
        {
            get
            {
                lock (BankAccountsLock)
                {
                    return BankAccounts.ToList();
                }
            }
        }

        /// <inheritdoc/>
        [XmlArray(ElementName = "Currencies")]
        public List<Currency> Currencies
        {
            get;
            private set;
        } = new List<Currency>();

        /// <inheritdoc/>
        [XmlIgnore]
        public IReadOnlyList<ICurrency> CurrenciesThreadSafe
        {
            get
            {
                lock (CurrenciesLock)
                {
                    return Currencies.ToList();
                }
            }
        }

        /// <inheritdoc/>
        [XmlArray(ElementName = "BenchMarks")]
        public List<Sector> BenchMarks
        {
            get;
            set;
        } = new List<Sector>();

        /// <inheritdoc/>
        [XmlIgnore]
        public IReadOnlyList<IValueList> BenchMarksThreadSafe
        {
            get
            {
                lock (BenchmarksLock)
                {
                    return BenchMarks.ToList();
                }
            }
        }

        /// <summary>
        /// Default parameterless constructor.
        /// </summary>
        internal Portfolio()
        {
        }

        /// <inheritdoc/>
        public void CopyData(Portfolio portfolio)
        {
            BaseCurrency = portfolio.BaseCurrency;
            FilePath = portfolio.FilePath;
            Funds = portfolio.Funds;
            BankAccounts = portfolio.BankAccounts;
            Currencies = portfolio.Currencies;
            BenchMarks = portfolio.BenchMarks;
            NotesInternal = portfolio.NotesInternal;
        }

        /// <summary>
        /// For legacy loading this is required to set the benchmarks.
        /// </summary>
        public void SetBenchMarks(List<Sector> sectors)
        {
            lock (BenchmarksLock)
            {
                BenchMarks.Clear();
                BenchMarks.AddRange(sectors);
            }
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

            if (obj is bool _)
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
        public int NumberOf(Account elementType)
        {
            switch (elementType)
            {
                case (Account.All):
                {
                    return FundsThreadSafe.Count + CurrenciesThreadSafe.Count + BankAccountsThreadSafe.Count + BenchMarksThreadSafe.Count;
                }
                case (Account.Security):
                {
                    return FundsThreadSafe.Count;
                }
                case (Account.Currency):
                {
                    return CurrenciesThreadSafe.Count;
                }
                case (Account.BankAccount):
                {
                    return BankAccountsThreadSafe.Count;
                }
                case (Account.Benchmark):
                {
                    return BenchMarksThreadSafe.Count;
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
                    return FundsThreadSafe.Where(fund => selector(fund)).Count();
                }
                case Account.BankAccount:
                {
                    return BankAccountsThreadSafe.Where(fund => selector(fund)).Count();
                }
                case Account.Benchmark:
                {
                    return BenchMarksThreadSafe.Where(fund => selector(fund)).Count();
                }
                case Account.Currency:
                {
                    return CurrenciesThreadSafe.Where(fund => selector(fund)).Count();
                }
                default:
                    return 0;
            }
        }

        /// <inheritdoc/>
        public void CleanData()
        {
            foreach (ISecurity security in FundsThreadSafe)
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
