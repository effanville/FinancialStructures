using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static double RecentChange(this IPortfolio portfolio, Totals totals = Totals.Security, TwoName names = null)
        {
            switch (totals)
            {
                case Totals.All:
                {
                    return portfolio.RecentChange(Totals.Security) + portfolio.RecentChange(Totals.BankAccount);
                }
                case Totals.Security:
                {
                    return RecentChangeOf(portfolio.FundsThreadSafe, portfolio, totals);
                }
                case Totals.SecurityCompany:
                case Totals.BankAccountCompany:
                {
                    return RecentChangeOf(portfolio.CompanyAccounts(totals.ToAccount(), names?.Company), portfolio, totals);
                }
                case Totals.BankAccount:
                {
                    return RecentChangeOf(portfolio.BankAccountsThreadSafe, portfolio, totals);
                }
                case Totals.SecuritySector:
                {
                    double total = 0;
                    foreach (ISecurity security in portfolio.FundsThreadSafe)
                    {
                        if (security.IsSectorLinked(names) && security.Any())
                        {
                            ICurrency currency = portfolio.Currency(totals.ToAccount(), security);
                            total += security.RecentChange(currency);
                        }
                    }

                    return total;
                }
                case Totals.BankAccountSector:
                {
                    double total = 0;
                    foreach (IExchangableValueList bankAccount in portfolio.BankAccountsThreadSafe)
                    {
                        if (bankAccount.IsSectorLinked(names) && bankAccount.Any())
                        {
                            ICurrency currency = portfolio.Currency(totals.ToAccount(), bankAccount);
                            total += bankAccount.RecentChange(currency);
                        }
                    }

                    return total;
                }
                case Totals.Sector:
                {
                    return portfolio.RecentChange(Totals.BankAccountSector, names) + portfolio.RecentChange(Totals.SecuritySector, names);
                }
                case Totals.Company:
                {
                    return portfolio.RecentChange(Totals.SecurityCompany, names) + portfolio.RecentChange(Totals.BankAccountCompany, names);
                }
                case Totals.Benchmark:
                case Totals.Currency:
                case Totals.CurrencySector:
                case Totals.SecurityCurrency:
                case Totals.BankAccountCurrency:
                default:
                {
                    return 0.0;
                }
            }
        }

        private static double RecentChangeOf(IReadOnlyList<IValueList> accounts, IPortfolio portfolio, Totals totals)
        {
            double total = 0;
            foreach (IExchangableValueList valueList in accounts)
            {
                if (valueList.Any())
                {
                    ICurrency currency = portfolio.Currency(totals.ToAccount(), valueList);
                    total += valueList.RecentChange(currency);
                }
            }

            return total;
        }

        /// <summary>
        /// returns the change between the most recent two valuations of the security.
        /// </summary>
        public static double RecentChange(this IPortfolio portfolio, Account elementType, TwoName names)
        {
            switch (elementType)
            {
                case Account.Security:
                case Account.BankAccount:
                {
                    if (portfolio.TryGetAccount(elementType, names, out IValueList account) && account.Any())
                    {
                        IExchangableValueList exchangeValueList = account as IExchangableValueList;
                        ICurrency currency = portfolio.Currency(elementType, exchangeValueList);
                        return exchangeValueList.RecentChange(currency);
                    }

                    break;
                }
                default:
                {
                    if (portfolio.TryGetAccount(elementType, names, out IValueList valueList) && valueList.Any())
                    {
                        return valueList.RecentChange();
                    }
                    break;
                }
            }

            return double.NaN;
        }
    }
}
