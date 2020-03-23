using FinancialStructures.DataStructures;
using FinancialStructures.FinanceFunctionsList;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;
using System;
using System.Collections.Generic;

namespace FinancialStructures.Database
{
    public static partial class PortfolioSecurity
    {
        public static DateTime CompanyFirstDate(this IPortfolio portfolio, string company)
        {
            var output = DateTime.Today;
            foreach (var sec in portfolio.CompanySecurities(company))
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

            var securities = portfolio.CompanySecurities(company);
            if (securities.Count == 0)
            {
                return double.NaN;
            }

            foreach (var desired in securities)
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
            var output = new List<DayValue_Named>();
            foreach (var sec in portfolio.CompanySecurities(company))
            {
                var currency = PortfolioValues.Currency(portfolio, AccountType.Security, sec);
                output.AddRange(sec.AllInvestmentsNamed(currency));
            }

            return output;
        }

        /// <summary>
        /// returns the profit in the company.
        /// </summary>
        public static double CompanyProfit(this IPortfolio portfolio, string company)
        {
            var securities = portfolio.CompanySecurities(company);
            double value = 0;
            foreach (var security in securities)
            {
                if (security.Any())
                {
                    var currency = PortfolioValues.Currency(portfolio, AccountType.Security, security);
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
            var securities = portfolio.CompanySecurities(company);
            if (securities.Count == 0)
            {
                return double.NaN;
            }
            foreach (var security in securities)
            {
                if (security.Any())
                {
                    var first = security.FirstValue().Day;
                    var last = security.LatestValue().Day;
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
            var securities = portfolio.CompanySecurities(company);
            if (securities.Count == 0)
            {
                return double.NaN;
            }
            double earlierValue = 0;
            double laterValue = 0;
            var Investments = new List<DailyValuation>();

            foreach (var security in securities)
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
