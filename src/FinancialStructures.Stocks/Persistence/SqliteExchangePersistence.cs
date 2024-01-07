using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.DataStructures;
using Common.Structure.Reporting;

using FinancialStructures.NamingStructures;
using FinancialStructures.Persistence;
using FinancialStructures.Stocks.Implementation;
using FinancialStructures.Stocks.Persistence.Database;
using FinancialStructures.Stocks.Persistence.Models;

using Nager.Date;

namespace FinancialStructures.Stocks.Persistence
{
    public sealed class SqliteExchangePersistence : IExchangePersistence
    {
        public IStockExchange Load(PersistenceOptions options, IReportLogger reportLogger = null)
        {            
            if (options is not SqlitePersistenceOptions sqliteOptions)
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(), "Options for loading from Xml file not of correct type.");
                return null;
            }

            var dbContext = new DatabaseFactory().Create(sqliteOptions.FileSystem, sqliteOptions.FilePath);

            if (dbContext == null)
            {
                return null;
            }

            var exchange = dbContext.Exchanges.First();
            var stockExchange = new StockExchange 
                { 
                    ExchangeIdentifier = exchange.ExchangeIdentifier,
                    Name = exchange.Name,
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById(exchange.TimeZone)
                };
            if (Enum.TryParse<CountryCode>(exchange.CountryCode, out var code))
            {
                stockExchange.CountryDateCode = code;
            }

            foreach (var dbStock in dbContext.Instruments)
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
                        Low= Convert.ToDecimal(price.Low),
                        Close =Convert.ToDecimal( price.Close),
                        Volume = Convert.ToDecimal(price.Volume)
                    };
                    stock.Valuations.Add(stockDay);
                }
                
                stockExchange.Stocks.Add(stock);
            }

            return stockExchange;
        }

        public void Save(IStockExchange exchange, PersistenceOptions options, IReportLogger reportLogger = null)
        {    
            if (options is not SqlitePersistenceOptions sqliteOptions)
            {
                reportLogger?.Log(ReportType.Information, ReportLocation.Loading.ToString(), "Options for loading from Xml file not of correct type.");
                return;
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
            dbBuilder.WithExchanges(new List<Exchange> { exchangeData });
            var exchangeId = exchangeData.Id;
            foreach (var stock in exchange.Stocks)
            {
                var instrument = new Instrument()
                {
                    Company = stock.Name.Company,
                    Name = stock.Name.Name,
                    Currency = stock.Name.Currency,
                    ExchangeId = exchangeId,
                    Isin = stock.Name.Isin,
                    Sedol = stock.Name.Sedol,
                    Url = stock.Name.Url,
                    Ric = stock.Name.Ric
                };

                var priceData = new List<InstrumentPriceData>();
                foreach (var valuation in stock.Valuations)
                {
                    var data = new InstrumentPriceData() 
                    {
                        DataSourceId = 1,
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

                dbBuilder.WithInstrument(instrument, null, priceData, reportLogger);
            }
        }
    }
}