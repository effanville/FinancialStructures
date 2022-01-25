using System;
using System.Collections.Generic;

using Common.Structure.DataStructures;

using FinancialStructures.FinanceStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;

using NUnit.Framework;

namespace FinancialStructures.Tests.FinanceStructuresTests
{
    [TestFixture]
    public sealed class AmortizableAssetValueTests
    {
        private static IEnumerable<TestCaseData> LatestValueData()
        {
            yield return new TestCaseData(
                AmortizableAssetConstructor.Empty(),
                null).SetName("LatestValue-NoEntry");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DailyValuation(new DateTime(2020, 1, 1), 180000m)).SetName("LatestValue-DefaultSec");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Secondary(),
                new DailyValuation(new DateTime(2020, 1, 1), 599.4m)).SetName("LatestValue-SecondarySec");
        }

        [TestCaseSource(nameof(LatestValueData))]
        public void LatestValueTests(IAmortisableAsset valueList, DailyValuation expectedValue)
        {
            DailyValuation actualValue = valueList.LatestValue();

            Assert.Multiple(() =>
            {
                if (actualValue != null)
                {
                    Assert.AreEqual(expectedValue.Day, actualValue.Day);
                    Assert.AreEqual(expectedValue.Value, actualValue.Value);
                }
                else
                {
                    Assert.IsNull(expectedValue);
                }
            });
        }

        private static IEnumerable<TestCaseData> FirstValueData()
        {
            yield return new TestCaseData(
                AmortizableAssetConstructor.Empty(),
                null).SetName("FirstValue-NoEntry");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DailyValuation(new DateTime(2010, 1, 1), 20000m)).SetName("FirstValue-DefaultSec");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Secondary(),
                new DailyValuation(new DateTime(2010, 1, 5), 490m)).SetName("FirstValue-SecondarySec");
        }

        [TestCaseSource(nameof(FirstValueData))]
        public void FirstValueTests(IAmortisableAsset valueList, DailyValuation expectedValue)
        {
            DailyValuation actualValue = valueList.FirstValue();

            Assert.Multiple(() =>
            {
                if (actualValue != null)
                {
                    Assert.AreEqual(expectedValue.Day, actualValue.Day);
                    Assert.AreEqual(expectedValue.Value, actualValue.Value);
                }
                else
                {
                    Assert.IsNull(expectedValue);
                }
            });
        }

        private static IEnumerable<TestCaseData> ValueData()
        {
            yield return new TestCaseData(
                AmortizableAssetConstructor.Empty(),
                new DateTime(2020, 1, 1),
                null).SetName("Value-NoEntry");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2010, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 20000m)).SetName("Value-DefaultSecFirstValue");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2008, 1, 1),
                new DailyValuation(new DateTime(2008, 1, 1), 0m)).SetName("Value-DefaultSecBeforeFirst");
            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2015, 1, 1), 155947.5164011246485473289597m)).SetName("Value-DefaultSecMiddle");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2021, 1, 1),
                new DailyValuation(new DateTime(2021, 1, 1), 180000m)).SetName("Value-DefaultSecAfterLast");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Secondary(),
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2015, 1, 1), 108.0703047706377741849858836m)).SetName("Value-SecondarySec");
        }

        [TestCaseSource(nameof(ValueData))]
        public void ValueTests(IAmortisableAsset valueList, DateTime dateToQuery, DailyValuation expectedValue)
        {
            DailyValuation actualValue = valueList.Value(dateToQuery);

            Assert.Multiple(() =>
            {
                if (actualValue != null)
                {
                    Assert.AreEqual(expectedValue.Day, actualValue.Day);
                    Assert.AreEqual(expectedValue.Value, actualValue.Value);
                }
                else
                {
                    Assert.IsNull(expectedValue);
                }
            });
        }

        private static IEnumerable<TestCaseData> ValueBeforeData()
        {
            yield return new TestCaseData(
                AmortizableAssetConstructor.Empty(),
                new DateTime(2020, 1, 1),
                null).SetName("ValueBefore-NoEntry");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2010, 1, 1),
                null).SetName("ValueBefore-DefaultSecFirstValue");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2008, 1, 1),
                null).SetName("ValueBefore-DefaultSecBeforeFirst");
            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2012, 5, 1), 113000m)).SetName("ValueBefore-DefaultSecMiddle");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2021, 1, 1),
                new DailyValuation(new DateTime(2020, 1, 1), 180000m)).SetName("ValueBefore-DefaultSecAfterLast");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Secondary(),
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2012, 5, 5), 90m)).SetName("ValueBefore-SecondarySec");
        }

        [TestCaseSource(nameof(ValueBeforeData))]
        public void ValueBeforeTests(IAmortisableAsset valueList, DateTime dateToQuery, DailyValuation expectedValue)
        {
            DailyValuation actualValue = valueList.ValueBefore(dateToQuery);

            Assert.Multiple(() =>
            {
                if (actualValue != null)
                {
                    Assert.AreEqual(expectedValue.Day, actualValue.Day);
                    Assert.AreEqual(expectedValue.Value, actualValue.Value);
                }
                else
                {
                    Assert.IsNull(expectedValue);
                }
            });
        }

        private static IEnumerable<TestCaseData> ValueOnOrBeforeData()
        {
            yield return new TestCaseData(
                AmortizableAssetConstructor.Empty(),
                new DateTime(2020, 1, 1),
                null).SetName("ValueOnOrBefore-NoEntry");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2010, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 20000m)).SetName("ValueOnOrBefore-DefaultSecFirstValue");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2008, 1, 1),
                null).SetName("ValueOnOrBefore-DefaultSecBeforeFirst");
            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2012, 5, 1), 113000m)).SetName("ValueOnOrBefore-DefaultSecMiddle");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Default(),
                new DateTime(2021, 1, 1),
                new DailyValuation(new DateTime(2020, 1, 1), 180000m)).SetName("ValueOnOrBefore-DefaultSecAfterLast");

            yield return new TestCaseData(
                AmortizableAssetConstructor.Secondary(),
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2012, 5, 5), 90m)).SetName("ValueOnOrBefore-SecondarySec");
        }

        [TestCaseSource(nameof(ValueOnOrBeforeData))]
        public void ValueOnOrBeforeTests(IAmortisableAsset valueList, DateTime dateToQuery, DailyValuation expectedValue)
        {
            DailyValuation actualValue = valueList.ValueOnOrBefore(dateToQuery);

            Assert.Multiple(() =>
            {
                if (actualValue != null)
                {
                    Assert.AreEqual(expectedValue.Day, actualValue.Day);
                    Assert.AreEqual(expectedValue.Value, actualValue.Value);
                }
                else
                {
                    Assert.IsNull(expectedValue);
                }
            });
        }
    }
}
