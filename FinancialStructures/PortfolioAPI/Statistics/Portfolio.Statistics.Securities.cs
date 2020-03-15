using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
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
        public static List<SecurityStatsHolder> GenerateSecurityStatistics(this Portfolio portfolio, bool DisplayValueFunds)
        {
            if (portfolio != null)
            {
                var funds = portfolio.GetSecurities();
                var namesAndCompanies = new List<SecurityStatsHolder>();

                foreach (var security in funds)
                {
                    var latest = new SecurityStatsHolder(security.GetName(), security.GetCompany());
                    portfolio.AddSecurityStats(latest, DateTime.Today);
                    if ((DisplayValueFunds && latest.LatestVal > 0) || !DisplayValueFunds)
                    {
                        namesAndCompanies.Add(latest);
                    }
                }
                namesAndCompanies.Sort();

                var totals = new SecurityStatsHolder("Totals", "");
                portfolio.AddSecurityStats(totals, DateTime.Today);
                if ((DisplayValueFunds && totals.LatestVal > 0) || !DisplayValueFunds)
                {
                    namesAndCompanies.Add(totals);
                }
                return namesAndCompanies;
            }

            return new List<SecurityStatsHolder>();
        }

        public static SecurityStatsHolder GenerateCompanyStatistics(this Portfolio portfolio, string company)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatsHolder("Totals", company);
                portfolio.AddSecurityStats(totals, DateTime.Today);
                return totals;
            }
            return new SecurityStatsHolder();
        }

        public static SecurityStatsHolder GeneratePortfolioStatistics(this Portfolio portfolio)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatsHolder("Totals", string.Empty);
                portfolio.AddSecurityStats(totals, DateTime.Today);
                return totals;
            }

            return new SecurityStatsHolder();
        }

        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static List<SecurityStatsHolder> GenerateCompanyFundsStatistics(this Portfolio portfolio, string company)
        {
            if (portfolio != null)
            {
                var funds = portfolio.GetSecurities();
                var namesAndCompanies = new List<SecurityStatsHolder>();

                foreach (var security in funds)
                {
                    if (security.GetCompany() == company)
                    {
                        var latest = new SecurityStatsHolder(security.GetName(), security.GetCompany());
                        portfolio.AddSecurityStats(latest, DateTime.Today);
                        namesAndCompanies.Add(latest);
                    }
                }

                namesAndCompanies.Sort();
                if (namesAndCompanies.Count > 1)
                {
                    var totals = new SecurityStatsHolder("Totals", company);
                    portfolio.AddSecurityStats(totals, DateTime.Today);
                    namesAndCompanies.Add(totals);
                }

                return namesAndCompanies;
            }

            return new List<SecurityStatsHolder>();
        }

        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static SecurityStatsHolder GenerateSectorFundsStatistics(this Portfolio portfolio, List<Sector> sectors, string sectorName)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatsHolder(sectorName, "Totals");
                portfolio.AddSectorStats(totals, DateTime.Today, sectors);
                return totals;
            }

            return new SecurityStatsHolder();
        }

        /// <summary>
        /// returns the change between the most recent two valuations of the security.
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="company"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static double RecentChange(this Portfolio portfolio, string company, string name)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
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
        public static double Profit(this Portfolio portfolio, string company, string name)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
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
        public static double SecurityFraction(this Portfolio portfolio, string company, string name, DateTime date)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
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
        public static double SecurityCompanyFraction(this Portfolio portfolio, string company, string name, DateTime date)
        {
            return portfolio.SecurityFraction(company, name, date) / portfolio.CompanyFraction(company, date);
        }

        /// <summary>
        /// Returns the investments in the security.
        /// </summary>
        private static List<DayValue_Named> SecurityInvestments(this Portfolio portfolio, string company, string name)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
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
        /// If possible, returns the CAR of the security specified.
        /// </summary>
        public static double CAR(this Portfolio portfolio, string company, string name, DateTime earlierTime, DateTime laterTime)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
            {
                if (desired.Any())
                {
                    var currency = PortfolioValues.Currency(portfolio, AccountType.Security, desired);
                    return desired.CAR(earlierTime, laterTime, currency);
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// If possible, returns the IRR of the security specified.
        /// </summary>
        public static double IRR(this Portfolio portfolio, string company, string name)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
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
        public static double IRR(this Portfolio portfolio, string company, string name, DateTime earlierTime, DateTime laterTime)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
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
        public static List<DayValue_Named> AllSecuritiesInvestments(this Portfolio portfolio)
        {
            var output = new List<DayValue_Named>();
            var companies = portfolio.Companies(AccountType.Security, null);
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
