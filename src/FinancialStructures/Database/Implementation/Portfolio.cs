using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly ValueListCollection<ISecurity, Security> _funds = new ValueListCollection<ISecurity, Security>(
            Account.Security,
            (account, name) => new Security(account, name));
        
        private readonly ValueListCollection<IExchangeableValueList, CashAccount> _bankAccounts = new (
            Account.BankAccount,
            (account, name) => account == Account.BankAccount ? new CashAccount(name) : null);
        
        private readonly ValueListCollection<ICurrency, Currency> _currencies = new (
            Account.Currency,
            (account, name) => account == Account.Currency ? new Currency(name) : null);
        
        private readonly ValueListCollection<IValueList, Sector> _benchmarks = new (
            Account.Benchmark,
            (account, name) => account == Account.Benchmark ? new Sector(name) : null);
        
        private readonly ValueListCollection<IAmortisableAsset, AmortisableAsset> _assets = new (
            Account.Asset,
            (account, name) => account == Account.Asset ? new AmortisableAsset(name) : null);

        private readonly ValueListCollection<ISecurity, Security> _pensions = new (
            Account.Pension,
            (account, name) => new Security(account, name));
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

        /// <inheritdoc/>
        public IReadOnlyList<ISecurity> Funds => _funds.Values;

        internal void AddFund(Security security) => _funds.AddValueList(security);

        /// <inheritdoc/>
        public IReadOnlyList<IExchangeableValueList> BankAccounts => _bankAccounts.Values;

        internal void AddBankAccount(CashAccount cashAccount) => _bankAccounts.AddValueList(cashAccount);

        /// <inheritdoc/>
        public IReadOnlyList<ICurrency> Currencies => _currencies.Values;

        internal void AddCurrency(Currency currency) => _currencies.AddValueList(currency);

        /// <inheritdoc/>
        public IReadOnlyList<IValueList> BenchMarks => _benchmarks.Values;

        internal void AddBenchMark(Sector sector) => _benchmarks.AddValueList(sector);

        /// <inheritdoc/>
        public IReadOnlyList<IAmortisableAsset> Assets => _assets.Values;

        internal void AddAsset(AmortisableAsset asset) => _assets.AddValueList(asset);

        /// <inheritdoc />
        public IReadOnlyList<ISecurity> Pensions => _pensions.Values;

        internal void AddPension(Security pension) => _pensions.AddValueList(pension);

        /// <summary>
        /// Default parameterless constructor.
        /// </summary>
        internal Portfolio()
        {
            _funds.CollectionChanged += OnPortfolioChanged;
            _funds.CollectionItemChanged += OnPortfolioChanged;
        }

        private void SetFrom(Portfolio portfolio)
        {
            BaseCurrency = portfolio.BaseCurrency;
            Name = portfolio.Name;
            _funds.ReplaceDictionary(portfolio._funds);
            _bankAccounts.ReplaceDictionary(portfolio._bankAccounts);
            _currencies.ReplaceDictionary(portfolio._currencies);
            _benchmarks.ReplaceDictionary(portfolio._benchmarks);
            _assets.ReplaceDictionary(portfolio._assets);
            _pensions.ReplaceDictionary(portfolio._pensions);
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
        
        public event EventHandler<PortfolioEventArgs> NewPortfolio;

        public void OnNewPortfolio(object obj, PortfolioEventArgs e)
        {
            IsAlteredSinceSave = true;
            EventHandler<PortfolioEventArgs> handler = NewPortfolio;
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
        public void CleanData()
        {
            foreach (ISecurity security in Funds)
            {
                security.CleanData();
            }
        }

        public void WireDataChangedEvents()
        {
            _funds.SetupCollectionChangedEvents();
            _bankAccounts.SetupCollectionChangedEvents();
            _benchmarks.SetupCollectionChangedEvents();
            _assets.SetupCollectionChangedEvents();
            _pensions.SetupCollectionChangedEvents();
        }
    }
}
