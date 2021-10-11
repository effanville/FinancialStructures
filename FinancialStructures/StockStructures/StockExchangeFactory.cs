using Common.Structure.Reporting;
using FinancialStructures.StockStructures.Implementation;

namespace FinancialStructures.StockStructures
{
    public static class StockExchangeFactory
    {
        public static IStockExchange CreateNew()
        {
            return new StockExchange();
        }

        public static IStockExchange Create(string filePath, IReportLogger logger)
        {
            var exchange = new StockExchange();
            exchange.LoadStockExchange(filePath, logger);
            if (!exchange.CheckValidity())
            {
                _ = logger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, "Stock input data not suitable.");
                return null;
            }
            return exchange;
        }
    }
}
