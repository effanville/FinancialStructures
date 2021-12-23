using System;
using System.IO.Abstractions;
using System.Linq;
using Common.Structure.Reporting;
using FinancialStructures.StockStructures.Implementation;

namespace FinancialStructures.StockStructures
{
    /// <summary>
    /// Static factory methods for creating an <see cref="IStockExchange"/>.
    /// </summary>
    public static class StockExchangeFactory
    {
        /// <summary>
        /// Create an empty stock exchange.
        /// </summary>
        public static IStockExchange Create()
        {
            return new StockExchange();
        }

        /// <summary>
        /// Create a stock exchange by loading from file.
        /// </summary>
        public static IStockExchange Create(string filePath, IFileSystem fileSystem, IReportLogger logger)
        {
            StockExchange exchange = new StockExchange();
            exchange.LoadStockExchange(filePath, fileSystem, logger);
            if (!exchange.CheckValidity())
            {
                _ = logger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, "Stock input data not suitable.");
                return null;
            }

            return exchange;
        }

        /// <summary>
        /// Create a stock exchange from another stock exchange by copying all data strictly prior to date <paramref name="time"/>.
        /// </summary>
        public static IStockExchange Create(IStockExchange otherExchange, DateTime time)
        {
            StockExchange exchange = new StockExchange();
            foreach (Stock stock in otherExchange.Stocks)
            {
                exchange.Stocks.Add(stock.Copy(time));
            }

            return exchange;
        }

        /// <summary>
        /// Updates the values in the stocks in <paramref name="inheritedStockExchange"/> from data in <paramref name="baseStockExchange"/>
        /// <para/> at the time <paramref name="time"/>.
        /// </summary>
        public static void UpdateFromBase(IStockExchange baseStockExchange, IStockExchange inheritedStockExchange, DateTime time, bool openOnly)
        {
            foreach (Stock stock in baseStockExchange.Stocks)
            {
                Stock inheritedStock = inheritedStockExchange.Stocks.FirstOrDefault(thing => thing.Name.Equals(stock.Name));
                StockDay stockData = stock.GetData(time);
                if (stockData != null)
                {
                    if (openOnly)
                    {
                        inheritedStock.AddValue(stockData.Time, stockData.Open, 0.0, 0.0, 0.0, 0.0);
                    }
                    else
                    {
                        inheritedStock.AddOrEditValue(stockData.Time, stockData.Open, stockData.High, stockData.Low, stockData.Close, stockData.Volume);
                    }
                }
            }
        }
    }
}
