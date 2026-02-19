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
    internal sealed class UpdateDatabaseCommand : ICommand
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;
        private readonly IReportLogger _reportLogger;
        private readonly IConfiguration _config;
        private readonly HistoricalMarketsBuilder _builder;
        private readonly IHistoricalMarketsPersistence _persistence;
        private readonly CommandOption<string> _dbFilePathOption;
        private readonly CommandOption<string> _indexNameOption;
        private readonly CommandOption<DateTime> _startDateOption;
        private readonly CommandOption<DateTime> _endDateOption;

        /// <inheritdoc/>
        public string Name => "update";

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
        public UpdateDatabaseCommand(
            IFileSystem fileSystem,
            ILogger<UpdateDatabaseCommand> logger,
            IReportLogger reportLogger,
            IConfiguration config,
            HistoricalMarketsBuilder builder,
            IHistoricalMarketsPersistence persistence)
        {
            _fileSystem = fileSystem;
            _logger = logger;
            _reportLogger = reportLogger;
            _config = config;
            _persistence = persistence;
            _builder = builder;
            _dbFilePathOption = new CommandOption<string>(
                "filePath",
                "FilePath to the stock database to add data to.",
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
                DateTime.Today.AddDays(-90));
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

        /// <inheritdoc/>
        public int Execute()
        {
            if (!_fileSystem.File.Exists(_dbFilePathOption.Value))
            {
                _logger.Log(LogLevel.Error, "File does not exist.");
                return -1;
            }

            if (_startDateOption.Value > _endDateOption.Value)
            {
                _logger.Log(LogLevel.Error, "Start date is after end date");
                return -1;
            }

            SqlitePersistenceOptions options = new SqlitePersistenceOptions(inMemory: false, _dbFilePathOption.Value, _fileSystem, "1.0.0.0");
            HistoricalMarkets database = _persistence.Load(options);

            _logger.Log(LogLevel.Information, $"Loaded database from file {_dbFilePathOption.Value}");
            _builder.WithBaseInstance(database);
            _logger.Log(LogLevel.Information, $"Updating index instruments from {_indexNameOption.Value}");
            _builder.UpdateIndexInstruments(_indexNameOption.Value, _reportLogger).Wait();
            _logger.Log(LogLevel.Information, $"Updated index instruments.");

            _logger.Log(LogLevel.Information, $"Downloading prices from {_startDateOption.Value} to {_endDateOption.Value}");
            _builder.WithInstrumentPriceData(
                _startDateOption.Value,
                _endDateOption.Value,
                _reportLogger).Wait();

            _logger.Log(LogLevel.Information, $"Completed update, saving file");
            if (_persistence.Save(_builder.GetInstance(), options))
            {
                return 0;
            }

            return -1;
        }

        /// <inheritdoc/>
        public bool Validate()
            => this.Validate(_config, _logger);
    }
}