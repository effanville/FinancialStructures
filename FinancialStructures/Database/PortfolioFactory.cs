using System.IO.Abstractions;
using FinancialStructures.Database.Implementation;
using Common.Structure.Reporting;

namespace FinancialStructures.Database
{
    public static class PortfolioFactory
    {
        public static IPortfolio GenerateEmpty()
        {
            return new Portfolio();
        }

        public static void FillDetailsFromFile(this IPortfolio portfolio, IFileSystem fileSystem, string filePath, IReportLogger logger)
        {
            portfolio.LoadPortfolio(filePath, fileSystem, logger);
        }

        public static IPortfolio CreateFromFile(IFileSystem fileSystem, string filepath, IReportLogger logger)
        {
            var portfolio = new Portfolio();
            portfolio.LoadPortfolio(filepath, fileSystem, logger);
            return portfolio;
        }
    }
}
