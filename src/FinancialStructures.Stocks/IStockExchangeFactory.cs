using System;

namespace Effanville.FinancialStructures.Stocks;

public interface IStockExchangeFactory
{
    IStockExchange Create(StockExchangeSettings settings = null);
    IStockExchange Create(IStockExchange otherExchange, DateTime time);
    void UpdateFromBase(IStockExchange baseStockExchange, IStockExchange inheritedStockExchange, DateTime time);

    /// <summary>
    /// Instantiates a <see cref="IStockExchange"/> from a file
    /// where each line is
    /// Ticker, Company,Name,Url
    /// </summary>
    void Configure(IStockExchange stockExchange, StockExchangeSettings settings);
}
