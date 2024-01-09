using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;

using Common.Structure.Reporting;

using FinancialStructures.Stocks.Download;

namespace FinancialStructures.Stocks.HistoricalRepository
{
    public class HistoricalMarketsBuilder
    {
        private HistoricalMarkets _instance;

        public HistoricalMarkets GetInstance() => _instance;
        
        public HistoricalMarketsBuilder WithExchangesFromFile(string filePath, IFileSystem fileSystem,
            IReportLogger logger = null)
        {
            _instance = HistoricalMarkets.Create(filePath, fileSystem, logger);
            return this;
        }

        public HistoricalMarketsBuilder WithBaseInstance(HistoricalMarkets markets)
        {
            _instance = markets;
            return this;
        }

        public HistoricalMarketsBuilder WithExchanges(IList<HistoricalExchange> exchanges, IReportLogger logger = null)
        {
            _instance.Exchanges.AddRange(exchanges);
            return this;
        }
        
        public async Task<HistoricalMarketsBuilder> WithIndexInstruments(string indexName, IReportLogger logger = null)
        {
            string[] instruments = InstrumentDownloader.GetIndexInstruments(indexName);
            _ = StockDataParser.ConfigureInstruments(_instance, indexName, instruments, out _, logger);
            await StockDataParser.InsertInstrumentData(_instance, indexName, instruments, logger);
            return this;
        }
        
        public async Task<HistoricalMarketsBuilder> WithInstrumentPriceData(DateTime startDate, DateTime endDate,
            IReportLogger logger = null)
        {
            _ = await StockPriceDataParser.Populate(_instance, startDate, endDate, logger);
            return this;
        }

        public async Task<HistoricalMarketsBuilder> UpdateIndexInstruments(string indexName, IReportLogger logger = null)
        {
            string[] instruments = InstrumentDownloader.GetIndexInstruments(indexName);
            _ = StockDataParser.ConfigureInstruments(
                _instance,
                indexName,
                instruments,
                out var removedInstruments,
                logger);

            _ = await StockDataParser.UpdateInstrumentData(_instance, indexName, instruments, removedInstruments, logger);

            return this;
        }
    }
}