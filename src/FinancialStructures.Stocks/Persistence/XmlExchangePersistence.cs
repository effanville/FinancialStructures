using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using Effanville.Common.Structure.FileAccess;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Persistence;
using Effanville.FinancialStructures.Stocks.Implementation;
using Effanville.FinancialStructures.Stocks.Persistence.Xml;

using Nager.Date;

namespace Effanville.FinancialStructures.Stocks.Persistence
{
    public sealed class XmlExchangePersistence : IPersistence<IStockExchange>
    {
        private readonly IReportLogger _logger;

        public XmlExchangePersistence(IReportLogger logger) => _logger = logger;

        public IStockExchange Load(PersistenceOptions options)
        {
            StockExchange stockExchange = new StockExchange();
            if (!Load(stockExchange, options))
            {
                return null;
            }

            return stockExchange;
        }
        public bool Load(IStockExchange stockExchange, PersistenceOptions options)
        {
            if (options is not XmlFilePersistenceOptions xmlOptions)
            {
                _logger?.Log(
                    ReportType.Information,
                    ReportLocation.Loading.ToString(),
                    "Options for loading from Xml file not of correct type.");
                return false;
            }

            IFileSystem fileSystem = xmlOptions.FileSystem;
            string filePath = xmlOptions.FilePath;
            if (!fileSystem.File.Exists(filePath))
            {
                _logger?.Info(nameof(XmlExchangePersistence), "Loaded Empty New StockExchange.");
                return false;
            }

            if (stockExchange is not StockExchange exchange)
            {
                return false;
            }

            XmlStockExchange database = XmlFileAccess.ReadFromXmlFile<XmlStockExchange>(
                fileSystem,
                filePath,
                out _);
            if (database != null)
            {
                _logger?.Info(nameof(XmlExchangePersistence), $"Loaded StockExchange from {filePath}.");
                exchange.Name = database.Name;
                if (Enum.TryParse<CountryCode>(database.CountryCode, out CountryCode code))
                {
                    exchange.CountryDateCode = code;
                }

                if (database.TimeZone != null)
                {
                    exchange.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(database.TimeZone);
                }

                if (database.Stocks != null)
                {
                    exchange.Stocks = new List<Stock>();
                    foreach (XmlStock xmlStock in database.Stocks)
                    {
                        Stock stock = new Stock { Name = xmlStock.Name };

                        foreach (XmlStockCandle valuation in xmlStock.Valuations)
                        {
                            StockDay stockDay = new StockDay
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

                return true;
            }

            return false;
        }

        public bool Save(IStockExchange exchange, PersistenceOptions options)
        {
            if (options is not XmlFilePersistenceOptions xmlOptions)
            {
                _logger?.Info(nameof(XmlExchangePersistence), "Options for loading from Xml file not of correct type.");
                return false;
            }

            IFileSystem fileSystem = xmlOptions.FileSystem;
            string filePath = xmlOptions.FilePath;
            if (exchange is not StockExchange stockExchange)
            {
                _logger?.Error(nameof(XmlExchangePersistence), "Attempted to save a StockExchange that was not of the correct type.");
                return false;
            }

            XmlStockExchange xmlStockExchange = new XmlStockExchange
            {
                Name = stockExchange.Name,
                CountryCode = stockExchange.CountryDateCode.ToString(),
                TimeZone = stockExchange.TimeZone.Id
            };

            if (stockExchange.Stocks.Count > 0)
            {
                xmlStockExchange.Stocks = new List<XmlStock>();
            }

            foreach (Stock stock in stockExchange.Stocks)
            {
                XmlStock xmlStock = new XmlStock { Name = stock.Name, Valuations = new List<XmlStockCandle>() };
                foreach (StockDay valuation in stock.Valuations)
                {
                    XmlStockCandle xmlStockCandle = new XmlStockCandle()
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
                _logger?.Error(nameof(XmlExchangePersistence), error);
            }

            _logger?.Info(nameof(XmlExchangePersistence), $"Saved StockExchange at {filePath}");
            return true;
        }
    }
}