using System;
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
            Portfolio PortfoCopy = new Portfolio
            {
                BaseCurrency = BaseCurrency,
                FilePath = FilePath
            };

            foreach (Security security in Funds)
            {
                PortfoCopy.Funds.Add((Security)security.Copy());
            }
            foreach (CashAccount bankAcc in BankAccounts)
            {
                PortfoCopy.BankAccounts.Add((CashAccount)bankAcc.Copy());
            }
            foreach (Currency currency in Currencies)
            {
                PortfoCopy.Currencies.Add((Currency)currency.Copy());
            }
            foreach (Sector sector in BenchMarks)
            {
                PortfoCopy.BenchMarks.Add((Sector)sector.Copy());
            }
            foreach (AmortisableAsset asset in AssetsBackingList)
            {
                PortfoCopy.AssetsBackingList.Add((AmortisableAsset)asset.Copy());
            }

            return PortfoCopy;
        }

        /// <inheritdoc/>
        public IReadOnlyList<IValueList> Accounts(Account account)
        {
            switch (account)
            {
                case Account.All:
                {
                    List<IValueList> accountList = new List<IValueList>();
                    accountList.AddRange(FundsThreadSafe);
                    accountList.AddRange(BankAccountsThreadSafe);
                    accountList.AddRange(Assets);
                    return accountList;
                }
                case Account.Security:
                {
                    return FundsThreadSafe;
                }
                case Account.BankAccount:
                {
                    return BankAccountsThreadSafe;
                }
                case Account.Asset:
                {
                    return Assets;
                }
                case Account.Benchmark:
                {
                    return BenchMarksThreadSafe;
                }
                case Account.Currency:
                {
                    return CurrenciesThreadSafe;
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
                    return FundsThreadSafe.Where(fund => fund.Names.Company == name.Company).ToList();
                }
                case Totals.BankAccountCompany:
                {
                    return BankAccountsThreadSafe.Where(fund => fund.Names.Company == name.Company).ToList();
                }
                case Totals.Security:
                {
                    return FundsThreadSafe;
                }
                case Totals.Benchmark:
                {
                    return BenchMarksThreadSafe;
                }
                case Totals.BankAccount:
                {
                    return BankAccountsThreadSafe;
                }
                case Totals.Asset:
                {
                    return Assets;
                }
                case Totals.AssetCompany:
                {
                    return Assets.Where(asset => asset.Names.Company == name.Company).ToList();
                }
                case Totals.All:
                {
                    return FundsThreadSafe
                        .Union(BankAccountsThreadSafe
                        .Union(Assets))
                        .ToList();
                }
                case Totals.SecuritySector:
                {
                    return FundsThreadSafe.Where(fund => fund.IsSectorLinked(name)).ToList();
                }
                case Totals.BankAccountSector:
                {
                    return BankAccountsThreadSafe.Where(fund => fund.IsSectorLinked(name)).ToList();
                }
                case Totals.AssetSector:
                {
                    return Assets.Where(fund => fund.IsSectorLinked(name)).ToList();
                }
                case Totals.Sector:
                {
                    return Accounts(Totals.SecuritySector, name)
                        .Union(Accounts(Totals.AssetSector, name))
                        .Union(Accounts(Totals.BankAccountSector, name))
                        .ToList();
                }
                case Totals.Company:
                {
                    return Accounts(Totals.SecurityCompany, name)
                        .Union(Accounts(Totals.AssetCompany, name))
                        .Union(Accounts(Totals.BankAccountCompany, name))
                        .ToList();
                }
                case Totals.Currency:
                {
                    return Accounts(Totals.SecurityCurrency, name)
                        .Union(Accounts(Totals.AssetCurrency, name))
                        .Union(Accounts(Totals.BankAccountCurrency, name))
                        .ToList();
                }
                case Totals.SecurityCurrency:
                {
                    return FundsThreadSafe.Where(fund => fund.Names.Currency==name.Company).ToList();
                }
                case Totals.BankAccountCurrency:
                {
                    return BankAccountsThreadSafe.Where(fund => fund.Names.Currency == name.Company).ToList();
                }
                case Totals.AssetCurrency:
                {
                    return Assets.Where(fund => fund.Names.Currency == name.Company).ToList();
                }
                case Totals.CurrencySector:
                default:
                    throw new NotImplementedException($"Total value {account} not implemented for {nameof(IPortfolio)}.{nameof(Accounts)}");
            }
        }
    }
}
