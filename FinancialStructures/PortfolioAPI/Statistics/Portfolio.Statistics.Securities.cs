using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.StatisticStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioStatistics
    {
        /// <summary>
        /// returns a sorted list of all funds in portfolio, ordering by company then by fund name.
        /// </summary>
        /// <returns></returns>
        public static List<SecurityStatistics> GenerateSecurityStatistics(this IPortfolio portfolio, bool DisplayValueFunds)
        {
            if (portfolio != null)
            {
                var namesAndCompanies = new List<SecurityStatistics>();

                foreach (ISecurity security in portfolio.Funds)
                {
                    var latest = new SecurityStatistics(StatisticsType.Individual, security.Names);
                    portfolio.AddSecurityStats(latest, DateTime.Today);
                    if ((DisplayValueFunds && latest.LatestVal > 0) || !DisplayValueFunds)
                    {
                        namesAndCompanies.Add(latest);
                    }
                }
                namesAndCompanies.Sort();

                var totals = new SecurityStatistics(StatisticsType.PortfolioTotal, new TwoName(string.Empty, "Totals"));
                portfolio.AddSecurityStats(totals, DateTime.Today);
                if ((DisplayValueFunds && totals.LatestVal > 0) || !DisplayValueFunds)
                {
                    namesAndCompanies.Add(totals);
                }
                return namesAndCompanies;
            }

            return new List<SecurityStatistics>();
        }

        public static SecurityStatistics GenerateCompanyStatistics(this IPortfolio portfolio, string company)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatistics(StatisticsType.CompanyTotal, new TwoName(company, "Totals"));
                portfolio.AddSecurityStats(totals, DateTime.Today);
                return totals;
            }
            return new SecurityStatistics();
        }

        public static SecurityStatistics GeneratePortfolioStatistics(this IPortfolio portfolio)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatistics(StatisticsType.PortfolioTotal, new TwoName(string.Empty, "Totals"));
                portfolio.AddSecurityStats(totals, DateTime.Today);
                return totals;
            }

            return new SecurityStatistics();
        }

        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static List<SecurityStatistics> GenerateCompanyFundsStatistics(this IPortfolio portfolio, string company)
        {
            if (portfolio != null)
            {
                var namesAndCompanies = new List<SecurityStatistics>();

                foreach (ISecurity security in portfolio.Funds)
                {
                    if (security.Company == company)
                    {
                        var latest = new SecurityStatistics(StatisticsType.Individual, security.Names);
                        portfolio.AddSecurityStats(latest, DateTime.Today);
                        namesAndCompanies.Add(latest);
                    }
                }

                namesAndCompanies.Sort();
                if (namesAndCompanies.Count > 1)
                {
                    var totals = new SecurityStatistics(StatisticsType.CompanyTotal, new TwoName(company, "Totals"));
                    portfolio.AddSecurityStats(totals, DateTime.Today);
                    namesAndCompanies.Add(totals);
                }

                return namesAndCompanies;
            }

            return new List<SecurityStatistics>();
        }

        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static SecurityStatistics GenerateSectorFundsStatistics(this IPortfolio portfolio, string sectorName)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatistics(StatisticsType.SectorTotal, new TwoName("Totals", sectorName));
                portfolio.AddSectorStats(totals, DateTime.Today);
                return totals;
            }

            return new SecurityStatistics();
        }

        /// <summary>
        /// returns the change between the most recent two valuations of the security.
        /// </summary>
        public static double RecentChange(this IPortfolio portfolio, TwoName names)
        {
            if (portfolio.TryGetSecurity(names, out ISecurity desired))
            {
                if (desired.Any())
                {
                    var currency = PortfolioValues.Currency(portfolio, AccountType.Security, desired);
                    var needed = desired.LatestValue(currency);
                    return needed.Value - desired.LastEarlierValuation(needed.Day, currency).Value;
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// Returns the profit of the company over its lifetime in the portfolio.
        /// </summary>
        public static double Profit(this IPortfolio portfolio, TwoName names)
        {
            if (portfolio.TryGetSecurity(names, out ISecurity desired))
            {
                if (desired.Any())
                {
                    var currency = PortfolioValues.Currency(portfolio, AccountType.Security, desired);
                    return desired.LatestValue(currency).Value - desired.TotalInvestment(currency);
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// Returns the fraction of investments in the security out of the portfolio.
        /// </summary>
        public static double SecurityFraction(this IPortfolio portfolio, TwoName names, DateTime date)
        {
            if (portfolio.TryGetSecurity(names, out ISecurity desired))
            {
                if (desired.Any())
                {
                    var currency = PortfolioValues.Currency(portfolio, AccountType.Security, desired);
                    return desired.Value(date, currency).Value / portfolio.TotalValue(AccountType.Security, date);
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// returns the fraction a security has out of its company.
        /// </summary>
        public static double SecurityCompanyFraction(this IPortfolio portfolio, TwoName names, DateTime date)
        {
            return portfolio.SecurityFraction(names, date) / portfolio.CompanyFraction(names.Company, date);
        }

        /// <summary>
        /// Returns the investments in the security.
        /// </summary>
        private static List<DayValue_Named> SecurityInvestments(this IPortfolio portfolio, TwoName names)
        {
            if (portfolio.TryGetSecurity(names, out ISecurity desired))
            {
                if (desired.Any())
                {
                    var currency = PortfolioValues.Currency(portfolio, AccountType.Security, desired);
                    return desired.AllInvestmentsNamed(currency);
                }
            }

            return null;
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
                    var currency = PortfolioValues.Currency(portfolio, AccountType.Security, desired);
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
                    var currency = PortfolioValues.Currency(portfolio, AccountType.Security, desired);
                    return desired.IRRTime(earlierTime, laterTime, currency);
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// Returns a list of all investments in the portfolio securities.
        /// </summary>
        public static List<DayValue_Named> AllSecuritiesInvestments(this IPortfolio portfolio)
        {
            var output = new List<DayValue_Named>();
            var companies = portfolio.Companies(AccountType.Security);
            companies.Sort();
            foreach (var comp in companies)
            {
                output.AddRange(portfolio.CompanyInvestments(comp));
            }
            output.Sort();
            return output;
        }
    }
}
