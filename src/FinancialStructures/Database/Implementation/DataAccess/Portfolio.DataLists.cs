using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation
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
            Portfolio portfolioCopy = new ()
            {
                BaseCurrency = BaseCurrency,
                Name = Name
            };
            
            portfolioCopy._funds.CopyFrom(_funds);
            portfolioCopy._bankAccounts.CopyFrom(_bankAccounts);
            portfolioCopy._currencies.CopyFrom(_currencies);
            portfolioCopy._benchmarks.CopyFrom(_benchmarks);
            portfolioCopy._assets.CopyFrom(_assets);
            portfolioCopy._pensions.CopyFrom(_pensions);
            return portfolioCopy;
        }


        /// <inheritdoc/>
        public IReadOnlyList<IValueList> Accounts(Account account)
        {
            switch (account)
            {
                case Account.All:
                {
                    List<IValueList> accountList  = new List<IValueList>();
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
        public IReadOnlyList<IValueList> Accounts(Totals account, string identifier)
        {
            switch (account)
            {
                case Totals.SecurityCompany:
                {
                    return Funds.Where(fund => fund.Names.Company == identifier).ToList();
                }
                case Totals.BankAccountCompany:
                {
                    return BankAccounts.Where(fund => fund.Names.Company == identifier).ToList();
                }
                case Totals.AssetCompany:
                {
                    return Assets.Where(asset => asset.Names.Company == identifier).ToList();
                }
                case Totals.PensionCompany:
                {
                    return Pensions.Where(pension => pension.Names.Company == identifier).ToList();
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
                    return Funds.Union(BankAccounts
                        .Union(Assets))
                        .Union(Pensions)
                        .ToList();
                }
                case Totals.SecuritySector:
                {
                    return Funds.Where(fund => fund.IsSectorLinked(identifier)).ToList();
                }
                case Totals.BankAccountSector:
                {
                    return BankAccounts.Where(fund => fund.IsSectorLinked(identifier)).ToList();
                }
                case Totals.AssetSector:
                {
                    return Assets.Where(fund => fund.IsSectorLinked(identifier)).ToList();
                }
                case Totals.PensionSector:
                {
                    return Pensions.Where(fund => fund.IsSectorLinked(identifier)).ToList();
                }
                case Totals.Sector:
                {
                    return Accounts(Totals.SecuritySector, identifier)
                        .Union(Accounts(Totals.AssetSector, identifier))
                        .Union(Accounts(Totals.BankAccountSector, identifier))
                        .Union(Accounts(Totals.PensionSector, identifier))
                        .ToList();
                }
                case Totals.Company:
                {
                    return Accounts(Totals.SecurityCompany, identifier)
                        .Union(Accounts(Totals.AssetCompany, identifier))
                        .Union(Accounts(Totals.BankAccountCompany, identifier))
                        .Union(Accounts(Totals.PensionCompany, identifier))
                        .ToList();
                }
                case Totals.Currency:
                {
                    return Accounts(Totals.SecurityCurrency, identifier)
                        .Union(Accounts(Totals.AssetCurrency, identifier))
                        .Union(Accounts(Totals.BankAccountCurrency, identifier))
                        .Union(Accounts(Totals.PensionCurrency, identifier))
                        .ToList();
                }
                case Totals.SecurityCurrency:
                {
                    bool isBase = string.Equals(identifier, BaseCurrency);
                    return Funds.Where(fund => isBase ?( fund.Names.Currency == identifier || fund.Names.Currency == null ) : fund.Names.Currency == identifier).ToList();
                }
                case Totals.BankAccountCurrency:
                {
                    bool isBase = string.Equals(identifier, BaseCurrency);
                    return BankAccounts.Where(fund => isBase ?( fund.Names.Currency == identifier || fund.Names.Currency == null ) : fund.Names.Currency == identifier).ToList();
                }
                case Totals.AssetCurrency:
                {
                    bool isBase = string.Equals(identifier, BaseCurrency);
                    return Assets.Where(fund
                        => isBase ?( fund.Names.Currency == identifier || fund.Names.Currency == null ) : fund.Names.Currency == identifier ).ToList();
                }
                case Totals.PensionCurrency:
                {
                    bool isBase = string.Equals(identifier, BaseCurrency);
                    return Pensions.Where(fund => isBase ?( fund.Names.Currency == identifier || fund.Names.Currency == null ) : fund.Names.Currency == identifier).ToList();
                }
                case Totals.CurrencySector:
                default:
                    throw new NotImplementedException($"Total value {account} not implemented for {nameof(IPortfolio)}.{nameof(Accounts)}");
            }
        }
    }
}
