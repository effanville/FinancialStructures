﻿using FinancialStructures.DataStructures;
using FinancialStructures.FinanceFunctionsList;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.Database
{
    public static class PortfolioValues
    {
        /// <summary>
        /// Returns the earliest date held in the portfolio.
        /// </summary>
        public static DateTime FirstValueDate(this Portfolio portfolio)
        {
            var output = DateTime.Today;
            foreach (var sec in portfolio.Funds)
            {
                if (sec.Any())
                {
                    var currencyName = sec.GetCurrency();
                    var currency = portfolio.Currencies.Find(cur => cur.Name == currencyName);
                    var securityEarliest = sec.FirstValue(currency).Day;
                    if (securityEarliest < output)
                    {
                        output = securityEarliest;
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Returns a list of all investments in the portfolio securities.
        /// </summary>
        public static List<DailyValuation_Named> AllSecuritiesInvestments(this Portfolio portfolio)
        {
            var output = new List<DailyValuation_Named>();
            foreach (var comp in portfolio.GetSecuritiesCompanyNames())
            {
                output.AddRange(portfolio.GetCompanyInvestments(comp));
            }
            output.Sort();
            return output;
        }

        public static double TotalProfit(this Portfolio portfolio)
        {
            double total = 0;
            foreach (var sec in portfolio.Funds)
            {
                if (sec.Any())
                {
                    var currencyName = sec.GetCurrency();
                    var currency = portfolio.Currencies.Find(cur => cur.Name == currencyName);
                    total += portfolio.Profit(sec.GetName(), sec.GetCompany());
                }
            }

            return total;
        }

        /// <summary>
        /// If possible, returns the IRR of all securities over the time period.
        /// </summary>
        public static double IRRPortfolio(this Portfolio portfolio, DateTime earlierTime, DateTime laterTime)
        {
            if (portfolio.Funds.Count == 0)
            {
                return double.NaN;
            }
            double earlierValue = 0;
            double laterValue = 0;
            var Investments = new List<DailyValuation>();

            foreach (var security in portfolio.Funds)
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

        /// <summary>
        /// The total value of all securities on the date specified.
        /// </summary>
        public static double AllSecuritiesValue(this Portfolio portfolio, DateTime date)
        {
            double total = 0;
            foreach (var sec in portfolio.Funds)
            {
                if (sec.Any())
                {
                    var currencyName = sec.GetCurrency();
                    var currency = portfolio.Currencies.Find(cur => cur.Name == currencyName);
                    total += sec.NearestEarlierValuation(date, currency).Value;
                }
            }

            return total;
        }

        /// <summary>
        /// The total value of all bank accounts on the date specified
        /// </summary>
        public static double AllBankAccountsValue(this Portfolio portfolio, DateTime date)
        {
            double total = 0;
            foreach (var acc in portfolio.BankAccounts)
            {
                if (acc.Any())
                {
                    var currencyName = acc.GetCurrency();
                    var currency = portfolio.Currencies.Find(cur => cur.Name == currencyName);
                    total += acc.NearestEarlierValuation(date, currency).Value;
                }
            }

            return total;
        }

        /// <summary>
        /// The total value of all securities and bank accounts on the date specified
        /// </summary>
        public static double Value(this Portfolio portfolio, DateTime date)
        {
            return portfolio.AllSecuritiesValue(date) + portfolio.AllBankAccountsValue(date);
        }

        public static List<DatabaseStatistics> GenerateDatabaseStatistics(this Portfolio portfolio)
        {
            var names = new List<DatabaseStatistics>();
            foreach (var sec in portfolio.Funds)
            {
                names.Add(new DatabaseStatistics(sec.GetCompany(), sec.GetName(), sec.FirstValue().Day, sec.LatestValue().Day, sec.Count(), (sec.LatestValue().Day - sec.FirstValue().Day).Days / (365 * (double)sec.Count())));
            }
            foreach (var bankAcc in portfolio.BankAccounts)
            {
                names.Add(new DatabaseStatistics(bankAcc.GetName(), bankAcc.GetCompany(), bankAcc.FirstValue().Day, bankAcc.LatestValue().Day, bankAcc.Count(), 365 * (double)bankAcc.Count()/(bankAcc.LatestValue().Day - bankAcc.FirstValue().Day).Days ) );
            }

            return names;
        }
    }
}
