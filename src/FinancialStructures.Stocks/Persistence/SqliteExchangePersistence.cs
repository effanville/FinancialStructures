using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.Persistence;
using Effanville.FinancialStructures.Stocks.Implementation;
using Effanville.FinancialStructures.Stocks.Persistence.Database;
using Effanville.FinancialStructures.Stocks.Persistence.Database.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nager.Date;

namespace Effanville.FinancialStructures.Stocks.Persistence
{
    public sealed class SqliteExchangePersistence : IPersistence<IStockExchange>
    {
        private readonly ILogger<SqliteExchangePersistence> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public SqliteExchangePersistence(ILogger<SqliteExchangePersistence> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

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
            if (options is not SqlitePersistenceOptions sqliteOptions)
            {
                _logger.LogInformation("Options for loading from Xml file not of correct type.");
                return false;
            }

            if (stockExchange is not StockExchange stockExchangeImpl)
            {
                return false;
            }

            StockExchangeDbContext dbContext = new DatabaseFactory(_loggerFactory).Create(sqliteOptions.FileSystem, sqliteOptions.FilePath);

            if (dbContext == null)
            {
                return false;
            }

            Exchange exchange = dbContext.Exchanges.First();
            stockExchangeImpl.ExchangeIdentifier = exchange.ExchangeIdentifier;
            stockExchangeImpl.Name = exchange.Name;
            stockExchangeImpl.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(exchange.TimeZone);
            if (Enum.TryParse<CountryCode>(exchange.CountryCode, out CountryCode code))
            {
                stockExchangeImpl.CountryDateCode = code;
            }

            stockExchangeImpl.Stocks = new List<Stock>();
            foreach (Instrument dbStock in dbContext.Instruments.Include(x => x.Exchange))
            {
                Stock stock = new Stock();
                NameData name = new NameData()
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

                IQueryable<InstrumentPriceData> prices = dbContext.InstrumentPrices.Where(price => price.InstrumentId == dbStock.Id);
                foreach (InstrumentPriceData price in prices)
                {
                    StockDay stockDay = new StockDay
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

        public bool Save(IStockExchange exchange, PersistenceOptions options)
        {
            if (options is not SqlitePersistenceOptions sqliteOptions)
            {
                _logger.LogError("Options for loading from Xml file not of correct type.");
                return false;
            }

            string directory = sqliteOptions.FileSystem.Path.GetDirectoryName(sqliteOptions.FilePath);
            sqliteOptions.FileSystem.Directory.CreateDirectory(directory);
            Database.Setup.DatabaseBuilder dbBuilder = new DatabaseFactory(_loggerFactory)
                .GetDbBuilder(sqliteOptions.FileSystem, sqliteOptions.FilePath)
                .EnsureCreated();

            dbBuilder.WithDataSources();
            Exchange exchangeData = new Exchange()
            {
                ExchangeIdentifier = exchange.ExchangeIdentifier,
                Name = exchange.Name,
                TimeZone = exchange.TimeZone.Id,
                CountryCode = exchange.CountryDateCode.ToString(),
                ExchangeOpen = TimeOnly.FromTimeSpan(exchange.ExchangeOpenInUtc(DateTime.Today).TimeOfDay),
                ExchangeClose = TimeOnly.FromTimeSpan(exchange.ExchangeCloseInUtc(DateTime.Today).TimeOfDay)
            };
            dbBuilder.WithExchanges(new List<Exchange> { exchangeData });
            int exchangeId = exchangeData.Id;
            StockExchangeDbContext instance = dbBuilder.GetInstance();
            foreach (Stock stock in exchange.Stocks)
            {
                int coreInstrumentId = 0;
                NameData lastName = stock.Name;
                IQueryable<Instrument> existingInstrumentValues = instance.Instruments.Where(x =>
                    x.Ric == lastName.Ric);
                Instrument existingInstrument = existingInstrumentValues.ToList().MaxBy(x => x.ValidFrom);
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
                Instrument instrument = new Instrument()
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

                StockFundamentalData nameValue = stock.Fundamentals;
                InstrumentData instrumentData = new InstrumentData()
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

                List<InstrumentPriceData> priceData = new List<InstrumentPriceData>();
                foreach (StockDay valuation in stock.Valuations)
                {
                    InstrumentPriceData data = new InstrumentPriceData()
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

                dbBuilder.WithInstrument(instrument, instrumentData, priceData);
            }

            return true;
        }
    }
}