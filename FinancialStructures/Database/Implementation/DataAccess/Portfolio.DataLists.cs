﻿using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.FinanceStructures.Implementation.Asset;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <summary>
        /// Returns a copy of the currently held portfolio.
        /// Note one cannot use this portfolio to edit as it makes a copy.
        /// </summary>
        /// <remarks>
        /// This is in theory dangerous. I know thought that a security copied
        /// returns a genuine security, so I can case without trouble.
        /// </remarks>
        public IPortfolio Copy()
        {
            Portfolio portfolioCopy = new Portfolio
            {
                BaseCurrency = BaseCurrency,
                Name = Name
            };
            
            _fundsLock.EnterReadLock();
            try
            {
                foreach (var security in _fundsDictionary)
                {
                    portfolioCopy._fundsDictionary.Add(security.Key, (Security)security.Value.Copy());
                }
            }
            finally
            {
                _fundsLock.ExitReadLock();
            }

            _bankAccountsLock.EnterReadLock();
            try
            {
                foreach (var bankAcc in _bankAccountsDictionary)
                {
                    portfolioCopy._bankAccountsDictionary.TryAdd(bankAcc.Key, (CashAccount)bankAcc.Value.Copy());
                }
            }
            finally
            {
                _bankAccountsLock.ExitReadLock();
            }

            _currenciesLock.EnterReadLock();
            try
            {
                foreach (var currency in _currenciesDictionary)
                {
                    portfolioCopy._currenciesDictionary.Add(currency.Key, (Currency)currency.Value.Copy());
                }
            }
            finally
            {
                _currenciesLock.ExitReadLock();
            }

            _benchmarksLock.EnterReadLock();
            try
            {
                foreach (var sector in _benchMarksDictionary)
                {
                    portfolioCopy._benchMarksDictionary.Add(sector.Key, (Sector)sector.Value.Copy());
                }
            }
            finally
            {
                _benchmarksLock.ExitReadLock();
            }

            _assetsLock.EnterReadLock();
            try
            {
                foreach (var asset in _assetsDictionary)
                {
                    portfolioCopy._assetsDictionary.Add(asset.Key, (AmortisableAsset)asset.Value.Copy());
                }
            }
            finally
            {
                _assetsLock.ExitReadLock();
            }

            _pensionsLock.EnterReadLock();
            try
            {
                foreach (var pension in _pensionsDictionary)
                {
                    portfolioCopy._pensionsDictionary.Add(pension.Key, (Security)pension.Value.Copy());
                }
            }
            finally
            {
                _pensionsLock.ExitReadLock();
            }

            return portfolioCopy;
        }

        /// <inheritdoc/>
        public IReadOnlyList<IValueList> Accounts(Account account)
        {
            switch (account)
            {
                case Account.All:
                {
                    List<IValueList> accountList = new List<IValueList>();
                    accountList.AddRange(Funds);
                    accountList.AddRange(BankAccounts);
                    accountList.AddRange(Assets);
                    accountList.AddRange(Pensions);
                    return accountList;
                }
                case Account.Security:
                {
                    return Funds;
                }
                case Account.BankAccount:
                {
                    return BankAccounts;
                }
                case Account.Asset:
                {
                    return Assets;
                }
                case Account.Benchmark:
                {
                    return BenchMarks;
                }
                case Account.Currency:
                {
                    return Currencies;
                }
                case Account.Pension:
                {
                    return Pensions;
                }
                default:
                    return null;
            }
        }

        /// <inheritdoc/>
        public IReadOnlyList<IValueList> Accounts(Totals account, TwoName name)
        {
            switch (account)
            {
                case Totals.SecurityCompany:
                {
                    return Funds.Where(fund => fund.Names.Company == name.Company).ToList();
                }
                case Totals.BankAccountCompany:
                {
                    return BankAccounts.Where(fund => fund.Names.Company == name.Company).ToList();
                }
                case Totals.AssetCompany:
                {
                    return Assets.Where(asset => asset.Names.Company == name.Company).ToList();
                }
                case Totals.PensionCompany:
                {
                    return Pensions.Where(pension => pension.Names.Company == name.Company).ToList();
                }
                case Totals.Security:
                {
                    return Funds;
                }
                case Totals.Benchmark:
                {
                    return BenchMarks;
                }
                case Totals.BankAccount:
                {
                    return BankAccounts;
                }
                case Totals.Asset:
                {
                    return Assets;
                }
                case Totals.Pension:
                {
                    return Pensions;
                }
                case Totals.All:
                {
                    return Funds
                        .Union(BankAccounts
                        .Union(Assets))
                        .Union(Pensions)
                        .ToList();
                }
                case Totals.SecuritySector:
                {
                    return Funds.Where(fund => fund.IsSectorLinked(name)).ToList();
                }
                case Totals.BankAccountSector:
                {
                    return BankAccounts.Where(fund => fund.IsSectorLinked(name)).ToList();
                }
                case Totals.AssetSector:
                {
                    return Assets.Where(fund => fund.IsSectorLinked(name)).ToList();
                }
                case Totals.PensionSector:
                {
                    return Pensions.Where(fund => fund.IsSectorLinked(name)).ToList();
                }
                case Totals.Sector:
                {
                    return Accounts(Totals.SecuritySector, name)
                        .Union(Accounts(Totals.AssetSector, name))
                        .Union(Accounts(Totals.BankAccountSector, name))
                        .Union(Accounts(Totals.PensionSector, name))
                        .ToList();
                }
                case Totals.Company:
                {
                    return Accounts(Totals.SecurityCompany, name)
                        .Union(Accounts(Totals.AssetCompany, name))
                        .Union(Accounts(Totals.BankAccountCompany, name))
                        .Union(Accounts(Totals.PensionCompany, name))
                        .ToList();
                }
                case Totals.Currency:
                {
                    return Accounts(Totals.SecurityCurrency, name)
                        .Union(Accounts(Totals.AssetCurrency, name))
                        .Union(Accounts(Totals.BankAccountCurrency, name))
                        .Union(Accounts(Totals.PensionCurrency, name))
                        .ToList();
                }
                case Totals.SecurityCurrency:
                {
                    return Funds.Where(fund => fund.Names.Currency == name.Company).ToList();
                }
                case Totals.BankAccountCurrency:
                {
                    return BankAccounts.Where(fund => fund.Names.Currency == name.Company).ToList();
                }
                case Totals.AssetCurrency:
                {
                    return Assets.Where(fund => fund.Names.Currency == name.Company).ToList();
                }
                case Totals.PensionCurrency:
                {
                    return Pensions.Where(fund => fund.Names.Currency == name.Company).ToList();
                }
                case Totals.CurrencySector:
                default:
                    throw new NotImplementedException($"Total value {account} not implemented for {nameof(IPortfolio)}.{nameof(Accounts)}");
            }
        }
    }
}
