using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.FinanceStructures.Implementation.Asset;

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
        private readonly object AssetsLock = new object();
        private readonly object PensionsLock = new object();

        /// <summary>
        /// Flag to state when the user has altered values in the portfolio
        /// after the last save.
        /// </summary>
        [XmlIgnore]
        public bool IsAlteredSinceSave
        {
            get;
            private set;
        } = false;

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
        [XmlArrayItem(ElementName = "Security")]
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

        /// <summary>
        /// Backing for the BankAccounts.
        /// </summary>
        [XmlArray(ElementName = "BankAccounts")]
        [XmlArrayItem(ElementName = "CashAccount")]
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

        /// <summary>
        /// Backing for the currencies.
        /// </summary>
        [XmlArray(ElementName = "Currencies")]
        [XmlArrayItem(ElementName = "Currency")]
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
        [XmlArrayItem(ElementName = "Sector")]
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
        /// The list of assets in the portfolio.
        /// </summary>
        [XmlArray(ElementName = "Assets")]
        [XmlArrayItem(ElementName = "AmortisableAsset")]
        public List<AmortisableAsset> AssetsBackingList
        {
            get;
            set;
        } = new List<AmortisableAsset>();

        /// <inheritdoc/>
        [XmlIgnore]
        public IReadOnlyList<IAmortisableAsset> Assets
        {
            get
            {
                lock (AssetsLock)
                {
                    return AssetsBackingList.ToList();
                }
            }
        }

        /// <summary>
        /// A list storing the actual data for all Pensions
        /// </summary>
        [XmlArray(ElementName = "Pensions")]
        [XmlArrayItem(ElementName = "Pension")]
        public List<Security> PensionsBackingList
        {
            get;
            private set;
        } = new List<Security>();

        /// <inheritdoc />
        [XmlIgnore]
        public IReadOnlyList<ISecurity> Pensions
        {
            get
            {
                return PensionsBackingList.ToList();
            }
        }

        /// <summary>
        /// Default parameterless constructor.
        /// </summary>
        internal Portfolio()
        {
        }

        /// <inheritdoc/>
        public void SetFrom(Portfolio portfolio)
        {
            BaseCurrency = portfolio.BaseCurrency;
            FilePath = portfolio.FilePath;
            Funds = portfolio.Funds;
            BankAccounts = portfolio.BankAccounts;
            Currencies = portfolio.Currencies;
            BenchMarks = portfolio.BenchMarks;
            AssetsBackingList = portfolio.AssetsBackingList;
            PensionsBackingList = portfolio.PensionsBackingList;
            NotesInternal = portfolio.NotesInternal;
        }

        /// <inheritdoc />
        public void Clear()
        {
            SetFrom(new Portfolio());
            WireDataChangedEvents();
            OnPortfolioChanged(this, new PortfolioEventArgs(changedPortfolio: true));
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
                case Account.All:
                {
                    return FundsThreadSafe.Count + CurrenciesThreadSafe.Count + BankAccountsThreadSafe.Count + BenchMarksThreadSafe.Count;
                }
                case Account.Security:
                {
                    return FundsThreadSafe.Count;
                }
                case Account.Currency:
                {
                    return CurrenciesThreadSafe.Count;
                }
                case Account.BankAccount:
                {
                    return BankAccountsThreadSafe.Count;
                }
                case Account.Benchmark:
                {
                    return BenchMarksThreadSafe.Count;
                }
                case Account.Asset:
                {
                    return AssetsBackingList.Count;
                }
                case Account.Pension:
                {
                    return PensionsBackingList.Count;
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
                case Account.Asset:
                {
                    return AssetsBackingList.Where(fund => selector(fund)).Count();
                }
                case Account.Pension:
                {
                    return PensionsBackingList.Where(fund => selector(fund)).Count();
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

            foreach (AmortisableAsset asset in AssetsBackingList)
            {
                asset.DataEdit += OnPortfolioChanged;
                asset.SetupEventListening();
            }

            foreach (Security pension in PensionsBackingList)
            {
                pension.DataEdit += OnPortfolioChanged;
                pension.SetupEventListening();
            }
        }
    }
}
