using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;

using Common.Structure.Reporting;

using FinancialStructures.Stocks.Persistence.Models;

namespace FinancialStructures.Stocks.Persistence.Database.Setup
{
    public sealed class DatabaseBuilder
    {
        private readonly StockExchangeDbContext _context;

        public DatabaseBuilder(StockExchangeDbContext context)
        {
            _context = context;
        }

        public StockExchangeDbContext GetInstance()
            => _context;

        public DatabaseBuilder EnsureCreated()
        {
            _ = _context.Database.EnsureCreated();
            return this;
        }

        public DatabaseBuilder WithDataSources(IReportLogger logger = null)
        {
            DataSourceData.Configure(_context, logger);
            return this;
        }

        public DatabaseBuilder WithExchangesFromFile(string filePath, IFileSystem fileSystem,
            IReportLogger logger = null)
        {
            ExchangeData.Configure(_context, filePath, fileSystem, logger);
            return this;
        }

        public DatabaseBuilder WithExchanges(IList<Exchange> exchanges, IReportLogger logger = null)
        {
            _context.Exchanges.AddRange(exchanges);
            int numberChanges = _context.SaveChanges();
            return this;
        }

        public DatabaseBuilder WithInstrumentsFromFile(string stockFilePath, IFileSystem fileSystem,
            IReportLogger logger = null)
        {
            _ = InstrumentLoader.ConfigureInstruments(_context, stockFilePath, fileSystem, logger);
            return this;
        }
        
        public DatabaseBuilder WithInstrument(Instrument instrument, InstrumentData instrumentData, IList<InstrumentPriceData> instrumentPriceData, IReportLogger logger = null)
        {
            _context.Instruments.Add(instrument);
            
            _ = _context.SaveChanges();
            var instrumentId = instrument.Id;
            if(instrumentData != null)
            {
                instrumentData.InstrumentId = instrumentId;
                _context.InstrumentData.Add(instrumentData);
            }

            foreach (var data in instrumentPriceData)
            {
                data.InstrumentId = instrumentId;
            }

            _context.InstrumentPrices.AddRange(instrumentPriceData);
            int numberChanges = _context.SaveChanges();
            return this;
        }
        
        public async Task<DatabaseBuilder> WithIndexInstrumentsFromFTSE(string indexName, IReportLogger logger = null)
        {
            string[] instruments = InstrumentLoader.GetIndexInstrumentsFromFTSE(indexName);
            _ = InstrumentLoader.ConfigureInstruments(_context, instruments, out _, out _, out _, logger);
            _ = await InstrumentDataLoader.InsertInstrumentData(_context, indexName, instruments, logger);
            return this;
        }

        public async Task<DatabaseBuilder> WithInstrumentPriceData(DateTime startDate, DateTime endDate,
            IReportLogger logger = null)
        {
            await InstrumentPriceDataLoader.Populate(_context, startDate, endDate, logger);
            return this;
        }

        public async Task<DatabaseBuilder> UpdateIndexInstrumentsFromFTSE(string indexName, IReportLogger logger = null)
        {
            string[] instruments = InstrumentLoader.GetIndexInstrumentsFromFTSE(indexName);
            _ = InstrumentLoader.ConfigureInstruments(
                _context,
                instruments,
                out var newInstrumentData,
                out var existingInstrumentData,
                out var removedInstruments,
                logger);

            _ = await InstrumentDataLoader.InsertInstrumentData(_context, indexName, newInstrumentData, logger);
            _ = await InstrumentDataLoader.UpdateInstrumentData(_context, existingInstrumentData, removedInstruments,
                logger);

            return this;
        }
    }
}