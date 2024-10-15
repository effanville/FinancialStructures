using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.Stocks.Statistics;
using Effanville.FinancialStructures.Stocks.Statistics.Implementation;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Stocks.Tests.Statistics;

[TestFixture]
public sealed class AdxStatTests
{
    public static IEnumerable<TestCaseData> CanCalculateValues()
    {
        yield return new TestCaseData(
            ((DateTime, decimal, decimal, decimal, decimal, decimal)[])null,
            2,
            1,
            new DateTime(2023, 1, 3, 8, 0, 0),
            double.NaN);

        yield return new TestCaseData(
            new (DateTime, decimal, decimal, decimal, decimal, decimal)[] { },
            2,
            1,
            new DateTime(2023, 1, 3, 8, 0, 0),
            double.NaN);

        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            1,
            1,
            new DateTime(2023, 1, 3, 17, 0, 0),
            100);

        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            2,
            1,
            new DateTime(2023, 1, 3, 17, 0, 0),
            double.NaN);

        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2022, 12, 31, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            1,
            1,
            new DateTime(2023, 1, 3, 17, 0, 0),
            100);
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2022, 12, 31, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.1m, 101.2m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.2m, 102.3m, 100000.0m),
                (new DateTime(2023, 1, 4, 8, 0, 0), 101.0m, 103.0m, 101.2m, 102.4m, 100000.0m),
                (new DateTime(2023, 1, 5, 8, 0, 0), 100.0m, 102.0m, 101.2m, 101.6m, 100000.0m),
                (new DateTime(2023, 1, 6, 8, 0, 0), 101.0m, 102.2m, 99.2m, 102.4m, 100000.0m),
            },
            2,
            3,
            new DateTime(2023, 1, 6, 17, 0, 0),
            100);
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2022, 12, 31, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.1m, 101.2m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.2m, 102.3m, 100000.0m),
                (new DateTime(2023, 1, 4, 8, 0, 0), 101.0m, 103.0m, 101.2m, 102.4m, 100000.0m),
                (new DateTime(2023, 1, 5, 8, 0, 0), 100.0m, 102.0m, 101.2m, 101.6m, 100000.0m),
                (new DateTime(2023, 1, 6, 8, 0, 0), 101.0m, 102.2m, 99.2m, 102.4m, 100000.0m),
                (new DateTime(2023, 1, 7, 8, 0, 0), 101.0m, 102.2m, 99.2m, 102.4m, 100000.0m),
                (new DateTime(2023, 1, 8, 8, 0, 0), 101.0m, 102.2m, 99.2m, 102.4m, 100000.0m),
                (new DateTime(2023, 1, 9, 8, 0, 0), 101.0m, 102.2m, 99.2m, 102.4m, 100000.0m),
            },
            2,
            6,
            new DateTime(2023, 1, 9, 17, 0, 0),
            44.44444444444445);
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2022, 12, 31, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.1m, 101.2m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.2m, 102.3m, 100000.0m),
                (new DateTime(2023, 1, 4, 8, 0, 0), 101.0m, 103.0m, 101.2m, 101.4m, 100000.0m),
                (new DateTime(2023, 1, 5, 8, 0, 0), 100.0m, 102.0m, 101.2m, 101.6m, 100000.0m),
                (new DateTime(2023, 1, 6, 8, 0, 0), 101.0m, 102.2m, 99.2m, 102.4m, 100000.0m),
            },
            2,
            3,
            new DateTime(2023, 1, 6, 17, 0, 0),
            100);
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2022, 12, 31, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            1,
            1,
            new DateTime(2023, 1, 3, 17, 0, 0),
            100);

        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2022, 12, 31, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.2m, 101.0m, 102.0m, 100000.0m),
            },
            0,
            1,
            new DateTime(2023, 1, 3, 17, 0, 0),
            100);

        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            1,
            1,
            new DateTime(2023, 1, 3, 17, 0, 0),
            100);
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            1,
            1,
            new DateTime(2023, 1, 3, 17, 0, 0),
            100);
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            1,
            1,
            new DateTime(2022, 1, 3, 17, 0, 0),
            double.NaN);
        yield return new TestCaseData(
            new[]
            {
                (new DateTime(2023, 1, 1, 8, 0, 0), 100.0m, 101.0m, 99.0m, 100.0m, 100000.0m),
                (new DateTime(2023, 1, 2, 8, 0, 0), 101.0m, 102.0m, 100.0m, 101.0m, 100000.0m),
                (new DateTime(2023, 1, 3, 8, 0, 0), 102.0m, 103.0m, 101.0m, 102.0m, 100000.0m),
            },
            1,
            1,
            new DateTime(2024, 1, 3, 17, 0, 0),
            double.NaN);
    }

    [TestCaseSource(nameof(CanCalculateValues))]
    public void CanCalculate(
        (DateTime, decimal, decimal, decimal, decimal, decimal)[] stockValues,
        int lag,
        int smoothingPeriod,
        DateTime calculationDate,
        double expectedValue)
    {
        var stock = StockTestHelpers.SetupTestStock("", stockValues);

        var settings = new AdxStatisticSettings(smoothingPeriod, nameof(AdxStat), lag, StockDataStream.Close);
        var stat = new AdxStat(settings);
        double value = stat.Calculate(calculationDate, stock);
        Assert.That(value, Is.EqualTo(expectedValue).Within(1e-8));
    }
}