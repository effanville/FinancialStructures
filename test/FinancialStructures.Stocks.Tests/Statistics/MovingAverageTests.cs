using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.Stocks.Statistics;
using Effanville.FinancialStructures.Stocks.Statistics.Implementation;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Stocks.Tests.Statistics;

[TestFixture]
public sealed class MovingAverageTests
{
    public static IEnumerable<TestCaseData> CanCalculateValues()
    {                
        yield return new TestCaseData(
            ((DateTime, decimal, decimal, decimal, decimal, decimal)[])null,
            2,
            StockDataStream.Close,
            new DateTime(2023, 1, 3, 8, 0, 0),
            double.NaN);
        
        yield return new TestCaseData(
            new (DateTime, decimal, decimal, decimal, decimal, decimal)[]{},
            2,
            StockDataStream.Close,
            new DateTime(2023, 1, 3, 8, 0, 0),
            double.NaN);
        
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 1, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 1, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            2,
            StockDataStream.Close,
            new DateTime(2023, 1, 3, 8, 0, 0),
            101.5);
        
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            1,
            StockDataStream.Close,
            new DateTime(2023, 1, 3, 8, 0, 0),
            102.0);
        
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2022, 12, 31, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 101.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            2,
            StockDataStream.Open,
            new DateTime(2023, 1, 3, 8, 0, 0),
            101);
                
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            4,
            StockDataStream.Close,
            new DateTime(2023, 1, 3, 8, 0, 0),
            double.NaN);
                        
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            42,
            StockDataStream.Close,
            new DateTime(2024, 1, 3, 8, 0, 0),
            double.NaN);
                        
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            2,
            StockDataStream.Close,
            new DateTime(2022, 1, 3, 8, 0, 0),
            double.NaN);
        
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2022, 12, 31, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            3,
            StockDataStream.Close,
            new DateTime(2023, 1, 3, 8, 0, 0),
            101);

    }

    [TestCaseSource(nameof(CanCalculateValues))]
    public void CanCalculate(
        (DateTime, decimal, decimal, decimal, decimal, decimal)[] stockValues,
        int lag,
        StockDataStream stream,
        DateTime calculationDate,
        double expectedValue)
    {
        var stock = StockTestHelpers.SetupTestStock("", stockValues);

        var settings =
            new StockStatisticSettings(nameof(PreviousNDayRelativeValue), lag, stream);
        var stat = new MovingAverage(settings);
        double value = stat.Calculate(calculationDate, stock);
        Assert.That(value, Is.EqualTo(expectedValue).Within(1e-8));
    }
}