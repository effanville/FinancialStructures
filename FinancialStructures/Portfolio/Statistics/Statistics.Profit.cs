using System.Collections.Generic;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static double TotalProfit(this IPortfolio portfolio, Account elementType, TwoName names = null)
        {
            double total = 0;
            switch (elementType)
            {
                case (Account.Security):
                {
                    foreach (ISecurity security in portfolio.Funds)
                    {
                        if (security.Any())
                        {
                            total += portfolio.Profit(elementType, security.Names);
                        }
                    }
                    break;
                }
                case (Account.BankAccount):
                {
                    foreach (var account in portfolio.BankAccounts)
                    {
                        if (account.Any())
                        {
                            total += portfolio.Profit(elementType, account.Names);
                        }
                    }
                    break;
                }
                case (Account.Benchmark):
                {
                    foreach (var benchmark in portfolio.BenchMarks)
                    {
                        if (benchmark.Any())
                        {
                            total += portfolio.Profit(elementType, benchmark.Names);
                        }
                    }
                    break;
                }
                case (Account.Currency):
                {
                    foreach (var currency in portfolio.Currencies)
                    {
                        if (currency.Any())
                        {
                            total += portfolio.Profit(elementType, currency.Names);
                        }
                    }
                    break;
                }
                case Account.Sector:
                {
                    foreach (ISecurity security in portfolio.SectorSecurities(names.Name))
                    {
                        if (security.Any())
                        {
                            total += security.LatestValue().Value - security.TotalInvestment();
                        }
                    }
                    break;
                }
                case Account.All:
                {
                    return portfolio.TotalProfit(Account.Security) + portfolio.TotalProfit(Account.BankAccount);
                }
                default:
                    break;
            }

            return total;
        }

        /// <summary>
        /// Returns the profit of the company over its lifetime in the portfolio.
        /// </summary>
        public static double Profit(this IPortfolio portfolio, Account elementType, TwoName names)
        {
            switch (elementType)
            {
                case (Account.Security):
                {
                    if (portfolio.TryGetSecurity(names, out ISecurity desired))
                    {
                        if (desired.Any())
                        {
                            ICurrency currency = portfolio.Currency(Account.Security, desired);
                            return desired.LatestValue(currency).Value - desired.TotalInvestment(currency);
                        }
                    }

                    return double.NaN;
                }
                case Account.BankAccount:
                {
                    if (portfolio.TryGetAccount(elementType, names, out ISingleValueDataList desired))
                    {
                        if (desired is ICashAccount cashAcc)
                        {
                            if (cashAcc.Any())
                            {
                                ICurrency currency = portfolio.Currency(elementType, cashAcc);
                                return cashAcc.LatestValue(currency).Value - cashAcc.FirstValue(currency).Value;
                            }
                        }
                    }

                    return double.NaN;
                }
                case Account.Currency:
                case Account.Benchmark:
                {
                    if (portfolio.TryGetAccount(elementType, names, out ISingleValueDataList desired))
                    {
                        if (desired.Any())
                        {
                            return 1000 * (desired.LatestValue().Value - desired.FirstValue().Value);
                        }
                    }

                    return double.NaN;
                }
                default:
                    return double.NaN;
            }
        }

        /// <summary>
        /// returns the profit in the company.
        /// </summary>
        public static double CompanyProfit(this IPortfolio portfolio, string company)
        {
            List<ISecurity> securities = portfolio.CompanySecurities(company);
            double value = 0;
            foreach (ISecurity security in securities)
            {
                if (security.Any())
                {
                    ICurrency currency = portfolio.Currency(Account.Security, security);
                    value += security.LatestValue(currency).Value - security.TotalInvestment(currency);
                }
            }

            return value;
        }
    }
}
