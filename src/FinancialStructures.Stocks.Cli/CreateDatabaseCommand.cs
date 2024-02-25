using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using Effanville.Common.Console;
using Effanville.Common.Console.Commands;
using Effanville.Common.Console.Options;
using Effanville.Common.Structure.Reporting;

using Effanville.FinancialStructures.Persistence;
using FinancialStructures.Stocks.HistoricalRepository;
using FinancialStructures.Stocks.Persistence;

namespace FinancialStructures.Stocks.Cli
{
    /// <summary>
    /// Command that controls the downloading of stock data.
    /// </summary>
    internal sealed class CreateDatabaseCommand : ICommand
    {
        private readonly IFileSystem _fileSystem;
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
        public CreateDatabaseCommand(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
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
        public void WriteHelp(IConsole console)
            => CommandExtensions.WriteHelp(this, console);

        /// <inheritdoc/>
        public int Execute(IConsole console, string[] args)
            => Execute(console, null, args);

        /// <inheritdoc/>
        public int Execute(IConsole console, IReportLogger logger, string[] args)
        {
            string fullPath = _fileSystem.Path.GetFullPath(_dbFilePathOption.Value);
            if (!_fileSystem.File.Exists(fullPath))
            {
                logger.Error(
                    $"{nameof(CreateDatabaseCommand)}.{nameof(Execute)}",
                    "File does not exist.");
                return -1;
            }

            if (_startDateOption.Value > _endDateOption.Value)
            {
                logger.Error(
                    $"{nameof(CreateDatabaseCommand)}.{nameof(Execute)}",
                    "Start date is after end date");
                return -1;
            }

            var historicalMarketsBuilder = new HistoricalMarketsBuilder()
                .WithExchangesFromFile(fullPath, _fileSystem, logger);
             historicalMarketsBuilder.WithIndexInstruments(_indexNameOption.Value, logger).Wait();
             historicalMarketsBuilder.WithInstrumentPriceData(
                _startDateOption.Value,
                _endDateOption.Value,
                logger).Wait();
            HistoricalMarkets historicalMarkets = historicalMarketsBuilder.GetInstance();

            string outputDbFilePath = _fileSystem.Path.Combine(
                _fileSystem.Path.GetDirectoryName(fullPath),
                _fileSystem.Path.GetFileNameWithoutExtension(fullPath) + ".db");
            IHistoricalMarketsPersistence persistence = new SqliteHistoricalMarketsPersistence();
            var options = new SqlitePersistenceOptions(inMemory: false, outputDbFilePath, _fileSystem);
            if (persistence.Save(historicalMarkets, options, logger))
            {
                return 0;
            }

            return -1;
        }

        /// <inheritdoc/>
        public bool Validate(IConsole console, string[] args) => Validate(console, null, args);

        /// <inheritdoc/>
        public bool Validate(IConsole console, IReportLogger logger, string[] args)
            => this.Validate(args, console, logger);
    }
}