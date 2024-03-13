using Effanville.FinancialStructures.Database.Implementation;

namespace Effanville.FinancialStructures.Database
{
    /// <summary>
    /// Class for generating portfolios.
    /// </summary>
    public static class PortfolioFactory
    {
        /// <summary>
        /// Create an empty portfolio.
        /// </summary>
        public static IPortfolio GenerateEmpty() => new Portfolio();
    }
}
