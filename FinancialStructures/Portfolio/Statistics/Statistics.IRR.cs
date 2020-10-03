using System;
using System.Collections.Generic;
using FinancialStructures.FinanceInterfaces;
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
        /// <param name="portfolio"></param>
        /// <param name="accountType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static double IRRTotal(this IPortfolio portfolio, Totals accountType, string name = null)
        {
            DateTime earlierTime = portfolio.FirstValueDate(accountType, name);
            DateTime laterTime = portfolio.LatestDate(accountType, name);
            return portfolio.IRRTotal(accountType, earlierTime, laterTime, name);
        }

        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="accountType"></param>
        /// <param name="earlierTime"></param>
        /// <param name="laterTime"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static double IRRTotal(this IPortfolio portfolio, Totals accountType, DateTime earlierTime, DateTime laterTime, string name = null)
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
                            ICurrency currency = portfolio.Currencies.Find(cur => cur.Name == security.Currency);
                            earlierValue += security.Value(earlierTime, currency).Value;
                            laterValue += security.Value(laterTime, currency).Value;
                            Investments.AddRange(security.InvestmentsBetween(earlierTime, laterTime, currency));
                        }
                    }

                    return FinancialFunctions.IRRTime(new DailyValuation(earlierTime, earlierValue), Investments, new DailyValuation(laterTime, laterValue));
                }
                case Totals.SecurityCompany:
                {
                    List<ISecurity> securities = portfolio.CompanySecurities(name);
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
                            ICurrency currency = portfolio.Currencies.Find(cur => cur.Name == security.Currency);
                            earlierValue += security.Value(earlierTime, currency).Value;
                            laterValue += security.Value(laterTime, currency).Value;
                            Investments.AddRange(security.InvestmentsBetween(earlierTime, laterTime, currency));
                        }
                    }

                    return FinancialFunctions.IRRTime(new DailyValuation(earlierTime, earlierValue), Investments, new DailyValuation(laterTime, laterValue));
                }
                case Totals.Sector:
                {
                    List<ISecurity> securities = portfolio.SectorSecurities(name);
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
                            return desired.IRRTime(earlierTime, laterTime, currency);
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
