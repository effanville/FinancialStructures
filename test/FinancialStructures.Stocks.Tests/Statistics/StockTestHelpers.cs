using System;

using Effanville.FinancialStructures.Stocks.Implementation;

namespace Effanville.FinancialStructures.Stocks.Tests.Statistics;

public static class StockTestHelpers
{
    public static IStock SetupTestStock(string ticker, (DateTime, decimal, decimal, decimal, decimal, decimal)[] values)
    {
        if (values == null)
        {
            return null;
        }

        IStock stock = new Stock(ticker, null, null, null, null);
        foreach (var value in values)
        {
            stock.AddValue(value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6);
        }

        return stock;
    }
}