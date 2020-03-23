using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

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
        /// <returns>Whether exists or not.</returns>
        public static bool CompanyExists(this IPortfolio portfolio, AccountType elementType, string company)
        {
            foreach (string comp in portfolio.Companies(elementType))
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
        /// <returns>Whether exists or not.</returns>
        public static bool Exists(this IPortfolio portfolio, AccountType elementType, TwoName name)
        {
            foreach (TwoName sec in portfolio.NameData(elementType))
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
