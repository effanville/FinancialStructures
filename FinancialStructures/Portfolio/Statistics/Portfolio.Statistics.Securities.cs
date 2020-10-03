using System;
using System.Collections.Generic;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.StatisticStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class PortfolioStatisticGenerators
    {
        /// <summary>
        /// returns a sorted list of all funds in portfolio, ordering by company then by fund name.
        /// </summary>
        /// <returns></returns>
        public static List<SecurityStatistics> GenerateSecurityStatistics(this IPortfolio portfolio, bool DisplayValueFunds)
        {
            if (portfolio != null)
            {
                List<SecurityStatistics> namesAndCompanies = new List<SecurityStatistics>();

                foreach (ISecurity security in portfolio.Funds)
                {
                    SecurityStatistics latest = new SecurityStatistics(StatisticsType.Individual, security.Names);
                    portfolio.AddSecurityStats(latest, DateTime.Today);
                    if ((DisplayValueFunds && latest.LatestVal > 0) || !DisplayValueFunds)
                    {
                        namesAndCompanies.Add(latest);
                    }
                }
                namesAndCompanies.Sort();

                SecurityStatistics totals = new SecurityStatistics(StatisticsType.PortfolioTotal, new TwoName(string.Empty, "Totals"));
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
                SecurityStatistics totals = new SecurityStatistics(StatisticsType.CompanyTotal, new TwoName(company, "Totals"));
                portfolio.AddSecurityStats(totals, DateTime.Today);
                return totals;
            }
            return new SecurityStatistics();
        }

        public static SecurityStatistics GeneratePortfolioStatistics(this IPortfolio portfolio)
        {
            if (portfolio != null)
            {
                SecurityStatistics totals = new SecurityStatistics(StatisticsType.PortfolioTotal, new TwoName(string.Empty, "Totals"));
                portfolio.AddSecurityStats(totals, DateTime.Today);
                return totals;
            }

            return new SecurityStatistics();
        }

        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static List<SecurityStatistics> GenerateFundsStatistics(this IPortfolio portfolio)
        {
            List<SecurityStatistics> namesAndCompanies = new List<SecurityStatistics>();
            if (portfolio != null)
            {
                foreach (ISecurity security in portfolio.Funds)
                {
                    SecurityStatistics latest = new SecurityStatistics(StatisticsType.Individual, security.Names);
                    portfolio.AddSecurityStats(latest, DateTime.Today);
                    namesAndCompanies.Add(latest);
                }

                namesAndCompanies.Sort();
            }

            return namesAndCompanies;
        }

        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static List<SecurityStatistics> GenerateCompanyFundsStatistics(this IPortfolio portfolio, string company)
        {
            if (portfolio != null)
            {
                List<SecurityStatistics> namesAndCompanies = new List<SecurityStatistics>();

                foreach (ISecurity security in portfolio.Funds)
                {
                    if (security.Company == company)
                    {
                        SecurityStatistics latest = new SecurityStatistics(StatisticsType.Individual, security.Names);
                        portfolio.AddSecurityStats(latest, DateTime.Today);
                        namesAndCompanies.Add(latest);
                    }
                }

                namesAndCompanies.Sort();
                if (namesAndCompanies.Count > 1)
                {
                    SecurityStatistics totals = new SecurityStatistics(StatisticsType.CompanyTotal, new TwoName(company, "Totals"));
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
        public static List<SecurityStatistics> GenerateCompanyOverviewStatistics(this IPortfolio portfolio)
        {
            List<SecurityStatistics> namesAndCompanies = new List<SecurityStatistics>();
            if (portfolio != null)
            {
                List<string> companies = portfolio.Companies(Account.Security);
                foreach (string company in companies)
                {
                    SecurityStatistics totals = new SecurityStatistics(StatisticsType.CompanyTotal, new TwoName(company, "Totals"));
                    portfolio.AddSecurityStats(totals, DateTime.Today);
                    namesAndCompanies.Add(totals);
                }
            }

            return namesAndCompanies;
        }

        /// <summary>
        /// returns the securities under the given sector name.
        /// </summary>
        public static SectorStatistics GenerateSectorFundsStatistics(this IPortfolio portfolio, string sectorName)
        {
            if (portfolio != null)
            {
                SectorStatistics totals = new SectorStatistics(StatisticsType.SectorTotal, new TwoName("Totals", sectorName));
                portfolio.AddSectorStats(totals, DateTime.Today);
                return totals;
            }

            return new SectorStatistics();
        }
    }
}
