using System;
using System.Collections.Generic;
using Common.Structure.DataStructures;
using Common.Structure.MathLibrary.Finance;
using FinancialStructures.Database.Extensions.Values;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Rates
{
    /// <summary>
    /// Contains extension methods for calculating rates.
    /// </summary>
    public static class Rates
    {
        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalIRR(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            DateTime earlierTime = portfolio.FirstValueDate(total, name);
            DateTime laterTime = portfolio.LatestDate(total, name);
            return portfolio.TotalIRR(total, earlierTime, laterTime, name);
        }

        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalIRR(this IPortfolio portfolio, Totals accountType, DateTime earlierTime, DateTime laterTime, TwoName name = null, int numIterations = 10)
        {
            switch (accountType)
            {
                case Totals.All:
                case Totals.Security:
                {
                    return TotalSecurityIRROf(portfolio.FundsThreadSafe, portfolio, earlierTime, laterTime, numIterations);
                }
                case Totals.SecurityCompany:
                {
                    return TotalSecurityIRROf(portfolio.CompanyAccounts(Account.Security, name.Company), portfolio, earlierTime, laterTime, numIterations);
                }
                case Totals.Sector:
                case Totals.SecuritySector:
                {
                    return TotalSecurityIRROf(portfolio.SectorAccounts(Account.Security, name), portfolio, earlierTime, laterTime, numIterations);
                }
                case Totals.BankAccount:
                {
                    return TotalExchangableValueListCAROf(portfolio.BankAccountsThreadSafe, portfolio, earlierTime, laterTime);
                }
                case Totals.Asset:
                {
                    return TotalExchangableValueListCAROf(portfolio.Assets, portfolio, earlierTime, laterTime);
                }
                case Totals.Benchmark:
                {
                    return TotalValueListCAROf(portfolio.BenchMarksThreadSafe, earlierTime, laterTime);
                }
                case Totals.Currency:
                {
                    return TotalValueListCAROf(portfolio.CurrenciesThreadSafe, earlierTime, laterTime);
                }

                case Totals.AssetCompany:
                case Totals.AssetSector:
                case Totals.AssetCurrency:
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

        private static double TotalValueListCAROf(IReadOnlyList<IValueList> valueLists, DateTime earlierTime, DateTime laterTime)
        {
            decimal earlierValue = 0;
            decimal laterValue = 0;

            foreach (IValueList currency in valueLists)
            {
                earlierValue += currency.Value(earlierTime).Value;
                laterValue += currency.Value(laterTime).Value;
            }

            return FinanceFunctions.CAR(new DailyValuation(earlierTime, earlierValue), new DailyValuation(laterTime, laterValue));
        }

        private static double TotalExchangableValueListCAROf(IReadOnlyList<IValueList> valueLists, IPortfolio portfolio, DateTime earlierTime, DateTime laterTime)
        {
            decimal earlierValue = 0;
            decimal laterValue = 0;

            foreach (IExchangableValueList bankAccount in valueLists)
            {
                ICurrency currency = portfolio.Currency(bankAccount.Names.Currency);
                earlierValue += bankAccount.Value(earlierTime, currency).Value;
                laterValue += bankAccount.Value(laterTime, currency).Value;
            }

            return FinanceFunctions.CAR(new DailyValuation(earlierTime, earlierValue), new DailyValuation(laterTime, laterValue));
        }

        private static double TotalSecurityIRROf(IReadOnlyList<IValueList> securities, IPortfolio portfolio, DateTime earlierTime, DateTime laterTime, int numIterations)
        {
            if (securities.Count == 0)
            {
                return double.NaN;
            }

            decimal earlierValue = 0;
            decimal laterValue = 0;
            List<DailyValuation> investments = new List<DailyValuation>();

            foreach (ISecurity security in securities)
            {
                if (security.Any())
                {
                    ICurrency currency = portfolio.Currency(security.Names.Currency);
                    earlierValue += security.Value(earlierTime, currency).Value;
                    laterValue += security.Value(laterTime, currency).Value;
                    investments.AddRange(security.InvestmentsBetween(earlierTime, laterTime, currency));
                }
            }

            return FinanceFunctions.IRR(new DailyValuation(earlierTime, earlierValue), investments, new DailyValuation(laterTime, laterValue), numIterations);
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
                        ISecurity desired = account as ISecurity;
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
                case Account.Asset:
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
                        ISecurity desired = account as ISecurity;
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
