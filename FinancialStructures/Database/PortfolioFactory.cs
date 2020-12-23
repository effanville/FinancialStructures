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

        public static IPortfolio CreateFromFile(string filepath, IReportLogger logger)
        {
            var portfolio = new Portfolio();
            portfolio.LoadPortfolio(filepath, logger);
            return portfolio;
        }
    }
}
