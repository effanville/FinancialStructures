using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Effanville.FinancialStructures.Stocks.Download;
using Microsoft.Extensions.Logging;

namespace Effanville.FinancialStructures.Stocks.HistoricalRepository
{
    public class HistoricalMarketsBuilder
    {
        private HistoricalMarkets _instance;

        private readonly ILogger<HistoricalMarketsBuilder> _logger;
        private readonly StockPriceDataParser _priceDataParser;
        private readonly StockDataParser _stockDataParser;
        private readonly InstrumentDownloader _instrumentDownloader;

        public HistoricalMarketsBuilder(
            IStockDownloaderFactory stockDownloaderFactory,
            StockDataParser stockDataParser,
            InstrumentDownloader instrumentDownloader,
            ILogger<HistoricalMarketsBuilder> logger,
            ILoggerFactory loggerFactory)
        {
            _priceDataParser = new StockPriceDataParser(stockDownloaderFactory, loggerFactory.CreateLogger<StockPriceDataParser>());
            _stockDataParser = stockDataParser;
            _instrumentDownloader = instrumentDownloader;
            _logger = logger;
        }

        public HistoricalMarkets GetInstance() => _instance;

        public HistoricalMarketsBuilder WithExchangesFromFile(string filePath,
            IFileSystem fileSystem)
        {
            _instance = HistoricalMarkets.Create(filePath, fileSystem);
            return this;
        }

        public HistoricalMarketsBuilder WithBaseInstance(HistoricalMarkets markets)
        {
            _instance = markets;
            return this;
        }

        public HistoricalMarketsBuilder WithExchanges(IList<HistoricalExchange> exchanges)
        {
            _instance.Exchanges.AddRange(exchanges);
            return this;
        }

        public async Task<HistoricalMarketsBuilder> WithIndexInstruments(string indexName)
        {
            string[] instruments = _instrumentDownloader.GetIndexInstruments(indexName);
            _ = _stockDataParser.ConfigureInstruments(_instance, indexName, instruments, out _);
            await _stockDataParser.InsertInstrumentData(_instance, indexName, instruments);
            return this;
        }

        public async Task<HistoricalMarketsBuilder> WithInstrumentPriceData(
            DateTime startDate,
            DateTime endDate)
        {
            _ = await _priceDataParser.Populate(_instance, startDate, endDate);
            return this;
        }

        public async Task<HistoricalMarketsBuilder> UpdateIndexInstruments(
            string indexName)
        {
            string[] instruments = _instrumentDownloader.GetIndexInstruments(indexName);
            _logger?.LogInformation($"Retrieved index instruments: {string.Join(Environment.NewLine, instruments)}");
            _ = _stockDataParser.ConfigureInstruments(
                _instance,
                indexName,
                instruments,
                out var removedInstruments);
            _logger?.LogInformation($"Configured instruments. Removed are {string.Join(Environment.NewLine, removedInstruments.Select(x => x.Name.LastOrDefault().Value.Ric))}");

            _ = await _stockDataParser.UpdateInstrumentData(_instance, indexName, instruments, removedInstruments);

            return this;
        }
    }
}