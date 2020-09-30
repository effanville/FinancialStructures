using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    /// <summary>
    /// Class containing static extension methods for statistics for a portfolio.
    /// </summary>
    public static partial class Statistics
    {
        /// <summary>
        /// Returns a list of all the investments in the given type.
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="accountType"></param>
        /// <param name="sectorName"></param>
        /// <returns></returns>
        public static List<DayValue_Named> TotalInvestments(this IPortfolio portfolio, Account accountType, TwoName sectorName = null)
        {
            switch (accountType)
            {
                case Account.Security:
                {
                    List<DayValue_Named> output = new List<DayValue_Named>();
                    List<string> companies = portfolio.Companies(Account.Security);
                    companies.Sort();
                    foreach (string comp in companies)
                    {
                        output.AddRange(portfolio.CompanyInvestments(comp));
                    }
                    output.Sort();
                    return output;
                }
                case Account.Sector:
                {
                    List<DayValue_Named> output = new List<DayValue_Named>();
                    foreach (ISecurity security in portfolio.SectorSecurities(sectorName.Name))
                    {
                        output.AddRange(security.AllInvestmentsNamed());
                    }

                    return output;
                }
                case Account.All:
                {
                    return portfolio.TotalInvestments(Account.Security);
                }
                default:
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the investments in the object specified
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="accountType">The type of account to look for.</param>
        /// <param name="names">The name of the account.</param>
        /// <returns></returns>
        public static List<DayValue_Named> Investments(this IPortfolio portfolio, Account accountType, TwoName names)
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
                            return desired.AllInvestmentsNamed(currency);
                        }
                    }

                    return null;
                }
                default:
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns a named list of all investments in the company.
        /// </summary>
        public static List<DayValue_Named> CompanyInvestments(this IPortfolio portfolio, string company)
        {
            List<DayValue_Named> output = new List<DayValue_Named>();
            foreach (ISecurity sec in portfolio.CompanySecurities(company))
            {
                ICurrency currency = portfolio.Currency(Account.Security, sec);
                output.AddRange(sec.AllInvestmentsNamed(currency));
            }

            return output;
        }
    }
}
