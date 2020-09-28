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
        /// If possible, returns the IRR of all securities over the time period.
        /// </summary>
        public static double IRRPortfolio(this IPortfolio portfolio, DateTime earlierTime, DateTime laterTime)
        {
            if (portfolio.NumberOf(AccountType.Security) == 0)
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

        /// <summary>
        /// If possible, returns the IRR of all securities in the sector specified over the time period.
        /// </summary>
        public static double IRRSector(this IPortfolio portfolio, string sectorName, DateTime earlierTime, DateTime laterTime)
        {
            List<ISecurity> securities = portfolio.SectorSecurities(sectorName);
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

        /// <summary>
        /// If possible, returns the IRR of all securities in the company specified over the time period.
        /// </summary>
        public static double IRRCompany(this IPortfolio portfolio, string company, DateTime earlierTime, DateTime laterTime)
        {
            List<ISecurity> securities = portfolio.CompanySecurities(company);
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

        /// <summary>
        /// Gives total return of all securities in the portfolio with given company
        /// </summary>
        public static double IRRCompany(this IPortfolio portfolio, string company)
        {
            DateTime earlierTime = DateTime.Today;
            DateTime laterTime = DateTime.Today;
            List<ISecurity> securities = portfolio.CompanySecurities(company);
            if (securities.Count == 0)
            {
                return double.NaN;
            }
            foreach (ISecurity security in securities)
            {
                if (security.Any())
                {
                    DateTime first = security.FirstValue().Day;
                    DateTime last = security.LatestValue().Day;
                    if (first < earlierTime)
                    {
                        earlierTime = first;
                    }
                    if (last > laterTime)
                    {
                        laterTime = last;
                    }
                }
            }
            return portfolio.IRRCompany(company, earlierTime, laterTime);
        }

        /// <summary>
        /// If possible, returns the IRR of the security specified.
        /// </summary>
        public static double IRR(this IPortfolio portfolio, TwoName names)
        {
            if (portfolio.TryGetSecurity(names, out ISecurity desired))
            {
                if (desired.Any())
                {
                    ICurrency currency = portfolio.Currency(AccountType.Security, desired);
                    return desired.IRR(currency);
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// If possible, returns the IRR of the security specified over the time period.
        /// </summary>
        public static double IRR(this IPortfolio portfolio, TwoName names, DateTime earlierTime, DateTime laterTime)
        {
            if (portfolio.TryGetSecurity(names, out ISecurity desired))
            {
                if (desired.Any())
                {
                    ICurrency currency = portfolio.Currency(AccountType.Security, desired);
                    return desired.IRRTime(earlierTime, laterTime, currency);
                }
            }

            return double.NaN;
        }
    }
}
