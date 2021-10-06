using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static double TotalProfit(this IPortfolio portfolio, Totals totals, TwoName names = null)
        {
            switch (totals)
            {
                case Totals.Security:
                {
                    double total = 0;
                    foreach (ISecurity security in portfolio.FundsThreadSafe)
                    {
                        if (security.Any())
                        {
                            ICurrency currency = portfolio.Currency(totals.ToAccount(), security);
                            total += security.Profit(currency);
                        }
                    }

                    return total;
                }
                case Totals.SecurityCompany:
                {
                    double total = 0;
                    IReadOnlyList<IValueList> securities = portfolio.CompanyAccounts(Account.Security, names.Company);
                    foreach (ISecurity security in securities)
                    {
                        if (security.Any())
                        {
                            Account accountType = totals.ToAccount();
                            ICurrency currency = portfolio.Currency(accountType, security);
                            total += security.Profit(currency, accountType);
                            ;
                        }
                    }

                    return total;
                }
                case Totals.BankAccount:
                {
                    double total = 0;
                    foreach (IExchangableValueList account in portfolio.BankAccountsThreadSafe)
                    {
                        if (account.Any())
                        {
                            Account accountType = totals.ToAccount();
                            ICurrency currency = portfolio.Currency(accountType, account);
                            total += account.Profit(currency, accountType);
                        }
                    }

                    return total;
                }
                case Totals.BankAccountCompany:
                {
                    double total = 0;
                    IReadOnlyList<IValueList> bankAccounts = portfolio.CompanyAccounts(Account.BankAccount, names.Company);
                    foreach (IExchangableValueList account in bankAccounts)
                    {
                        if (account.Any())
                        {
                            Account accountType = totals.ToAccount();
                            ICurrency currency = portfolio.Currency(accountType, account);
                            total += account.Profit(currency, accountType);
                        }
                    }

                    return total;
                }
                case Totals.Benchmark:
                {
                    double total = 0;
                    foreach (IValueList benchmark in portfolio.BenchMarksThreadSafe)
                    {
                        if (benchmark.Any())
                        {
                            total += benchmark.Profit();
                        }
                    }

                    return total;
                }
                case Totals.Currency:
                {
                    double total = 0;
                    foreach (IValueList currency in portfolio.CurrenciesThreadSafe)
                    {
                        if (currency.Any())
                        {
                            total += currency.Profit();
                        }
                    }

                    return total;
                }
                case Totals.Sector:
                case Totals.SecuritySector:
                {
                    double total = 0;
                    foreach (ISecurity security in portfolio.SectorAccounts(Account.Security, names))
                    {
                        ICurrency currency = portfolio.Currency(totals.ToAccount(), security);
                        total += security.Profit(currency);
                    }

                    return total;
                }
                case Totals.All:
                {
                    return portfolio.TotalProfit(Totals.Security, names) + portfolio.TotalProfit(Totals.BankAccount, names);
                }
                case Totals.Company:
                {
                    return portfolio.TotalProfit(Totals.SecurityCompany, names) + portfolio.TotalProfit(Totals.BankAccountCompany, names);
                }
                case Totals.BankAccountSector:
                case Totals.CurrencySector:
                case Totals.SecurityCurrency:
                case Totals.BankAccountCurrency:
                default:
                    return double.NaN;
            }
        }

        /// <summary>
        /// Returns the profit of the company over its lifetime in the portfolio.
        /// </summary>
        public static double Profit(this IPortfolio portfolio, Account account, TwoName names)
        {
            switch (account)
            {
                case Account.Security:
                case Account.BankAccount:
                {
                    if (portfolio.TryGetAccount(account, names, out IValueList desired))
                    {
                        if (desired is IExchangableValueList cashAcc)
                        {
                            ICurrency currency = portfolio.Currency(account, desired);
                            return cashAcc.Profit(currency, account);
                        }
                    }

                    return double.NaN;
                }
                case Account.Currency:
                case Account.Benchmark:
                {
                    if (portfolio.TryGetAccount(account, names, out IValueList desired))
                    {
                        return desired.Profit();
                    }

                    return double.NaN;
                }

                case Account.All:
                default:
                    return double.NaN;
            }
        }
    }
}
