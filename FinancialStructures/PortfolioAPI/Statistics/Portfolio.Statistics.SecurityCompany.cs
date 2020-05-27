using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;
using StructureCommon.DataStructures;
using StructureCommon.FinanceFunctions;

namespace FinancialStructures.Database
{
    public static partial class PortfolioSecurity
    {
        public static DateTime CompanyFirstDate(this IPortfolio portfolio, string company)
        {
            DateTime output = DateTime.Today;
            foreach (ISecurity sec in portfolio.CompanySecurities(company))
            {
                if (sec.FirstValue().Day < output)
                {
                    output = sec.FirstValue().Day;
                }
            }

            return output;
        }

        public static double CompanyRecentChange(this IPortfolio portfolio, string company)
        {
            double total = 0;

            List<ISecurity> securities = portfolio.CompanySecurities(company);
            if (securities.Count == 0)
            {
                return double.NaN;
            }

            foreach (ISecurity desired in securities)
            {
                if (desired.Any())
                {
                    total += portfolio.RecentChange(desired.Names);
                }
            }

            return total;
        }

        /// <summary>
        /// Returns a named list of all investments in the company.
        /// </summary>
        public static List<DayValue_Named> CompanyInvestments(this IPortfolio portfolio, string company)
        {
            List<DayValue_Named> output = new List<DayValue_Named>();
            foreach (ISecurity sec in portfolio.CompanySecurities(company))
            {
                ICurrency currency = PortfolioValues.Currency(portfolio, AccountType.Security, sec);
                output.AddRange(sec.AllInvestmentsNamed(currency));
            }

            return output;
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
                    ICurrency currency = PortfolioValues.Currency(portfolio, AccountType.Security, security);
                    value += security.LatestValue(currency).Value - security.TotalInvestment(currency);
                }
            }

            return value;
        }

        /// <summary>
        /// The fraction of money held in the company out of the portfolio.
        /// </summary>
        public static double CompanyFraction(this IPortfolio portfolio, string company, DateTime date)
        {
            return portfolio.CompanyValue(AccountType.Security, company, date) / portfolio.TotalValue(AccountType.Security, date);
        }

        /// <summary>
        /// Gives total return of all securities in the portfolio with given company
        /// </summary>
        public static double IRRCompanyTotal(this IPortfolio portfolio, string company)
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
    }
}
