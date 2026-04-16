using System.Collections.Generic;
using System.IO.Abstractions;

using Effanville.FinancialStructures.Stocks.Persistence.Database.Models;
using Microsoft.Extensions.Logging;

namespace Effanville.FinancialStructures.Stocks.Persistence.Database.Setup
{
    public sealed class DatabaseBuilder
    {
        private readonly ILogger<DatabaseBuilder> _logger;
        private readonly StockExchangeDbContext _context;

        public DatabaseBuilder(ILogger<DatabaseBuilder> logger, StockExchangeDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public StockExchangeDbContext GetInstance()
            => _context;

        public DatabaseBuilder EnsureCreated()
        {
            _ = _context.Database.EnsureCreated();
            return this;
        }

        public DatabaseBuilder WithDataSources()
        {
            int numberChanges = DataSourceData.Configure(_context);

            _logger?.LogInformation($"Added {numberChanges} into database.");
            return this;
        }

        public DatabaseBuilder WithExchangesFromFile(string filePath, IFileSystem fileSystem)
        {
            ExchangeData.Configure(_context, filePath, fileSystem, _logger);
            return this;
        }

        public DatabaseBuilder WithExchanges(IEnumerable<Exchange> exchanges)
        {
            foreach (var exchange in exchanges)
            {
                _context.Exchanges.AddIfNotExists(
                    exchange,
                    otherEntity => exchange.ExchangeIdentifier == otherEntity.ExchangeIdentifier);
            }

            _ = _context.SaveChanges();
            return this;
        }

        public DatabaseBuilder WithInstrument(
            Instrument instrument,
            InstrumentData fundamentalData,
            IEnumerable<InstrumentPriceData> priceData)
        {
            if (instrument != null)
            {
                _context.Instruments.AddIfNotExists(
                    instrument,
                    otherEntity => instrument.CoreInstrumentId == otherEntity.CoreInstrumentId
                                   && instrument.Ric == otherEntity.Ric
                                   && instrument.ValidFrom == otherEntity.ValidFrom);
            }
            _ = _context.SaveChanges();

            if (fundamentalData != null)
            {
                _context.InstrumentData.AddIfNotExists(
                    fundamentalData,
                    otherEntity => fundamentalData.InstrumentId == otherEntity.InstrumentId
                                   && fundamentalData.ValidFrom == otherEntity.ValidFrom);
            }
            _ = _context.SaveChanges();

            if (priceData != null)
            {
                foreach (InstrumentPriceData data in priceData)
                {
                    _context.InstrumentPrices.AddIfNotExists(
                        data,
                        otherEntity => data.InstrumentId == otherEntity.InstrumentId
                                       && data.StartTime == otherEntity.StartTime
                                       && data.EndTime == otherEntity.EndTime);
                }
            }

            _ = _context.SaveChanges();
            return this;
        }

        public DatabaseBuilder WithInstrumentHistory(
            IEnumerable<Instrument> instruments,
            IEnumerable<InstrumentData> fundamentalData,
            IEnumerable<InstrumentPriceData> priceData)
        {
            if (instruments != null)
            {
                foreach (var data in instruments)
                {
                    _context.Instruments.AddIfNotExists(
                        data,
                        otherEntity => data.CoreInstrumentId == otherEntity.CoreInstrumentId
                                       && data.Ric == otherEntity.Ric
                                       && data.ValidFrom == otherEntity.ValidFrom);
                }
            }

            _ = _context.SaveChanges();
            if (fundamentalData != null)
            {
                foreach (var data in fundamentalData)
                {
                    _context.InstrumentData.AddIfNotExists(
                        data,
                        otherEntity => data.InstrumentId == otherEntity.InstrumentId
                                       && data.ValidFrom == otherEntity.ValidFrom);
                }
            }

            _ = _context.SaveChanges();
            if (priceData != null)
            {
                foreach (InstrumentPriceData data in priceData)
                {
                    _context.InstrumentPrices.AddIfNotExists(
                        data,
                        otherEntity => data.InstrumentId == otherEntity.InstrumentId
                                       && data.StartTime == otherEntity.StartTime
                                       && data.EndTime == otherEntity.EndTime);
                }
            }

            _ = _context.SaveChanges();
            return this;
        }
    }
}