using System;
using System.Collections.Generic;
using System.Linq;

using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.FinanceStructures.Implementation.Asset;
using FinancialStructures.NamingStructures;

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

        private Dictionary<TwoName, Security> _fundsDictionary = new Dictionary<TwoName, Security>();
        
        /// <inheritdoc/>
        public IReadOnlyList<ISecurity> Funds
        {
            get
            {
                lock(_fundsLock)
                {
                    return _fundsDictionary.Values.ToList();
                }
            }
        }

        internal void AddFund(Security security)
        {
            lock(_fundsLock)
            {
                _fundsDictionary.Add(security.Names.ToTwoName(), security);
            }
        }

        /// <summary>
        /// Backing for the BankAccounts.
        /// </summary>
        private Dictionary<TwoName, CashAccount> _bankAccountsDictionary = new Dictionary<TwoName, CashAccount>();

        /// <inheritdoc/>
        public IReadOnlyList<IExchangableValueList> BankAccounts
        {
            get
            {
                lock (_bankAccountsLock)
                {
                    return _bankAccountsDictionary.Values.ToList();
                }
            }
        }

        internal void AddBankAccount(CashAccount cashAccount)
        {
            lock (_bankAccountsLock)
            {
                _bankAccountsDictionary.Add(cashAccount.Names.ToTwoName(), cashAccount);
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
                lock (_currenciesLock)
                {
                    return _currenciesDictionary.Values.ToList();
                }
            }
        }

        internal void AddCurrency(Currency currency)
        {
            lock (_currenciesLock)
            {
                _currenciesDictionary.Add(currency.Names.ToTwoName(), currency);
            }
        }

        private Dictionary<TwoName,Sector> _benchMarksDictionary = new Dictionary<TwoName,Sector>();

        /// <inheritdoc/>
        public IReadOnlyList<IValueList> BenchMarks
        {
            get
            {
                lock (_benchmarksLock)
                {
                    return _benchMarksDictionary.Values.ToList();
                }
            }
        }

        internal void AddBenchMark(Sector sector)
        {                
            lock (_benchmarksLock)
            {
                _benchMarksDictionary.Add(sector.Names.ToTwoName(), sector);
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
                lock (_assetsLock)
                {
                    return _assetsDictionary.Values.ToList();
                }
            }
        }

        internal void AddAsset(AmortisableAsset asset)
        {
            lock (_assetsLock)
            {
                _assetsDictionary.Add(asset.Names.ToTwoName(), asset);
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
                lock (_pensionsLock)
                {
                    return _pensionsDictionary.Values.ToList();
                }
            }
        }

        internal void AddPension(Security pension)
        {
            lock (_pensionsLock)
            {
                _pensionsDictionary.Add(pension.Names.ToTwoName(), pension);
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
            lock (_fundsLock)
            {
                _fundsDictionary = portfolio._fundsDictionary;
            }
            lock (_bankAccountsLock)
            {
                _bankAccountsDictionary = portfolio._bankAccountsDictionary;
            }
            lock (_currenciesLock)
            {
                _currenciesDictionary = portfolio._currenciesDictionary;
            }
            lock (_benchmarksLock)
            {
                _benchMarksDictionary = portfolio._benchMarksDictionary;
            }
            lock (_assetsLock)
            {
                _assetsDictionary = portfolio._assetsDictionary;
            }
            lock (_pensionsLock)
            {
                _pensionsDictionary = portfolio._pensionsDictionary;
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
            lock (_fundsLock)
            {
                foreach (Security security in _fundsDictionary.Values)
                {
                    security.DataEdit += OnPortfolioChanged;
                    security.SetupEventListening();
                }
            }

            lock (_bankAccountsLock)
            {
                foreach (CashAccount bankAccount in _bankAccountsDictionary.Values)
                {
                    bankAccount.DataEdit += OnPortfolioChanged;
                    bankAccount.SetupEventListening();
                }
            }

            lock (_benchmarksLock)
            {
                foreach (Sector sector in _benchMarksDictionary.Values)
                {
                    sector.DataEdit += OnPortfolioChanged;
                    sector.SetupEventListening();
                }
            }

            lock (_currenciesLock)
            {
                foreach (Currency currency in _currenciesDictionary.Values)
                {
                    currency.DataEdit += OnPortfolioChanged;
                    currency.SetupEventListening();
                }
            }

            lock (_assetsLock)
            {
                foreach (AmortisableAsset asset in _assetsDictionary.Values)
                {
                    asset.DataEdit += OnPortfolioChanged;
                    asset.SetupEventListening();
                }
            }

            lock (_pensionsLock)
            {
                foreach (Security pension in _pensionsDictionary.Values)
                {
                    pension.DataEdit += OnPortfolioChanged;
                    pension.SetupEventListening();
                }
            }
        }
    }
}
