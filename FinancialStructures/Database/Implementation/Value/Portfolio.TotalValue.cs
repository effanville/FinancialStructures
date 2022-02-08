using System;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public decimal TotalValue(Totals elementType, TwoName names = null)
        {
            return TotalValue(elementType, DateTime.Today, names);
        }

        /// <inheritdoc/>
        public decimal TotalValue(Totals elementType, DateTime date, TwoName names = null)
        {
            switch (elementType)
            {
                case Totals.Security:
                {
                    decimal total = 0;
                    foreach (ISecurity sec in FundsThreadSafe)
                    {
                        if (sec.Any())
                        {
                            ICurrency currency = Currency(sec);
                            total += sec.Value(date, currency).Value;
                        }
                    }

                    return total;
                }
                case Totals.Currency:
                {
                    decimal total = 0;
                    foreach (ISecurity sec in FundsThreadSafe)
                    {
                        if (sec.Any() && sec.Names.Currency == names.Name)
                        {
                            ICurrency currency = Currency(sec);
                            total += sec.Value(date, currency).Value;
                        }
                    }
                    foreach (IExchangableValueList acc in BankAccountsThreadSafe)
                    {
                        if (acc.Any() && acc.Names.Currency == names.Name)
                        {
                            ICurrency currency = Currency(acc);
                            total += acc.ValueOnOrBefore(date, currency).Value;
                        }
                    }
                    return total;
                }
                case Totals.BankAccount:
                {
                    decimal sum = 0;
                    foreach (IExchangableValueList acc in BankAccountsThreadSafe)
                    {
                        if (acc.Any())
                        {
                            ICurrency currency = Currency(acc);
                            sum += acc.ValueOnOrBefore(date, currency).Value;
                        }
                    }

                    return sum;
                }
                case Totals.SecuritySector:
                {
                    decimal sum = 0;
                    foreach (ISecurity fund in FundsThreadSafe)
                    {
                        if (fund.IsSectorLinked(names))
                        {
                            sum += fund.Value(date).Value;
                        }
                    }

                    return sum;
                }
                case Totals.BankAccountSector:
                {
                    decimal sum = 0;

                    foreach (IExchangableValueList fund in BankAccountsThreadSafe)
                    {
                        if (fund.IsSectorLinked(names))
                        {
                            sum += fund.ValueOnOrBefore(date).Value;
                        }
                    }

                    return sum;
                }
                case Totals.Sector:
                {
                    return TotalValue(Totals.SecuritySector, date, names) + TotalValue(Totals.BankAccountSector, date, names) + TotalValue(Totals.AssetSector, date, names);
                }
                case Totals.All:
                {
                    return TotalValue(Totals.Security, date) + TotalValue(Totals.BankAccount, date) + TotalValue(Totals.Asset, date);
                }
                case Totals.SecurityCompany:
                {
                    IReadOnlyList<IValueList> companyAccounts = CompanyAccounts(elementType.ToAccount(), names.Company);
                    decimal value = 0;
                    foreach (IExchangableValueList valueList in companyAccounts)
                    {
                        if (valueList.Any())
                        {
                            ICurrency currency = Currency(valueList);
                            value += valueList.Value(date, currency).Value;
                        }
                    }

                    return value;
                }
                case Totals.BankAccountCompany:
                {
                    IReadOnlyList<IValueList> bankAccounts = CompanyAccounts(Account.BankAccount, names.Company);
                    if (bankAccounts.Count == 0)
                    {
                        return 0.0m;
                    }
                    decimal value = 0;
                    foreach (IExchangableValueList account in bankAccounts)
                    {
                        if (account != null && account.Any())
                        {
                            ICurrency currency = Currency(account);
                            value += account.ValueOnOrBefore(date, currency).Value;
                        }
                    }

                    return value;
                }
                case Totals.Company:
                {
                    return TotalValue(Totals.BankAccountCompany, date, names) + TotalValue(Totals.SecurityCompany, date, names) + TotalValue(Totals.AssetCompany, date, names);
                }
                case Totals.Asset:
                {
                    decimal total = 0;
                    foreach (IAmortisableAsset asset in Assets)
                    {
                        if (asset.Any())
                        {
                            ICurrency currency = Currency(asset);
                            total += asset.Value(date, currency).Value;
                        }
                    }

                    return total;
                }
                case Totals.AssetCompany:
                {
                    IReadOnlyList<IValueList> accounts = CompanyAccounts(Account.Asset, names.Company);
                    if (accounts.Count == 0)
                    {
                        return 0.0m;
                    }

                    decimal value = 0;
                    foreach (IAmortisableAsset account in accounts)
                    {
                        if (account != null && account.Any())
                        {
                            ICurrency currency = Currency(account);
                            value += account.Value(date, currency).Value;
                        }
                    }

                    return value;
                }
                case Totals.AssetSector:
                {
                    decimal sum = 0;
                    foreach (IAmortisableAsset fund in Assets)
                    {
                        if (fund.IsSectorLinked(names))
                        {
                            sum += fund.Value(date).Value;
                        }
                    }

                    return sum;
                }
                case Totals.AssetCurrency:
                case Totals.CurrencySector:
                case Totals.SecurityCurrency:
                case Totals.BankAccountCurrency:
                case Totals.Benchmark:
                default:
                    break;
            }

            return 0.0m;
        }
    }
}
