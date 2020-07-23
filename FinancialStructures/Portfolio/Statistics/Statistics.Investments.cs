using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// Returns the investments in the security.
        /// </summary>
        private static List<DayValue_Named> SecurityInvestments(this IPortfolio portfolio, TwoName names)
        {
            if (portfolio.TryGetSecurity(names, out ISecurity desired))
            {
                if (desired.Any())
                {
                    ICurrency currency = portfolio.Currency(AccountType.Security, desired);
                    return desired.AllInvestmentsNamed(currency);
                }
            }

            return null;
        }

        public static List<DayValue_Named> SectorInvestments(this IPortfolio portfolio, string sectorName)
        {
            List<DayValue_Named> output = new List<DayValue_Named>();
            foreach (ISecurity security in portfolio.SectorSecurities(sectorName))
            {
                output.AddRange(security.AllInvestmentsNamed());
            }

            return output;
        }

        /// <summary>
        /// Returns a named list of all investments in the company.
        /// </summary>
        public static List<DayValue_Named> CompanyInvestments(this IPortfolio portfolio, string company)
        {
            List<DayValue_Named> output = new List<DayValue_Named>();
            foreach (ISecurity sec in portfolio.CompanySecurities(company))
            {
                ICurrency currency = portfolio.Currency(AccountType.Security, sec);
                output.AddRange(sec.AllInvestmentsNamed(currency));
            }

            return output;
        }

        /// <summary>
        /// Returns a list of all investments in the portfolio securities.
        /// </summary>
        public static List<DayValue_Named> AllSecuritiesInvestments(this IPortfolio portfolio)
        {
            List<DayValue_Named> output = new List<DayValue_Named>();
            List<string> companies = portfolio.Companies(AccountType.Security);
            companies.Sort();
            foreach (string comp in companies)
            {
                output.AddRange(portfolio.CompanyInvestments(comp));
            }
            output.Sort();
            return output;
        }
    }
}
