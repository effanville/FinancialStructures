﻿using FinancialStructures.DataStructures;
using FinancialStructures.FinanceFunctionsList;
using FinancialStructures.FinanceStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.Database
{
    public static partial class PortfolioSecurity
    {
        public static bool DoesCompanyExist(this Portfolio portfolio, string company)
        {
            foreach (Security sec in portfolio.Funds)
            {
                if (sec.GetCompany() == company)
                {
                    return true;
                }
            }

            return false;
        }

        public static List<Security> CompanySecurities(this Portfolio portfolio, string company)
        {
            var securities = new List<Security>();
            foreach (var sec in portfolio.Funds)
            {
                if (sec.GetCompany() == company)
                {
                    securities.Add(sec);
                }
            }
            securities.Sort();
            return securities;
        }

        public static List<DailyValuation_Named> GetCompanyInvestments(this Portfolio portfolio, string company)
        {
            var output = new List<DailyValuation_Named>();
            foreach (var sec in portfolio.CompanySecurities(company))
            {
                var currencyName = sec.GetCurrency();
                var currency = portfolio.Currencies.Find(cur => cur.Name == currencyName);
                output.AddRange(sec.AllInvestmentsNamed(currency));
            }

            return output;
        }

        public static double CompanyValue(this Portfolio portfolio, string company, DateTime date)
        {
            var securities = portfolio.CompanySecurities(company);
            double value = 0;
            foreach (var security in securities)
            {
                if (security.Any())
                {
                    var currencyName = security.GetCurrency();
                    var currency = portfolio.Currencies.Find(cur => cur.Name == currencyName);
                    value += security.NearestEarlierValuation(date, currency).Value;
                }
            }

            return value;
        }

        public static double CompanyProfit(this Portfolio portfolio, string company)
        {
            var securities = portfolio.CompanySecurities(company);
            double value = 0;
            foreach (var security in securities)
            {
                if (security.Any())
                {
                    var currencyName = security.GetCurrency();
                    var currency = portfolio.Currencies.Find(cur => cur.Name == currencyName);
                    value += security.LatestValue(currency).Value - security.TotalInvestment(currency);
                }
            }

            return value;
        }

        public static double FundsCompanyFraction(this Portfolio portfolio, string company, DateTime date)
        {
            return portfolio.CompanyValue(company, date) / portfolio.AllSecuritiesValue(date);
        }

        /// <summary>
        /// Gives total return of all securities in the portfolio with given company
        /// </summary>
        public static double IRRCompanyTotal(this Portfolio portfolio, string company)
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
        public static double IRRCompany(this Portfolio portfolio, string company, DateTime earlierTime, DateTime laterTime)
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
                    var currencyName = security.GetCurrency();
                    var currency = portfolio.Currencies.Find(cur => cur.Name == currencyName);
                    earlierValue += security.NearestEarlierValuation(earlierTime, currency).Value;
                    laterValue += security.NearestEarlierValuation(laterTime, currency).Value;
                    Investments.AddRange(security.InvestmentsBetween(earlierTime, laterTime, currency));
                }
            }

            return FinancialFunctions.IRRTime(new DailyValuation(earlierTime, earlierValue), Investments, new DailyValuation(laterTime, laterValue));
        }
    }
}
