using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.Reporting;

using FinancialStructures.NamingStructures;
using FinancialStructures.Persistence;
using FinancialStructures.Stocks.HistoricalRepository;
using FinancialStructures.Stocks.Implementation;
using FinancialStructures.Stocks.Persistence.Database;
using FinancialStructures.Stocks.Persistence.Database.Models;

using Microsoft.EntityFrameworkCore;

using Nager.Date;

namespace FinancialStructures.Stocks.Persistence
{
    public class SqliteHistoricalMarketsPersistence : IHistoricalMarketsPersistence
    {
        public HistoricalMarkets Load(PersistenceOptions options, IReportLogger reportLogger = null)
        {
            if (options is not SqlitePersistenceOptions sqliteOptions)
            {
                reportLogger?.Log(
                    ReportType.Information,
                    ReportLocation.Loading.ToString(),
                    "Options for loading from file not of correct type.");
                return null;
            }

            var dbContext = new DatabaseFactory().Create(sqliteOptions.FileSystem, sqliteOptions.FilePath);

            if (dbContext == null)
            {
                return null;
            }

            var historicalMarkets = new HistoricalMarkets();

            var exchanges = dbContext.Exchanges;
            foreach (var exchange in exchanges)
            {
                if (!Enum.TryParse<CountryCode>(exchange.CountryCode, out var code))
                {
                    reportLogger?.Error(
                        $"{nameof(SqliteHistoricalMarketsPersistence)}.{nameof(Load)}",
                        "Country code not of correct format.");
                    continue;
                }

                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(exchange.TimeZone);
                var historicalExchange = new HistoricalExchange()
                {
                    ExchangeIdentifier = exchange.ExchangeIdentifier,
                    Name = exchange.Name,
                    TimeZone = timeZone,
                    CountryDateCode = code,
                    ExchangeOpen = exchange.ExchangeOpen,
                    ExchangeClose = exchange.ExchangeClose
                };
                historicalMarkets.Exchanges.Add(historicalExchange);
            }

            var instruments = dbContext.Instruments
                .Include(x => x.Prices)
                .Include(x => x.FundamentalData)
                .Include(x => x.Exchange);
            foreach (var instrument in instruments)
            {
                var exchange = instrument.Exchange;
                var historicalExchange =
                    historicalMarkets.Exchanges.FirstOrDefault(x =>
                        x.ExchangeIdentifier == exchange.ExchangeIdentifier);
                if (historicalExchange == null)
                {
                    reportLogger?.Error(
                        $"{nameof(SqliteHistoricalMarketsPersistence)}.{nameof(Load)}",
                        $"Could not find exchange for stock {instrument.Ric}.");
                    continue;
                }

                var name = new NameData()
                {
                    Company = instrument.Company,
                    Name = instrument.Name,
                    Currency = instrument.Currency,
                    Url = instrument.Url,
                    SectorsFlat = instrument.Sectors,
                    Ric = instrument.Ric,
                    Sedol = instrument.Sedol,
                    Exchange = instrument.Exchange?.ExchangeIdentifier
                };

                var historicalStock = new HistoricalStock();
                historicalStock.Name.Add(instrument.ValidFrom, name);

                var fundamentalData = instrument.FundamentalData;
                foreach (var data in fundamentalData)
                {
                    var fundamentals = new StockFundamentalData()
                    {
                        Index = data.Index,
                        PeRatio = data.PeRatio,
                        EPS = data.EPS,
                        Beta5YearMonth = data.Beta5YearMonth,
                        AverageVolume = data.AverageVolume,
                        ForwardDividend = data.ForwardDividend,
                        ForwardYield = data.ForwardYield,
                        MarketCap = data.MarketCap
                    };
                    historicalStock.Fundamentals.Add(data.ValidFrom, fundamentals);
                }

                var prices = instrument.Prices;
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
                    historicalStock.Valuations.Add(stockDay);
                }

                historicalExchange.Stocks.Add(historicalStock);
            }

            return historicalMarkets;
        }

        public bool Save(HistoricalMarkets historicalMarkets, PersistenceOptions options,
            IReportLogger reportLogger = null)
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

            // First add the root exchanges to the db.
            var exchanges = new List<Exchange>();
            foreach (HistoricalExchange exchange in historicalMarkets.Exchanges)
            {
                var exchangeData = new Exchange()
                {
                    ExchangeIdentifier = exchange.ExchangeIdentifier,
                    Name = exchange.Name,
                    TimeZone = exchange.TimeZone.Id,
                    CountryCode = exchange.CountryDateCode.ToString(),
                    ExchangeOpen = exchange.ExchangeOpen,
                    ExchangeClose = exchange.ExchangeClose
                };
                exchanges.Add(exchangeData);
            }

            dbBuilder.WithExchanges(exchanges, reportLogger);

            // Now add all the instrument data.
            foreach (HistoricalExchange exchange in historicalMarkets.Exchanges)
            {
                var instance = dbBuilder.GetInstance();
                Exchange dbExchange = instance.Exchanges
                    .First(ex => ex.ExchangeIdentifier == exchange.ExchangeIdentifier);
                foreach (var stock in exchange.Stocks)
                {
                    var lastName = stock.Name.Last();
                    int coreInstrumentId = 0;
                    var existingInstrument = instance.Instruments.FirstOrDefault(x =>
                        x.Ric == lastName.Value.Ric && lastName.Key == x.ValidFrom);
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

                    List<Instrument> instrumentNameData = new List<Instrument>();
                    foreach (var name in stock.Name)
                    {
                        var nameValue = name.Value;
                        var instrument = new Instrument()
                        {
                            CoreInstrumentId = coreInstrumentId,
                            Company = nameValue.Company,
                            Name = nameValue.Name,
                            Currency = nameValue.Currency,
                            ExchangeId = dbExchange.Id,
                            Isin = nameValue.Isin,
                            Sedol = nameValue.Sedol,
                            Url = nameValue.Url,
                            Ric = nameValue.Ric,
                            ValidFrom = name.Key
                        };
                        instrumentNameData.Add(instrument);
                    }

                    List<InstrumentData> instrumentData = new List<InstrumentData>();
                    foreach (var name in stock.Fundamentals)
                    {
                        var nameValue = name.Value;
                        var instrument = new InstrumentData()
                        {
                            InstrumentId = coreInstrumentId,
                            ValidFrom = name.Key,
                            Index = nameValue.Index,
                            PeRatio = nameValue.PeRatio,
                            EPS = nameValue.EPS,
                            Beta5YearMonth = nameValue.Beta5YearMonth,
                            AverageVolume = nameValue.AverageVolume,
                            ForwardDividend = nameValue.ForwardDividend,
                            ForwardYield = nameValue.ForwardYield,
                            MarketCap = nameValue.MarketCap
                        };
                        instrumentData.Add(instrument);
                    }

                    var priceData = new List<InstrumentPriceData>();
                    foreach (var valuation in stock.Valuations)
                    {
                        var data = new InstrumentPriceData()
                        {
                            DataSourceId = 1,
                            StartTime = valuation.Start,
                            EndTime = valuation.End,
                            InstrumentId = coreInstrumentId,
                            Open = Convert.ToDouble(valuation.Open),
                            High = Convert.ToDouble(valuation.High),
                            Low = Convert.ToDouble(valuation.Low),
                            Close = Convert.ToDouble(valuation.Close),
                            Volume = Convert.ToDouble(valuation.Volume),
                        };
                        priceData.Add(data);
                    }

                    dbBuilder.WithInstrumentHistory(instrumentNameData,
                        instrumentData,
                        priceData,
                        reportLogger);
                }
            }

            return true;
        }
    }
}