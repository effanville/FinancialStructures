using System;
using System.IO.Abstractions;
using System.Linq;

using Effanville.FinancialStructures.Persistence;
using Effanville.FinancialStructures.Stocks.Implementation;
using Effanville.FinancialStructures.Stocks.Persistence;
using Microsoft.Extensions.Logging;

namespace Effanville.FinancialStructures.Stocks;

/// <summary>
/// Static factory methods for creating an <see cref="IStockExchange"/>.
/// </summary>
public class StockExchangeFactory : IStockExchangeFactory
{
    private readonly ILogger<StockExchangeFactory> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IFileSystem _fileSystem;

    public StockExchangeFactory(ILogger<StockExchangeFactory> logger, ILoggerFactory loggerFactory, IFileSystem fileSystem)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _fileSystem = fileSystem;
    }

    public IStockExchange Create(StockExchangeSettings settings = null)
    {
        if (settings == null)
            return new StockExchange();

        if (string.IsNullOrEmpty(settings.FilePath))
            return new StockExchange();

        string filePath = settings.FilePath;
        IPersistence<IStockExchange> persistence = new ExchangePersistence(_loggerFactory);
        IStockExchange exchange = persistence.Load(ExchangePersistence.CreateOptions(filePath, _fileSystem));
        if (!exchange.CheckValidity())
        {
            _logger.LogError("Stock input data not suitable.");
            return null;
        }

        return exchange;
    }

    /// <summary>
    /// Create a stock exchange from another stock exchange by copying all data strictly prior to date <paramref name="time"/>.
    /// </summary>
    public IStockExchange Create(IStockExchange otherExchange, DateTime time)
    {
        StockExchange exchange = new StockExchange();
        foreach (Stock stock in otherExchange.Stocks)
        {
            exchange.Stocks.Add(stock.Copy(time));
        }

        return exchange;
    }

    /// <summary>
    /// Updates the values in the stocks in <paramref name="inheritedStockExchange"/> from data in <paramref name="baseStockExchange"/>
    /// <para/> at the time <paramref name="time"/>.
    /// </summary>
    public void UpdateFromBase(IStockExchange baseStockExchange, IStockExchange inheritedStockExchange, DateTime time)
    {
        foreach (Stock stock in baseStockExchange.Stocks)
        {
            Stock inheritedStock = inheritedStockExchange.Stocks.FirstOrDefault(thing => thing.Name.Equals(stock.Name));
            StockDay stockData = stock.GetData(time);
            if (stockData != null && inheritedStock != null)
            {
                inheritedStock.AddOrEditValue(stockData.Start, stockData.Open, stockData.High, stockData.Low, stockData.Close, stockData.Volume);
            }
        }
    }

    public void Configure(IStockExchange stockExchange, StockExchangeSettings settings)
    {
        string[] fileContents = Array.Empty<string>();
        try
        {
            fileContents = _fileSystem.File.ReadAllLines(settings.FilePath);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"Failed to read from file located at {settings.FilePath}: {ex.Message}.");
        }

        if (fileContents.Length == 0)
        {
            _logger?.LogError("Nothing in file selected, but expected stock company, name, url data.");
            return;
        }

        foreach (string line in fileContents)
        {
            string[] parameters = line.Split(',');
            if (parameters.Length != 5)
            {
                _logger?.LogError("Insufficient Data in line to add Stock");
                return;
            }

            Stock stock = new Stock(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
            stockExchange.Stocks.Add(stock);
        }

        _logger?.LogInformation($"Configured StockExchange from file {settings.FilePath}.");
    }
}
