using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Effanville.FinancialStructures.Stocks.Statistics;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Stocks.Tests.Statistics;

[TestFixture]
public sealed class IStockStatisticTests
{
    [Test]
    public void GIVEN_StockStatistics_THEN_AllHaveValidConstructor()
    {
        var type = typeof(IStockStatistic);
        IEnumerable<Type> instances = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && p.IsClass && !p.IsAbstract)
            .ToList();
        foreach (var validStatisticType in instances)
        {
            ConstructorInfo ctor = validStatisticType.GetConstructor(new[] { typeof(StockStatisticSettings) });
            Assert.NotNull(ctor);
        }
    }
}