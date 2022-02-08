using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
{
    /// <summary>
    /// Contains Extension methods for calculating statistics.
    /// </summary>
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static decimal TotalProfit(this IPortfolio portfolio, Totals totals, TwoName names = null)
        {
            switch (totals)
            {
                case Totals.Security:
                {
                    decimal total = 0;
                    foreach (ISecurity security in portfolio.FundsThreadSafe)
                    {
                        if (security.Any())
                        {
                            ICurrency currency = portfolio.Currency(security);
                            total += security.Profit(currency);
                        }
                    }

                    return total;
                }
                case Totals.SecurityCompany:
                {
                    decimal total = 0;
                    IReadOnlyList<IValueList> securities = portfolio.CompanyAccounts(Account.Security, names.Company);
                    foreach (ISecurity security in securities)
                    {
                        if (security.Any())
                        {
                            ICurrency currency = portfolio.Currency(security);
                            total += security.Profit(currency);
                            ;
                        }
                    }

                    return total;
                }
                case Totals.BankAccount:
                {
                    decimal total = 0;
                    foreach (IExchangableValueList account in portfolio.BankAccountsThreadSafe)
                    {
                        if (account.Any())
                        {
                            Account accountType = totals.ToAccount();
                            ICurrency currency = portfolio.Currency(account);
                            total += account.Profit(currency);
                        }
                    }

                    return total;
                }
                case Totals.BankAccountCompany:
                {
                    decimal total = 0;
                    IReadOnlyList<IValueList> bankAccounts = portfolio.CompanyAccounts(Account.BankAccount, names.Company);
                    foreach (IExchangableValueList account in bankAccounts)
                    {
                        if (account.Any())
                        {
                            Account accountType = totals.ToAccount();
                            ICurrency currency = portfolio.Currency(account);
                            total += account.Profit(currency);
                        }
                    }

                    return total;
                }
                case Totals.Benchmark:
                {
                    return Profit(portfolio.BenchMarksThreadSafe);
                }
                case Totals.Asset:
                {
                    return Profit(portfolio.Assets);
                }
                case Totals.Currency:
                {
                    return Profit(portfolio.CurrenciesThreadSafe);
                }
                case Totals.Sector:
                case Totals.SecuritySector:
                {
                    decimal total = 0;
                    foreach (ISecurity security in portfolio.SectorAccounts(Account.Security, names))
                    {
                        ICurrency currency = portfolio.Currency(security);
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
                    return 0.0m;
            }
        }

        private static decimal Profit(IReadOnlyList<IValueList> accounts)
        {
            decimal total = 0;
            foreach (IValueList accountList in accounts)
            {
                total += accountList.Profit();
            }

            return total;
        }

        /// <summary>
        /// Returns the profit of the company over its lifetime in the portfolio.
        /// </summary>
        public static decimal Profit(this IPortfolio portfolio, Account account, TwoName names)
        {
            switch (account)
            {
                case Account.Security:
                case Account.BankAccount:
                case Account.Asset:
                {
                    if (portfolio.TryGetAccount(account, names, out IValueList desired))
                    {
                        if (desired is IExchangableValueList cashAcc)
                        {
                            ICurrency currency = portfolio.Currency(desired);
                            return cashAcc.Profit(currency);
                        }
                    }

                    return 0.0m;
                }
                case Account.Currency:
                case Account.Benchmark:
                {
                    if (portfolio.TryGetAccount(account, names, out IValueList desired))
                    {
                        return desired.Profit();
                    }

                    return 0.0m;
                }

                case Account.All:
                default:
                    return 0.0m;
            }
        }
    }
}
