using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Common.Structure.FileAccess;
using Common.Structure.Reporting;
using FinancialStructures.Download.Implementation;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.StockStructures.Implementation
{
    /// <summary>
    /// Simulates a stock exchange.
    /// </summary>
    public class StockExchange : IStockExchange
    {
        /// <inheritdoc/>
        public string Name
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public List<Stock> Stocks
        {
            get;
            set;
        } = new List<Stock>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StockExchange()
        {
        }

        /// <inheritdoc/>
        public decimal GetValue(string ticker, DateTime date, StockDataStream datatype = StockDataStream.Close)
        {
            foreach (Stock stock in Stocks)
            {
                if (stock.Ticker.Equals(ticker))
                {
                    return Convert.ToDecimal(stock.Value(date, datatype));
                }
            }

            return 0.0m;
        }

        /// <inheritdoc/>
        public decimal GetValue(TwoName name, DateTime date, StockDataStream datatype = StockDataStream.Close)
        {
            foreach (Stock stock in Stocks)
            {
                if (stock.Name.IsEqualTo(name))
                {
                    return Convert.ToDecimal(stock.Value(date, datatype));
                }
            }

            return 0.0m;
        }

        /// <inheritdoc/>
        public StockDay GetCandle(TwoName name, DateTime date)
        {
            foreach (Stock stock in Stocks)
            {
                if (stock.Name.IsEqualTo(name))
                {
                    return stock.GetData(date);
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public DateTime EarliestDate()
        {
            DateTime earliest = Stocks[0].EarliestTime();

            for (int stockIndex = 1; stockIndex < Stocks.Count; stockIndex++)
            {
                DateTime stockEarliest = Stocks[stockIndex].EarliestTime();
                if (stockEarliest < earliest)
                {
                    earliest = stockEarliest;
                }
            }

            return earliest;
        }

        /// <inheritdoc/>
        public DateTime LatestEarliestDate()
        {
            DateTime earliest = Stocks[0].EarliestTime();

            for (int stockIndex = 1; stockIndex < Stocks.Count; stockIndex++)
            {
                DateTime stockEarliest = Stocks[stockIndex].EarliestTime();
                if (stockEarliest > earliest)
                {
                    earliest = stockEarliest;
                }
            }

            return earliest;
        }

        /// <inheritdoc/>
        public DateTime LastDate()
        {
            DateTime last = Stocks[0].LastTime();

            for (int stockIndex = 1; stockIndex < Stocks.Count; stockIndex++)
            {
                DateTime stockLast = Stocks[stockIndex].LastTime();
                if (stockLast < last)
                {
                    last = stockLast;
                }
            }

            return last;
        }

        /// <inheritdoc/>
        public bool CheckValidity()
        {
            if (Stocks.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public void LoadStockExchange(string filePath, IReportLogger reportLogger = null)
        {
            LoadStockExchange(filePath, new FileSystem(), reportLogger);
        }

        /// <inheritdoc/>
        public void LoadStockExchange(string filePath, IFileSystem fileSystem, IReportLogger reportLogger = null)
        {
            if (fileSystem.File.Exists(filePath))
            {
                StockExchange database = XmlFileAccess.ReadFromXmlFile<StockExchange>(fileSystem, filePath, out string error);
                if (database != null)
                {
                    _ = reportLogger?.LogUseful(ReportType.Information, ReportLocation.Loading, $"Loaded StockExchange from {filePath}.");
                    Stocks = database.Stocks;
                }
                else
                {
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.Loading, $"No StockExchange Loaded from {filePath}. Error {error}.");
                }
                return;
            }

            _ = reportLogger?.LogUseful(ReportType.Information, ReportLocation.Loading, "Loaded Empty New StockExchange.");
            Stocks = new List<Stock>();
        }

        /// <inheritdoc/>
        public void SaveStockExchange(string filePath, IReportLogger reportLogger = null)
        {
            SaveStockExchange(filePath, new FileSystem(), reportLogger);
        }

        /// <inheritdoc/>
        public void SaveStockExchange(string filePath, IFileSystem fileSystem, IReportLogger reportLogger)
        {
            XmlFileAccess.WriteToXmlFile<StockExchange>(fileSystem, filePath, this, out string error);
            if (error == null)
            {
                _ = reportLogger?.LogUseful(ReportType.Information, ReportLocation.Saving, $"Saved StockExchange at {filePath}");
            }
            else
            {
                _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.Saving, error);
            }
        }

        /// <inheritdoc/>
        public async Task Download(DateTime startDate, DateTime endDate, IReportLogger reportLogger = null)
        {
            foreach (Stock stock in Stocks)
            {
                var downloader = new YahooDownloader();
                IStock tempDataHolder = null;
                string code = YahooDownloader.GetFinancialCode(stock.Name.Url);
                if (await downloader.TryGetFullPriceHistory(code, startDate, endDate, TimeSpan.FromDays(1), value => tempDataHolder = value, reportLogger))
                {
                    stock.Valuations = tempDataHolder.Valuations;
                }
            }
        }

        /// <inheritdoc/>
        public async Task Download(IReportLogger reportLogger = null)
        {
            foreach (Stock stock in Stocks)
            {
                var downloader = new YahooDownloader();
                StockDay stockDay = null;
                string code = YahooDownloader.GetFinancialCode(stock.Name.Url);
                if (await downloader.TryGetLatestPriceData(code, value => stockDay = value, reportLogger))
                {
                    stock.AddValue(stockDay.Start, stockDay.Open, stockDay.High, stockDay.Low, stockDay.Close, stockDay.Volume);
                    stock.Sort();
                }
            }
        }

        /// <inheritdoc/>
        public void Configure(string stockFilePath, IReportLogger logger = null)
        {
            Configure(stockFilePath, new FileSystem(), logger);
        }

        /// <inheritdoc/>
        public void Configure(string stockFilePath, IFileSystem fileSystem, IReportLogger logger = null)
        {
            string[] fileContents = Array.Empty<string>();
            try
            {
                fileContents = fileSystem.File.ReadAllLines(stockFilePath);
            }
            catch (Exception ex)
            {
                _ = logger?.LogUsefulError(ReportLocation.AddingData, $"Failed to read from file located at {stockFilePath}: {ex.Message}.");
            }

            if (fileContents.Length == 0)
            {
                _ = logger?.LogUsefulError(ReportLocation.AddingData, "Nothing in file selected, but expected stock company, name, url data.");
                return;
            }

            foreach (string line in fileContents)
            {
                string[] inputs = line.Split(',');
                AddStock(inputs, logger);
            }

            _ = logger?.LogUseful(ReportType.Information, ReportLocation.AddingData, $"Configured StockExchange from file {stockFilePath}.");
        }

        private void AddStock(string[] parameters, IReportLogger logger = null)
        {
            if (parameters.Length != 5)
            {
                _ = logger?.LogUsefulError(ReportLocation.AddingData, "Insufficient Data in line to add Stock");
                return;
            }

            Stock stock = new Stock(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
            Stocks.Add(stock);
        }
    }
}
