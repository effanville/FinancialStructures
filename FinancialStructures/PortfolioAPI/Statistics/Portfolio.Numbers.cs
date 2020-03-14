using FinancialStructures.Database;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioStatistics
    {
        /// <summary>
        /// Number of type in the database.
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="elementType">The type to search for.</param>
        /// <returns>The number of type in the database.</returns>
        public static int NumberOf(this Portfolio portfolio, PortfolioElementType elementType)
        {
            switch (elementType)
            {
                case (PortfolioElementType.Security):
                    {
                        return portfolio.Funds.Count;
                    }
                case (PortfolioElementType.Currency):
                    {
                        return portfolio.Currencies.Count;
                    }
                case (PortfolioElementType.BankAccount):
                    {
                        return portfolio.BankAccounts.Count;
                    }
                case (PortfolioElementType.Sector):
                    {
                        break;
                    }
                default:
                    break;
            }

            return 0;
        }
    }
}
