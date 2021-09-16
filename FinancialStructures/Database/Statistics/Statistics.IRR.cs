using System;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using Common.Structure.DataStructures;
using Common.Structure.FinanceFunctions;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double IRRTotal(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            DateTime earlierTime = portfolio.FirstValueDate(total, name);
            DateTime laterTime = portfolio.LatestDate(total, name);
            return portfolio.IRRTotal(total, earlierTime, laterTime, name);
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

                    foreach (ISecurity security in portfolio.FundsThreadSafe)
                    {
                        if (security.Any())
                        {
                            ICurrency currency = portfolio.Currency(security.Names.Currency);
                            earlierValue += security.Value(earlierTime, currency).Value;
                            laterValue += security.Value(laterTime, currency).Value;
                            Investments.AddRange(security.InvestmentsBetween(earlierTime, laterTime, currency));
                        }
                    }

                    return FinancialFunctions.IRRTime(new DailyValuation(earlierTime, earlierValue), Investments, new DailyValuation(laterTime, laterValue));
                }
                case Totals.SecurityCompany:
                {
                    IReadOnlyList<IValueList> securities = portfolio.CompanyAccounts(Account.Security, name.Company);
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
                            ICurrency currency = portfolio.Currency(security.Names.Currency);
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
                    IReadOnlyList<IValueList> securities = portfolio.SectorAccounts(Account.Security, name);
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

                    foreach (IExchangableValueList bankAccount in portfolio.BankAccountsThreadSafe)
                    {
                        ICurrency currency = portfolio.Currency(bankAccount.Names.Currency);
                        earlierValue += bankAccount.Value(earlierTime, currency).Value;
                        laterValue += bankAccount.Value(laterTime, currency).Value;
                    }

                    return FinancialFunctions.CAR(new DailyValuation(earlierTime, earlierValue), new DailyValuation(laterTime, laterValue));
                }
                case Totals.Benchmark:
                {
                    double earlierValue = 0;
                    double laterValue = 0;

                    foreach (IValueList benchmark in portfolio.BenchMarksThreadSafe)
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

                    foreach (ICurrency currency in portfolio.CurrenciesThreadSafe)
                    {
                        earlierValue += currency.Value(earlierTime).Value;
                        laterValue += currency.Value(laterTime).Value;
                    }

                    return FinancialFunctions.CAR(new DailyValuation(earlierTime, earlierValue), new DailyValuation(laterTime, laterValue));
                }

                case Totals.BankAccountCompany:
                case Totals.Company:
                case Totals.BankAccountSector:
                case Totals.CurrencySector:
                case Totals.SecurityCurrency:
                case Totals.BankAccountCurrency:
                default:
                {
                    return 0.0;
                }
            }
        }

        /// <summary>
        /// Calculates the IRR for the account with specified account and name.
        /// </summary>
        public static double IRR(this IPortfolio portfolio, Account accountType, TwoName names)
        {
            switch (accountType)
            {
                case Account.Security:
                {
                    if (portfolio.TryGetAccount(accountType, names, out IValueList account))
                    {
                        var desired = account as ISecurity;
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

        /// <summary>
        /// Calculates the IRR for the account with specified account and name between the times specified.
        /// </summary>
        public static double IRR(this IPortfolio portfolio, Account accountType, TwoName names, DateTime earlierTime, DateTime laterTime)
        {
            switch (accountType)
            {
                case Account.Security:
                {
                    if (portfolio.TryGetAccount(accountType, names, out IValueList account))
                    {
                        var desired = account as ISecurity;
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
