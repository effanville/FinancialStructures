using System;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using StructureCommon.FinanceFunctions;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double IRRTotal(this IPortfolio portfolio, Totals accountType, TwoName name = null)
        {
            DateTime earlierTime = portfolio.FirstValueDate(accountType, name.Name);
            DateTime laterTime = portfolio.LatestDate(accountType, name.Name);
            return portfolio.IRRTotal(accountType, earlierTime, laterTime, name);
        }

        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double IRRTotal(this IPortfolio portfolio, Totals accountType, DateTime earlierTime, DateTime laterTime, TwoName name = null)
        {
            switch (accountType)
            {
                case Totals.All:
                case Totals.Security:
                {
                    if (portfolio.NumberOf(Account.Security) == 0)
                    {
                        return double.NaN;
                    }
                    double earlierValue = 0;
                    double laterValue = 0;
                    List<DailyValuation> Investments = new List<DailyValuation>();

                    foreach (ISecurity security in portfolio.Funds)
                    {
                        if (security.Any())
                        {
                            ICurrency currency = portfolio.Currencies.Find(cur => cur.Names.Name == security.Names.Currency);
                            earlierValue += security.Value(earlierTime, currency).Value;
                            laterValue += security.Value(laterTime, currency).Value;
                            Investments.AddRange(security.InvestmentsBetween(earlierTime, laterTime, currency));
                        }
                    }

                    return FinancialFunctions.IRRTime(new DailyValuation(earlierTime, earlierValue), Investments, new DailyValuation(laterTime, laterValue));
                }
                case Totals.SecurityCompany:
                {
                    List<ISecurity> securities = portfolio.CompanySecurities(name.Company);
                    if (securities.Count == 0)
                    {
                        return double.NaN;
                    }
                    double earlierValue = 0;
                    double laterValue = 0;
                    List<DailyValuation> Investments = new List<DailyValuation>();

                    foreach (ISecurity security in securities)
                    {
                        if (security.Any())
                        {
                            ICurrency currency = portfolio.Currencies.Find(cur => cur.Names.Name == security.Names.Currency);
                            earlierValue += security.Value(earlierTime, currency).Value;
                            laterValue += security.Value(laterTime, currency).Value;
                            Investments.AddRange(security.InvestmentsBetween(earlierTime, laterTime, currency));
                        }
                    }

                    return FinancialFunctions.IRRTime(new DailyValuation(earlierTime, earlierValue), Investments, new DailyValuation(laterTime, laterValue));
                }
                case Totals.Sector:
                case Totals.SecuritySector:
                {
                    List<ISecurity> securities = portfolio.SectorSecurities(name.Name);
                    if (securities.Count == 0)
                    {
                        return double.NaN;
                    }
                    double earlierValue = 0;
                    double laterValue = 0;
                    List<DailyValuation> Investments = new List<DailyValuation>();

                    foreach (ISecurity security in securities)
                    {
                        if (security.Any())
                        {
                            earlierValue += security.NearestEarlierValuation(earlierTime).Value;
                            laterValue += security.NearestEarlierValuation(laterTime).Value;
                            Investments.AddRange(security.InvestmentsBetween(earlierTime, laterTime));
                        }
                    }

                    return FinancialFunctions.IRRTime(new DailyValuation(earlierTime, earlierValue), Investments, new DailyValuation(laterTime, laterValue));
                }
                case Totals.BankAccount:
                {
                    double earlierValue = 0;
                    double laterValue = 0;

                    foreach (var bankAccount in portfolio.BankAccounts)
                    {
                        ICurrency currency = portfolio.Currencies.Find(cur => cur.Names.Name == bankAccount.Names.Currency);
                        earlierValue += bankAccount.Value(earlierTime, currency).Value;
                        laterValue += bankAccount.Value(laterTime, currency).Value;
                    }

                    return FinancialFunctions.CAR(new DailyValuation(earlierTime, earlierValue), new DailyValuation(laterTime, laterValue));
                }
                case Totals.Benchmark:
                {
                    double earlierValue = 0;
                    double laterValue = 0;

                    foreach (var benchmark in portfolio.BenchMarks)
                    {
                        earlierValue += benchmark.Value(earlierTime).Value;
                        laterValue += benchmark.Value(laterTime).Value;
                    }

                    return FinancialFunctions.CAR(new DailyValuation(earlierTime, earlierValue), new DailyValuation(laterTime, laterValue));
                }
                case Totals.Currency:
                {
                    double earlierValue = 0;
                    double laterValue = 0;

                    foreach (var currency in portfolio.Currencies)
                    {
                        earlierValue += currency.Value(earlierTime).Value;
                        laterValue += currency.Value(laterTime).Value;
                    }

                    return FinancialFunctions.CAR(new DailyValuation(earlierTime, earlierValue), new DailyValuation(laterTime, laterValue));
                }
                default:
                {
                    return 0.0;
                }
            }
        }

        public static double IRR(this IPortfolio portfolio, Account accountType, TwoName names)
        {
            switch (accountType)
            {
                case Account.Security:
                {
                    if (portfolio.TryGetSecurity(names, out ISecurity desired))
                    {
                        if (desired.Any())
                        {
                            ICurrency currency = portfolio.Currency(Account.Security, desired);
                            return desired.IRR(currency);
                        }
                    }

                    return double.NaN;
                }
                case Account.BankAccount:
                case Account.Benchmark:
                case Account.Currency:
                {
                    if (portfolio.TryGetAccount(accountType, names, out IValueList desired))
                    {
                        if (desired.Any())
                        {
                            // ICurrency currency = portfolio.Currency(accountType, desired);
                            return desired.CAR(desired.FirstValue().Day, desired.LatestValue().Day);
                        }
                    }

                    return double.NaN;
                }
                default:
                {
                    return 0.0;
                }
            }
        }

        public static double IRR(this IPortfolio portfolio, Account accountType, TwoName names, DateTime earlierTime, DateTime laterTime)
        {
            switch (accountType)
            {
                case Account.Security:
                {
                    if (portfolio.TryGetSecurity(names, out ISecurity desired))
                    {
                        if (desired.Any())
                        {
                            ICurrency currency = portfolio.Currency(Account.Security, desired);
                            return desired.IRR(earlierTime, laterTime, currency);
                        }
                    }

                    return double.NaN;
                }
                case Account.BankAccount:
                case Account.Benchmark:
                case Account.Currency:
                {
                    if (portfolio.TryGetAccount(accountType, names, out IValueList desired))
                    {
                        if (desired.Any())
                        {
                            // ICurrency currency = portfolio.Currency(accountType, desired);
                            return desired.CAR(earlierTime, laterTime);
                        }
                    }

                    return double.NaN;
                }
                default:
                {
                    return 0.0;
                }
            }
        }
    }
}
