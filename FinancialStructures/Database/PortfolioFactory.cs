using System.IO.Abstractions;
using Common.Structure.Reporting;
using FinancialStructures.Database.Implementation;

namespace FinancialStructures.Database
{
    /// <summary>
    /// Class for generating portfolios.
    /// </summary>
    public static class PortfolioFactory
    {
        /// <summary>
        /// Create an empty portfolio.
        /// </summary>
        public static IPortfolio GenerateEmpty()
        {
            return new Portfolio();
        }

        /// <summary>
        /// Overwrites existing information and adds the information in the file to an existing portfolio.
        /// </summary>
        public static void FillDetailsFromFile(this IPortfolio portfolio, IFileSystem fileSystem, string filePath, IReportLogger logger)
        {
            portfolio.Clear();
            portfolio.LoadPortfolio(filePath, fileSystem, logger);
        }

        /// <summary>
        /// Overwrites existing information with empty defaults.
        /// </summary>
        public static void Clear(this IPortfolio portfolio, IReportLogger logger)
        {
            portfolio.Clear();
        }

        /// <summary>
        /// Creates a portfolio from the file specified.
        /// </summary>
        public static IPortfolio CreateFromFile(IFileSystem fileSystem, string filepath, IReportLogger logger)
        {
            Portfolio portfolio = new Portfolio();
            portfolio.LoadPortfolio(filepath, fileSystem, logger);
            return portfolio;
        }
    }
}
