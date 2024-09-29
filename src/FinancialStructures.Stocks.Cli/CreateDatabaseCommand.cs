using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using Effanville.Common.Console.Commands;
using Effanville.Common.Console.Options;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Persistence;
using Effanville.FinancialStructures.Stocks.HistoricalRepository;
using Effanville.FinancialStructures.Stocks.Persistence;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Effanville.FinancialStructures.Stocks.Cli
{
    /// <summary>
    /// Command that controls the downloading of stock data.
    /// </summary>
    internal sealed class CreateDatabaseCommand : ICommand
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;
        private readonly IReportLogger _reportLogger;
        private readonly CommandOption<string> _dbFilePathOption;
        private readonly CommandOption<string> _indexNameOption;
        private readonly CommandOption<DateTime> _startDateOption;
        private readonly CommandOption<DateTime> _endDateOption;
        
        /// <inheritdoc/>
        public string Name => "create";

        /// <inheritdoc/>
        public IList<CommandOption> Options
        {
            get;
        } = new List<CommandOption>();

        /// <inheritdoc/>
        public IList<ICommand> SubCommands
        {
            get;
        } = new List<ICommand>();

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public CreateDatabaseCommand(IFileSystem fileSystem, ILogger<CreateDatabaseCommand> logger, IReportLogger reportLogger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
            _reportLogger = reportLogger;
            _dbFilePathOption = new CommandOption<string>(
                "filePath", 
                "FilePath to the exchange information to create db for.", 
                required: true,
                inputString => !string.IsNullOrWhiteSpace(inputString));
            Options.Add(_dbFilePathOption);
            _indexNameOption = new CommandOption<string>(
                "index",
                "The name of the index to instantiate stocks from.",
                required: true,
                inputString => !string.IsNullOrWhiteSpace(inputString)
                );
            Options.Add(_indexNameOption);
            _startDateOption = new CommandOption<DateTime>(
                "end", 
                "The start date to add data from.",
                new DateTime(2020,1,1));
            Options.Add(_startDateOption);
            _endDateOption = new CommandOption<DateTime>(
                "end", 
                "The end date to add data to.",
                DateTime.Today);
            Options.Add(_endDateOption);
        }

        /// <inheritdoc/>
        public void WriteHelp()
            => this.WriteHelp(_logger);

        public int Execute(IConfiguration config)
        {
            string fullPath = _fileSystem.Path.GetFullPath(_dbFilePathOption.Value);
            if (!_fileSystem.File.Exists(fullPath))
            {
                _logger.Log(LogLevel.Error, "File does not exist.");
                return -1;
            }

            if (_startDateOption.Value > _endDateOption.Value)
            {
                _logger.Log(LogLevel.Error, "Start date is after end date");
                return -1;
            }

            HistoricalMarketsBuilder historicalMarketsBuilder = new HistoricalMarketsBuilder()
                .WithExchangesFromFile(fullPath, _fileSystem, _reportLogger);
             historicalMarketsBuilder.WithIndexInstruments(_indexNameOption.Value, _reportLogger).Wait();
             historicalMarketsBuilder.WithInstrumentPriceData(
                _startDateOption.Value,
                _endDateOption.Value,
                _reportLogger).Wait();
            HistoricalMarkets historicalMarkets = historicalMarketsBuilder.GetInstance();

            string outputDbFilePath = _fileSystem.Path.Combine(
                _fileSystem.Path.GetDirectoryName(fullPath),
                _fileSystem.Path.GetFileNameWithoutExtension(fullPath) + ".db");
            IHistoricalMarketsPersistence persistence = new SqliteHistoricalMarketsPersistence();
            SqlitePersistenceOptions options = new SqlitePersistenceOptions(inMemory: false, outputDbFilePath, _fileSystem);
            if (persistence.Save(historicalMarkets, options, _reportLogger))
            {
                return 0;
            }

            return -1;
        }

        /// <inheritdoc/>
        public bool Validate(IConfiguration config) 
            => this.Validate(config, _logger);
    }
}