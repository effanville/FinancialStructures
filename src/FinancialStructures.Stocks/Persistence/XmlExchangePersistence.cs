using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using Common.Structure.FileAccess;
using Common.Structure.Reporting;

using FinancialStructures.Stocks.Implementation;
using FinancialStructures.Stocks.Persistence.Xml;

using Nager.Date;

namespace FinancialStructures.Stocks.Persistence
{
    public sealed class XmlExchangePersistence : IExchangePersistence
    {
        public IStockExchange Load(PersistenceOptions options, IReportLogger reportLogger = null)
        {
            if (options is not XmlFilePersistenceOptions xmlOptions)
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(), "Options for loading from Xml file not of correct type.");
                return null;
            }

            IFileSystem fileSystem = xmlOptions.FileSystem;
            string filePath = xmlOptions.FilePath;
            if (!fileSystem.File.Exists(filePath))
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(), "Loaded Empty New StockExchange.");
                return null;
            }

            XmlStockExchange database = XmlFileAccess.ReadFromXmlFile<XmlStockExchange>(fileSystem, filePath, out string error);
            if (database != null)
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(), $"Loaded StockExchange from {filePath}.");
                var stockExchange = new StockExchange { Name = database.Name };
                if (Enum.TryParse<CountryCode>(database.CountryCode, out var code))
                {
                    stockExchange.CountryDateCode = code;
                }

                if (database.TimeZone != null)
                {
                    stockExchange.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(database.TimeZone);
                }

                if (database.Stocks != null)
                {
                    stockExchange.Stocks = new List<Stock>();
                    foreach (var xmlStock in database.Stocks)
                    {
                        var stock = new Stock { Name = xmlStock.Name };

                        foreach (var valuation in xmlStock.Valuations)
                        {
                            var stockDay = new StockDay
                            {
                                Start = valuation.Start,
                                Duration = valuation.Duration,
                                Open = valuation.Open,
                                High = valuation.High,
                                Low = valuation.Low,
                                Close = valuation.Close,
                                Volume = valuation.Volume
                            };
                            stock.Valuations.Add(stockDay);
                        }

                        stockExchange.Stocks.Add(stock);
                    }
                }

                return stockExchange;
            }

            return null;
        }

        public void Save(IStockExchange exchange, PersistenceOptions options, IReportLogger reportLogger = null)
        {
            if (options is not XmlFilePersistenceOptions xmlOptions)
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(), "Options for loading from Xml file not of correct type.");
                return;
            }

            IFileSystem fileSystem = xmlOptions.FileSystem;
            string filePath = xmlOptions.FilePath;
            if (exchange is not StockExchange stockExchange)
            {
                reportLogger?.Log(ReportType.Error, ReportLocation.Saving.ToString(), "Attempted to save a StockExchange that was not of the correct type.");
                return;
            }

            var xmlStockExchange = new XmlStockExchange
            {
                Name = stockExchange.Name,
                CountryCode = stockExchange.CountryDateCode.ToString(),
                TimeZone = stockExchange.TimeZone.Id
            };

            if (stockExchange.Stocks.Count > 0)
            {
                xmlStockExchange.Stocks = new List<XmlStock>();
            }

            foreach (var stock in stockExchange.Stocks)
            {
                var xmlStock = new XmlStock { Name = stock.Name, Valuations = new List<XmlStockCandle>() };
                foreach (var valuation in stock.Valuations)
                {
                    var xmlStockCandle = new XmlStockCandle()
                    {
                        Start = valuation.Start,
                        Duration = valuation.Duration,
                        Open = valuation.Open,
                        High = valuation.High,
                        Low = valuation.Low,
                        Close = valuation.Close,
                        Volume = valuation.Volume
                    };
                    xmlStock.Valuations.Add(xmlStockCandle);
                }

                xmlStockExchange.Stocks.Add(xmlStock);
            }

            XmlFileAccess.WriteToXmlFile(fileSystem, filePath, xmlStockExchange, out string error);
            if (error != null)
            {
                reportLogger?.Log(ReportType.Error, ReportLocation.Saving.ToString(), error);
            }

            reportLogger?.Log(ReportType.Information, ReportLocation.Saving.ToString(), $"Saved StockExchange at {filePath}");
        }
    }
}