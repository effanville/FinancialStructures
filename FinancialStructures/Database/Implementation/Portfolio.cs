using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        private readonly object _fundsLock = new object();
        private readonly object _bankAccountsLock = new object();
        private readonly object _currenciesLock = new object();
        private readonly object _benchmarksLock = new object();
        private readonly object _assetsLock = new object();
        private readonly object _pensionsLock = new object();

        /// <summary>
        /// Flag to state when the user has altered values in the portfolio
        /// after the last save.
        /// </summary>
        public bool IsAlteredSinceSave
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public string Name
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public string BaseCurrency
        {
            get;
            set;
        }

        private List<Security> _fundsBackingList = new List<Security>();

        /// <inheritdoc/>
        public IReadOnlyList<ISecurity> Funds
        {
            get
            {
                lock(_fundsLock)
                {
                    return _fundsBackingList.ToList();
                }
            }
        }

        internal void AddFund(Security security)
        {
            lock(_fundsLock)
            {
                _fundsBackingList.Add(security);
            }
        }

        /// <summary>
        /// Backing for the BankAccounts.
        /// </summary>
        private List<CashAccount> _bankAccountBackingList = new List<CashAccount>();

        /// <inheritdoc/>
        public IReadOnlyList<IExchangableValueList> BankAccounts
        {
            get
            {
                lock (_bankAccountsLock)
                {
                    return _bankAccountBackingList.ToList();
                }
            }
        }

        internal void AddBankAccount(CashAccount cashAccount)
        {
            lock (_bankAccountsLock)
            {_bankAccountBackingList.Add(cashAccount);
            }
        }

        /// <summary>
        /// Backing for the currencies.
        /// </summary>
        private List<Currency> _currenciesBackingList = new List<Currency>();

        /// <inheritdoc/>
        public IReadOnlyList<ICurrency> Currencies
        {
            get
            {
                lock (_currenciesLock)
                {
                    return _currenciesBackingList.ToList();
                }
            }
        }

        internal void AddCurrency(Currency currency)
        {
            lock (_currenciesLock)
            {
                _currenciesBackingList.Add(currency);
            }
        }

        private List<Sector> _benchMarksBackingList = new List<Sector>();

        /// <inheritdoc/>
        public IReadOnlyList<IValueList> BenchMarks
        {
            get
            {
                lock (_benchmarksLock)
                {
                    return _benchMarksBackingList.ToList();
                }
            }
        }

        internal void AddBenchMark(Sector sector)
        {                
            lock (_benchmarksLock)
            {
                _benchMarksBackingList.Add(sector);
            }
        }

        /// <summary>
        /// The list of assets in the portfolio.
        /// </summary>
        private List<AmortisableAsset> _assetsBackingList = new List<AmortisableAsset>();

        /// <inheritdoc/>
        public IReadOnlyList<IAmortisableAsset> Assets
        {
            get
            {
                lock (_assetsLock)
                {
                    return _assetsBackingList.ToList();
                }
            }
        }

        internal void AddAsset(AmortisableAsset asset)
        {
            lock (_assetsLock)
            {
                _assetsBackingList.Add(asset);
            }
        }

        /// <summary>
        /// A list storing the actual data for all Pensions
        /// </summary>
        private List<Security> _pensionsBackingList = new List<Security>();

        /// <inheritdoc />
        public IReadOnlyList<ISecurity> Pensions
        {
            get
            {
                lock (_pensionsLock)
                {
                    return _pensionsBackingList.ToList();
                }
            }
        }

        internal void AddPension(Security security)
        {
            lock (_pensionsLock)
            {
                _pensionsBackingList.Add(security);
            }
        }

        /// <summary>
        /// Default parameterless constructor.
        /// </summary>
        internal Portfolio()
        {
        }

        private void SetFrom(Portfolio portfolio)
        {
            BaseCurrency = portfolio.BaseCurrency;
            Name = portfolio.Name;
            _fundsBackingList = portfolio._fundsBackingList;
            _bankAccountBackingList = portfolio._bankAccountBackingList;
            _currenciesBackingList = portfolio._currenciesBackingList;
            _benchMarksBackingList = portfolio._benchMarksBackingList;
            _assetsBackingList = portfolio._assetsBackingList;
            _pensionsBackingList = portfolio._pensionsBackingList;
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
            lock (_benchmarksLock)
            {
                _benchMarksBackingList.Clear();
                _benchMarksBackingList.AddRange(sectors);
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
            handler?.Invoke(obj, e);

            if (obj is bool _)
            {
                IsAlteredSinceSave = false;
            }
        }

        public void Saving() => IsAlteredSinceSave = false;

        /// <inheritdoc/>
        public int NumberOf(Account elementType)
            => elementType switch
            {
                Account.All => Funds.Count + Currencies.Count + BankAccounts.Count +
                               BenchMarks.Count,
                Account.Security => Funds.Count,
                Account.Currency => Currencies.Count,
                Account.BankAccount => BankAccounts.Count,
                Account.Benchmark => BenchMarks.Count,
                Account.Asset => Assets.Count,
                Account.Pension => Pensions.Count,
                Account.Unknown => 0,
                _ => 0
            };

        /// <inheritdoc/>
        public int NumberOf(Account account, Func<IValueList, bool> selector) 
            => account switch
            {
                Account.Security => Funds.Count(fund => selector(fund)),
                Account.BankAccount => BankAccounts.Count(fund => selector(fund)),
                Account.Benchmark => BenchMarks.Count(selector),
                Account.Currency => Currencies.Count(fund => selector(fund)),
                Account.Asset => Assets.Count(fund => selector(fund)),
                Account.Pension => Pensions.Count(fund => selector(fund)),
                _ => 0
            };

        /// <inheritdoc/>
        public void CleanData()
        {
            foreach (ISecurity security in Funds)
            {
                security.CleanData();
            }
        }

        public void WireDataChangedEvents()
        {
            foreach (Security security in _fundsBackingList)
            {
                security.DataEdit += OnPortfolioChanged;
                security.SetupEventListening();
            }

            foreach (CashAccount bankAccount in _bankAccountBackingList)
            {
                bankAccount.DataEdit += OnPortfolioChanged;
                bankAccount.SetupEventListening();
            }

            foreach (Sector sector in _benchMarksBackingList)
            {
                sector.DataEdit += OnPortfolioChanged;
                sector.SetupEventListening();
            }

            foreach (Currency currency in _currenciesBackingList)
            {
                currency.DataEdit += OnPortfolioChanged;
                currency.SetupEventListening();
            }

            foreach (AmortisableAsset asset in _assetsBackingList)
            {
                asset.DataEdit += OnPortfolioChanged;
                asset.SetupEventListening();
            }

            foreach (Security pension in _pensionsBackingList)
            {
                pension.DataEdit += OnPortfolioChanged;
                pension.SetupEventListening();
            }
        }
    }
}
