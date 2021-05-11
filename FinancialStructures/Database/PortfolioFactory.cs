using System.IO.Abstractions;
using FinancialStructures.Database.Implementation;
using StructureCommon.Reporting;

namespace FinancialStructures.Database
{
    public static class PortfolioFactory
    {
        public static IPortfolio GenerateEmpty()
        {
            return new Portfolio();
        }

        public static void FillDetailsFromFile(this IPortfolio portfolio, string filePath)
        {
        }

        public static IPortfolio CreateFromFile(IFileSystem fileSystem, string filepath, IReportLogger logger)
        {
            var portfolio = new Portfolio();
            portfolio.LoadPortfolio(filepath, fileSystem, logger);
            return portfolio;
        }
    }
}
