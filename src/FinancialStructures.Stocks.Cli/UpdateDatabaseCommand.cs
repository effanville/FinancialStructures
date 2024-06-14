using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using Effanville.Common.Console;
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
        public UpdateDatabaseCommand(IFileSystem fileSystem, ILogger<UpdateDatabaseCommand> logger, IReportLogger reportLogger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
            _reportLogger = reportLogger;
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
                DateTime.Today.AddDays(-30));
            Options.Add(_startDateOption);
            _endDateOption = new CommandOption<DateTime>(
                "end",
                "The end date to add data to.",
                DateTime.Today);
            Options.Add(_endDateOption);
        }

        /// <inheritdoc/>
        public void WriteHelp(IConsole console)
            => this.WriteHelp(console, _logger);

        /// <inheritdoc/>
        public int Execute(IConsole console, IConfiguration config)
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

            IHistoricalMarketsPersistence persistence = new SqliteHistoricalMarketsPersistence();
            var options = new SqlitePersistenceOptions(inMemory: false, _dbFilePathOption.Value, _fileSystem);
            var database = persistence.Load(options, _reportLogger);
            var historicalMarketsBuilder = new HistoricalMarketsBuilder()
                .WithBaseInstance(database);
            historicalMarketsBuilder.UpdateIndexInstruments(_indexNameOption.Value, _reportLogger).Wait();
            historicalMarketsBuilder.WithInstrumentPriceData(
                _startDateOption.Value,
                _endDateOption.Value,
                _reportLogger).Wait();

            if(persistence.Save(historicalMarketsBuilder.GetInstance(), options, _reportLogger))
            {
                return 0;
            }

            return -1;
        }

        /// <inheritdoc/>
        public bool Validate(IConsole console, IConfiguration config) 
            => this.Validate(config, console, _logger);
    }
}