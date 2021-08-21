﻿using System.IO.Abstractions;
using FinancialStructures.Database.Implementation;
using Common.Structure.Reporting;

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
            portfolio.LoadPortfolio(filePath, fileSystem, logger);
        }

        /// <summary>
        /// Creates a portfolio from the file specified.
        /// </summary>
        public static IPortfolio CreateFromFile(IFileSystem fileSystem, string filepath, IReportLogger logger)
        {
            var portfolio = new Portfolio();
            portfolio.LoadPortfolio(filepath, fileSystem, logger);
            return portfolio;
        }
    }
}
