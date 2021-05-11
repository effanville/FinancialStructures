using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static double TotalProfit(this IPortfolio portfolio, Totals elementType, TwoName names = null)
        {
            double total = 0;
            switch (elementType)
            {
                case (Totals.Security):
                {
                    foreach (ISecurity security in portfolio.FundsThreadSafe)
                    {
                        if (security.Any())
                        {
                            total += portfolio.Profit(EnumConvert.ConvertTotalToAccount(elementType), security.Names);
                        }
                    }
                    break;
                }
                case (Totals.SecurityCompany):
                {
                    List<ISecurity> securities = portfolio.CompanySecurities(names.Company);
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
                case (Totals.BankAccount):
                {
                    foreach (var account in portfolio.BankAccountsThreadSafe)
                    {
                        if (account.Any())
                        {
                            total += portfolio.Profit(EnumConvert.ConvertTotalToAccount(elementType), account.Names);
                        }
                    }
                    break;
                }
                case (Totals.Benchmark):
                {
                    foreach (IValueList benchmark in portfolio.BenchMarksThreadSafe)
                    {
                        if (benchmark.Any())
                        {
                            total += portfolio.Profit(EnumConvert.ConvertTotalToAccount(elementType), benchmark.Names);
                        }
                    }
                    break;
                }
                case (Totals.Currency):
                {
                    foreach (IValueList currency in portfolio.CurrenciesThreadSafe)
                    {
                        if (currency.Any())
                        {
                            total += portfolio.Profit(EnumConvert.ConvertTotalToAccount(elementType), currency.Names);
                        }
                    }
                    break;
                }
                case Totals.Sector:
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
                case Totals.All:
                {
                    return portfolio.TotalProfit(Totals.Security) + portfolio.TotalProfit(Totals.BankAccount);
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
                    if (portfolio.TryGetAccount(elementType, names, out IValueList desired))
                    {
                        if (desired.Any())
                        {
                            var security = desired as ISecurity;
                            ICurrency currency = portfolio.Currency(Account.Security, security);
                            return security.LatestValue(currency).Value - security.TotalInvestment(currency);
                        }
                    }

                    return double.NaN;
                }
                case Account.BankAccount:
                {
                    if (portfolio.TryGetAccount(elementType, names, out IValueList desired))
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
                    if (portfolio.TryGetAccount(elementType, names, out IValueList desired))
                    {
                        if (desired.Any())
                        {
                            return desired.LatestValue().Value - desired.FirstValue().Value;
                        }
                    }

                    return double.NaN;
                }
                default:
                    return double.NaN;
            }
        }
    }
}
