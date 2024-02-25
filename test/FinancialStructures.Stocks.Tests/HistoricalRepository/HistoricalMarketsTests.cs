using System;
using System.Linq;

using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.Stocks.HistoricalRepository;
using Effanville.FinancialStructures.Stocks.Implementation;

using Nager.Date;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Stocks.Tests.HistoricalRepository;

public class HistoricalMarketsTests
{
    [Test]
    public void CreateTests()
    {
    }

    [Test]
    public void CreateExchangeSnapshotTests()
    {
        var name = new NameData("TEst", "Test"){Ric = "0001.HK"};
        var fundamentalData = new StockFundamentalData
        {
            Index = "FTSE-100",
            PeRatio = 0,
            EPS = 0,
            Beta5YearMonth = 0,
            AverageVolume = 1000,
            ForwardDividend = 0,
            ForwardYield = 0,
            MarketCap = 0
        };        
        var fundamentalData2 = new StockFundamentalData
        {
            Index = "FTSE-250",
            PeRatio = 0,
            EPS = 0,
            Beta5YearMonth = 0,
            AverageVolume = 100,
            ForwardDividend = 0,
            ForwardYield = 0,
            MarketCap = 0
        };
        var historicalStock = new HistoricalStock();
        historicalStock.Name.Add(new DateTime(2023,11,1), name);
        historicalStock.Fundamentals.Add(new DateTime(2023,10, 12), fundamentalData);
        historicalStock.Fundamentals.Add(new DateTime(2024,1,1), fundamentalData2);

        var stockDay1 = new StockDay(new DateTime(2023,11,1), 1,3,2,1.5m,100);
        var stockDayNew = new StockDay(new DateTime(2023,12,15), 2,4,3,2.5m,1000);

        historicalStock.Valuations.Add(stockDay1);
        historicalStock.Valuations.Add(stockDayNew);
        var name2 = new NameData("TEst2", "Test2"){Ric = "0002.HK"};
        var historicalStock2 = new HistoricalStock();
        historicalStock2.Name.Add(new DateTime(2024,1,1), name2);    
        historicalStock2.Fundamentals.Add(new DateTime(2023,12, 12), fundamentalData);
        historicalStock2.Fundamentals.Add(new DateTime(2024,1,1), fundamentalData2);

        var historicalExchange = new HistoricalExchange()
        {
            ExchangeIdentifier = "LSE",
            Name = "London Stock Exchange",
            CountryDateCode = CountryCode.GB,
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"),
            ExchangeOpen = new TimeOnly(8, 0, 0),
            ExchangeClose = new TimeOnly(16, 30, 0)
        };
        historicalExchange.Stocks.Add(historicalStock);
        historicalExchange.Stocks.Add(historicalStock2);
        var historicalMarkets = new HistoricalMarkets();
        historicalMarkets.Exchanges.Add(historicalExchange);

        var stockExchangeInterface = historicalMarkets.CreateExchangeSnapshot(new DateTime(2023,12,1), "LSE");

        var stockExchange = stockExchangeInterface as StockExchange;
        Assert.NotNull(stockExchange);
        Assert.AreEqual(historicalExchange.ExchangeIdentifier,stockExchange.ExchangeIdentifier);
        Assert.AreEqual(historicalExchange.Name,stockExchange.Name);
        Assert.AreEqual(historicalExchange.CountryDateCode,stockExchange.CountryDateCode);
        Assert.AreEqual(historicalExchange.TimeZone,stockExchange.TimeZone);
        Assert.AreEqual(historicalExchange.ExchangeOpen,stockExchange.ExchangeOpen);
        Assert.AreEqual(historicalExchange.ExchangeClose,stockExchange.ExchangeClose);
        
        Assert.AreEqual(1, stockExchange.Stocks.Count);
        var actualStock = stockExchange.Stocks.Single();
        
        Assert.AreEqual("0001.HK", actualStock.Name.Ric);
        Assert.NotNull(actualStock.Fundamentals);
        Assert.AreEqual("FTSE-100", actualStock.Fundamentals.Index);
        
        Assert.AreEqual(1, actualStock.Valuations.Count);
        
    }
}