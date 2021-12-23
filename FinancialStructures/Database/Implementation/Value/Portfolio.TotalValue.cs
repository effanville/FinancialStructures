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
                            ICurrency currency = Currency(Account.Security, sec);
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
                            ICurrency currency = Currency(Account.Security, sec);
                            total += sec.Value(date, currency).Value;
                        }
                    }
                    foreach (IExchangableValueList acc in BankAccountsThreadSafe)
                    {
                        if (acc.Any() && acc.Names.Currency == names.Name)
                        {
                            ICurrency currency = Currency(Account.BankAccount, acc);
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
                            ICurrency currency = Currency(Account.BankAccount, acc);
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
                            sum += fund.ValueOnOrBefore(date).Value;
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
                    return TotalValue(Totals.SecuritySector, date, names) + TotalValue(Totals.BankAccountSector, date, names);
                }
                case Totals.All:
                {
                    return TotalValue(Totals.Security, date) + TotalValue(Totals.BankAccount, date);
                }
                case Totals.SecurityCompany:
                {
                    IReadOnlyList<IValueList> companyAccounts = CompanyAccounts(elementType.ToAccount(), names.Company);
                    decimal value = 0;
                    foreach (IExchangableValueList valueList in companyAccounts)
                    {
                        if (valueList.Any())
                        {
                            ICurrency currency = Currency(elementType.ToAccount(), valueList);
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
                            ICurrency currency = Currency(Account.BankAccount, account);
                            value += account.ValueOnOrBefore(date, currency).Value;
                        }
                    }

                    return value;
                }
                case Totals.Company:
                {
                    return TotalValue(Totals.BankAccountCompany, date, names) + TotalValue(Totals.SecurityCompany, date, names);
                }

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
