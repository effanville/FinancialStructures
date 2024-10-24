using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.Persistence;
using Effanville.FinancialStructures.Stocks.Implementation;
using Effanville.FinancialStructures.Stocks.Persistence.Database;
using Effanville.FinancialStructures.Stocks.Persistence.Database.Models;

using Microsoft.EntityFrameworkCore;

using Nager.Date;

namespace Effanville.FinancialStructures.Stocks.Persistence
{
    public sealed class SqliteExchangePersistence : IPersistence<IStockExchange>
    {        
        public IStockExchange Load(PersistenceOptions options, IReportLogger reportLogger = null)
        {
            StockExchange stockExchange = new StockExchange();
            if (!Load(stockExchange, options, reportLogger))
            {
                return null;
            }

            return stockExchange;
        }
        
        public bool Load(IStockExchange stockExchange, PersistenceOptions options, IReportLogger reportLogger = null)
        {
            if (options is not SqlitePersistenceOptions sqliteOptions)
            {
                reportLogger?.Log(
                    ReportType.Information,
                    ReportLocation.Loading.ToString(),
                    "Options for loading from Xml file not of correct type.");
                return false;
            }

            if (stockExchange is not StockExchange stockExchangeImpl)
            {
                return false;
            }

            var dbContext = new DatabaseFactory().Create(sqliteOptions.FileSystem, sqliteOptions.FilePath);

            if (dbContext == null)
            {
                return false;
            }

            var exchange = dbContext.Exchanges.First();
            stockExchangeImpl.ExchangeIdentifier = exchange.ExchangeIdentifier;
            stockExchangeImpl.Name = exchange.Name;
            stockExchangeImpl.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(exchange.TimeZone);
            if (Enum.TryParse<CountryCode>(exchange.CountryCode, out var code))
            {
                stockExchangeImpl.CountryDateCode = code;
            }

            stockExchangeImpl.Stocks = new List<Stock>();
            foreach (var dbStock in dbContext.Instruments.Include(x => x.Exchange))
            {
                var stock = new Stock();
                var name = new NameData()
                {
                    Company = dbStock.Company,
                    Name = dbStock.Name,
                    Currency = dbStock.Company,
                    Url = dbStock.Url,
                    SectorsFlat = dbStock.Sectors,
                    Ric = dbStock.Ric,
                    Sedol = dbStock.Sedol,
                    Exchange = dbStock.Exchange?.ExchangeIdentifier
                };
                stock.Name = name;

                var prices = dbContext.InstrumentPrices.Where(price => price.InstrumentId == dbStock.Id);
                foreach (var price in prices)
                {
                    var stockDay = new StockDay
                    {
                        Start = price.StartTime,
                        Duration = price.EndTime - price.StartTime,
                        Open = Convert.ToDecimal(price.Open),
                        High = Convert.ToDecimal(price.High),
                        Low = Convert.ToDecimal(price.Low),
                        Close = Convert.ToDecimal(price.Close),
                        Volume = Convert.ToDecimal(price.Volume)
                    };
                    stock.Valuations.Add(stockDay);
                }

                stockExchangeImpl.Stocks.Add(stock);
            }

            return true;
        }

        public bool Save(IStockExchange exchange, PersistenceOptions options, IReportLogger reportLogger = null)
        {
            if (options is not SqlitePersistenceOptions sqliteOptions)
            {
                reportLogger?.Log(
                    ReportType.Information,
                    ReportLocation.Loading.ToString(),
                    "Options for loading from Xml file not of correct type.");
                return false;
            }

            string directory = sqliteOptions.FileSystem.Path.GetDirectoryName(sqliteOptions.FilePath);
            sqliteOptions.FileSystem.Directory.CreateDirectory(directory);
            var dbBuilder = new DatabaseFactory()
                .GetDbBuilder(sqliteOptions.FileSystem, sqliteOptions.FilePath)
                .EnsureCreated();

            dbBuilder.WithDataSources();
            var exchangeData = new Exchange()
            {
                ExchangeIdentifier = exchange.ExchangeIdentifier,
                Name = exchange.Name,
                TimeZone = exchange.TimeZone.Id,
                CountryCode = exchange.CountryDateCode.ToString(),
                ExchangeOpen = TimeOnly.FromTimeSpan(exchange.ExchangeOpenInUtc(DateTime.Today).TimeOfDay),
                ExchangeClose = TimeOnly.FromTimeSpan(exchange.ExchangeCloseInUtc(DateTime.Today).TimeOfDay)
            };
            dbBuilder.WithExchanges(new List<Exchange> { exchangeData }, reportLogger);
            int exchangeId = exchangeData.Id;
            var instance = dbBuilder.GetInstance();
            foreach (var stock in exchange.Stocks)
            {
                int coreInstrumentId = 0;
                var lastName = stock.Name;
                var existingInstrumentValues = instance.Instruments.Where(x =>
                    x.Ric == lastName.Ric);
                var existingInstrument = existingInstrumentValues.ToList().MaxBy(x => x.ValidFrom);
                if (existingInstrument != null)
                {
                    coreInstrumentId = existingInstrument.CoreInstrumentId;
                }
                else
                {
                    coreInstrumentId = instance.Instruments.Any()
                        ? instance.Instruments.Max(x => x.CoreInstrumentId) + 1
                        : 1;
                }
                var instrument = new Instrument()
                {
                    Company = stock.Name.Company,
                    Name = stock.Name.Name,
                    CoreInstrumentId = coreInstrumentId,
                    Currency = stock.Name.Currency,
                    ExchangeId = exchangeId,
                    Isin = stock.Name.Isin,
                    Sedol = stock.Name.Sedol,
                    Url = stock.Name.Url,
                    Ric = stock.Name.Ric
                };

                var nameValue = stock.Fundamentals;
                var instrumentData = new InstrumentData()
                {
                    ValidFrom = DateTime.Now,
                    InstrumentId = coreInstrumentId,
                    Index = nameValue.Index,
                    PeRatio = nameValue.PeRatio,
                    EPS = nameValue.EPS,
                    Beta5YearMonth = nameValue.Beta5YearMonth,
                    AverageVolume = nameValue.AverageVolume,
                    ForwardDividend = nameValue.ForwardDividend,
                    ForwardYield = nameValue.ForwardYield,
                    MarketCap = nameValue.MarketCap
                };

                var priceData = new List<InstrumentPriceData>();
                foreach (var valuation in stock.Valuations)
                {
                    var data = new InstrumentPriceData()
                    {
                        DataSourceId = 1,
                        InstrumentId = coreInstrumentId,
                        StartTime = valuation.Start,
                        EndTime = valuation.End,
                        Open = Convert.ToDouble(valuation.Open),
                        High = Convert.ToDouble(valuation.High),
                        Low = Convert.ToDouble(valuation.Low),
                        Close = Convert.ToDouble(valuation.Close),
                        Volume = Convert.ToDouble(valuation.Volume),
                    };
                    priceData.Add(data);
                }

                dbBuilder.WithInstrument(instrument, instrumentData, priceData, reportLogger);
            }

            return true;
        }
    }
}