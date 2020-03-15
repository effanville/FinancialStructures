using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using System;

namespace FinancialStructures.PortfolioAPI
{
    public static class PortfolioExistence
    {
        /// <summary>
        /// Queries whether database contains item.
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="elementType">The type of item to search for.</param>
        /// <param name="company">The company of the item to find.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Whether exists or not.</returns>
        public static bool CompanyExists(this Portfolio portfolio, AccountType elementType, string company, Action<string, string, string> reportLogger)
        {
            foreach (string comp in portfolio.Companies(elementType, reportLogger))
            {
                if (comp.Equals(company))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Queries whether database contains item.
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="elementType">The type of item to search for.</param>
        /// <param name="name">The name of the item to find.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Whether exists or not.</returns>
        public static bool Exists(this Portfolio portfolio, AccountType elementType, NameData name, Action<string, string, string> reportLogger)
        {
            foreach (NameData sec in portfolio.NameData(elementType, reportLogger))
            {
                if (sec.IsEqualTo(name))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
