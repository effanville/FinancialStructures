using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Stocks.Download;

namespace Effanville.FinancialStructures.Stocks.HistoricalRepository
{
    public class HistoricalMarketsBuilder
    {
        private HistoricalMarkets _instance;

        public HistoricalMarkets GetInstance() => _instance;

        public HistoricalMarketsBuilder WithExchangesFromFile(string filePath,
            IFileSystem fileSystem,
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
            string[] instruments = InstrumentDownloader.GetIndexInstruments(indexName, logger);
            _ = StockDataParser.ConfigureInstruments(_instance, indexName, instruments, out _, logger);
            await StockDataParser.InsertInstrumentData(_instance, indexName, instruments, logger);
            return this;
        }

        public async Task<HistoricalMarketsBuilder> WithInstrumentPriceData(
            DateTime startDate,
            DateTime endDate,
            IReportLogger logger = null)
        {
            _ = await StockPriceDataParser.Populate(_instance, startDate, endDate, logger);
            return this;
        }

        public async Task<HistoricalMarketsBuilder> UpdateIndexInstruments(
            string indexName,
            IReportLogger logger = null)
        {
            string[] instruments = InstrumentDownloader.GetIndexInstruments(indexName, logger);
            logger?.Log(ReportSeverity.Useful, ReportType.Information, "Downloading",
                $"Retrieved index instruments: {string.Join(Environment.NewLine, instruments)}");
            _ = StockDataParser.ConfigureInstruments(
                _instance,
                indexName,
                instruments,
                out var removedInstruments,
                logger);
            logger?.Log(ReportSeverity.Useful, ReportType.Information, "Downloading",
                $"Configured instruments. Removed are {string.Join(Environment.NewLine, removedInstruments.Select(x => x.Name.LastOrDefault().Value.Ric))}");

            _ = await StockDataParser.UpdateInstrumentData(_instance, indexName, instruments, removedInstruments,
                logger);

            return this;
        }
    }
}