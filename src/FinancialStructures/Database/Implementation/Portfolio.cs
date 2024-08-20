using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.FinanceStructures.Implementation.Asset;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation
{
    /// <summary>
    /// Data structure holding information about finances.
    /// </summary>
    public partial class Portfolio : IPortfolio
    {
        private readonly ReaderWriterLockSlim _fundsLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim _bankAccountsLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim _currenciesLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim _benchmarksLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim _assetsLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim _pensionsLock = new ReaderWriterLockSlim();

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

        private Dictionary<TwoName, Security> _fundsDictionary = new Dictionary<TwoName, Security>();
        
        /// <inheritdoc/>
        public IReadOnlyList<ISecurity> Funds
        {
            get
            {
                _fundsLock.EnterReadLock();
                try
                {
                    return _fundsDictionary.Values.ToList();
                }
                finally
                {
                    _fundsLock.ExitReadLock();
                }
            }
        }

        internal void AddFund(Security security)
        {
            _fundsLock.EnterWriteLock();
            try
            {
                _fundsDictionary.Add(security.Names.ToTwoName(), security);
            }finally
            {
                _fundsLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Backing for the BankAccounts.
        /// </summary>
        private Dictionary<TwoName, CashAccount> _bankAccountsDictionary = new Dictionary<TwoName, CashAccount>();

        /// <inheritdoc/>
        public IReadOnlyList<IExchangeableValueList> BankAccounts
        {
            get
            {
                _bankAccountsLock.EnterReadLock();
                try
                {
                    return _bankAccountsDictionary.Values.ToList();
                }
                finally
                {
                    _bankAccountsLock.ExitReadLock();
                }
            }
        }

        internal void AddBankAccount(CashAccount cashAccount)
        {
            _bankAccountsLock.EnterWriteLock();
            try
            {
                _bankAccountsDictionary.TryAdd(cashAccount.Names.ToTwoName(), cashAccount);
            }
            finally
            {
                _bankAccountsLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Backing for the currencies.
        /// </summary>
        private Dictionary<TwoName, Currency> _currenciesDictionary = new Dictionary<TwoName, Currency>();

        /// <inheritdoc/>
        public IReadOnlyList<ICurrency> Currencies
        {
            get
            {
                _currenciesLock.EnterReadLock();
                try
                {
                    return _currenciesDictionary.Values.ToList();
                }
                finally
                {
                    _currenciesLock.ExitReadLock();
                }
            }
        }

        internal void AddCurrency(Currency currency)
        {
            _currenciesLock.EnterWriteLock();
            try
            {
                _currenciesDictionary.Add(currency.Names.ToTwoName(), currency);
            }
            finally
            {
                _currenciesLock.ExitWriteLock();
            }
        }

        private Dictionary<TwoName,Sector> _benchMarksDictionary = new Dictionary<TwoName,Sector>();

        /// <inheritdoc/>
        public IReadOnlyList<IValueList> BenchMarks
        {
            get
            {
                _benchmarksLock.EnterReadLock();
                try
                {
                    return _benchMarksDictionary.Values.ToList();
                }
                finally
                {
                    _benchmarksLock.ExitReadLock();
                }
            }
        }

        internal void AddBenchMark(Sector sector)
        {                
            _benchmarksLock.EnterWriteLock();
            try
            {
                _benchMarksDictionary.Add(sector.Names.ToTwoName(), sector);
            }
            finally
            {
                _benchmarksLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// The list of assets in the portfolio.
        /// </summary>
        private Dictionary<TwoName, AmortisableAsset> _assetsDictionary = new Dictionary<TwoName, AmortisableAsset>();

        /// <inheritdoc/>
        public IReadOnlyList<IAmortisableAsset> Assets
        {
            get
            {
                _assetsLock.EnterReadLock();
                try
                {
                    return _assetsDictionary.Values.ToList();
                }
                finally
                {
                    _assetsLock.ExitReadLock();
                }
            }
        }

        internal void AddAsset(AmortisableAsset asset)
        {
            _assetsLock.EnterWriteLock();
            try
            {
                _assetsDictionary.Add(asset.Names.ToTwoName(), asset);
            }
            finally
            {
                _assetsLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// A list storing the actual data for all Pensions
        /// </summary>
        private Dictionary<TwoName,Security> _pensionsDictionary = new Dictionary<TwoName, Security>();

        /// <inheritdoc />
        public IReadOnlyList<ISecurity> Pensions
        {
            get
            {
                _pensionsLock.EnterReadLock();
                try
                {
                    return _pensionsDictionary.Values.ToList();
                }
                finally
                {
                    _pensionsLock.ExitReadLock();
                }
            }
        }

        internal void AddPension(Security pension)
        {
            _pensionsLock.EnterWriteLock();
            try
            {
                _pensionsDictionary.Add(pension.Names.ToTwoName(), pension);
            }
            finally
            {
                _pensionsLock.ExitWriteLock();
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
            _fundsLock.EnterWriteLock();
            try
            {
                _fundsDictionary = portfolio._fundsDictionary;
            }
            finally
            {
                _fundsLock.ExitWriteLock();
            }
            _bankAccountsLock.EnterWriteLock();
            try
            {
                _bankAccountsDictionary = portfolio._bankAccountsDictionary;
            }
            finally
            {
                _bankAccountsLock.ExitWriteLock();
            }
            _currenciesLock.EnterWriteLock();
            try
            {
                _currenciesDictionary = portfolio._currenciesDictionary;
            }
            finally
            {
                _currenciesLock.ExitWriteLock();
            }
            _benchmarksLock.EnterWriteLock();
            try
            {
                _benchMarksDictionary = portfolio._benchMarksDictionary;
            }
            finally
            {
                _benchmarksLock.ExitWriteLock();
            }
            _assetsLock.EnterWriteLock();
            try
            {
                _assetsDictionary = portfolio._assetsDictionary;
            }
            finally
            {
                _assetsLock.ExitWriteLock();
            }
            _pensionsLock.EnterWriteLock();
            try
            {
                _pensionsDictionary = portfolio._pensionsDictionary;
            }
            finally
            {
                _pensionsLock.ExitWriteLock();
            }
            NotesInternal = portfolio.NotesInternal;
        }

        /// <inheritdoc />
        public void Clear()
        {
            SetFrom(new Portfolio());
            OnPortfolioChanged(this, new PortfolioEventArgs(changedPortfolio: true));
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
            _fundsLock.EnterWriteLock();
            try
            {
                foreach (Security security in _fundsDictionary.Values)
                {
                    security.DataEdit += OnPortfolioChanged;
                    security.SetupEventListening();
                }
            }
            finally
            {
                _fundsLock.ExitWriteLock();
            }

            _bankAccountsLock.EnterWriteLock();
            try
            {
                foreach (CashAccount bankAccount in _bankAccountsDictionary.Values)
                {
                    bankAccount.DataEdit += OnPortfolioChanged;
                    bankAccount.SetupEventListening();
                }
            }
            finally
            {
                _bankAccountsLock.ExitWriteLock();
            }

            _benchmarksLock.EnterWriteLock();
            try
            {
                foreach (Sector sector in _benchMarksDictionary.Values)
                {
                    sector.DataEdit += OnPortfolioChanged;
                    sector.SetupEventListening();
                }
            }
            finally
            {
                _benchmarksLock.ExitWriteLock();
            }

            _currenciesLock.EnterWriteLock();
            try
            {
                foreach (Currency currency in _currenciesDictionary.Values)
                {
                    currency.DataEdit += OnPortfolioChanged;
                    currency.SetupEventListening();
                }
            }
            finally
            {
                _currenciesLock.ExitWriteLock();
            }

            _assetsLock.EnterWriteLock();
            try
            {
                foreach (AmortisableAsset asset in _assetsDictionary.Values)
                {
                    asset.DataEdit += OnPortfolioChanged;
                    asset.SetupEventListening();
                }
            }
            finally
            {
                _assetsLock.ExitWriteLock();
            }

            _pensionsLock.EnterWriteLock();
            try
            {
                foreach (Security pension in _pensionsDictionary.Values)
                {
                    pension.DataEdit += OnPortfolioChanged;
                    pension.SetupEventListening();
                }
            }
            finally
            {
                _pensionsLock.ExitWriteLock();
            }
        }
    }
}
