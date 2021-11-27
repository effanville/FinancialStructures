using System;
using System.Collections.Generic;

using Common.Structure.DataStructures;

using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.Tests.TestDatabaseConstructor;

using NUnit.Framework;

namespace FinancialStructures.Tests.FinanceStructuresTests.SecurityTests
{
    [TestFixture]
    public sealed class SecurityRatesTests
    {
        private static IEnumerable<TestCaseData> CarData()
        {
            yield return new TestCaseData(
                new Security(),
                new DateTime(2010, 1, 1),
                new DateTime(2020, 1, 1),
                double.NaN).SetName("Car-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2009, 1, 1),
                new DateTime(2020, 1, 1),
                double.NaN).SetName("Car-DefaultSecAcrossValues");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2010, 1, 1),
                new DateTime(2020, 1, 1),
                0.10760285086157584).SetName("Car-DefaultSec");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2010, 5, 1),
                new DateTime(2020, 1, 1),
                0.11145534133627866).SetName("Car-DefaultSec2");
        }

        [TestCaseSource(nameof(CarData))]
        public void CarTests(ISecurity valueList, DateTime start, DateTime end, double expectedCar)
        {
            double actualCar = valueList.CAR(start, end);
            Assert.AreEqual(expectedCar, actualCar);
        }

        private static IEnumerable<TestCaseData> IRRData()
        {
            yield return new TestCaseData(
                new Security(),
                new DateTime(2010, 1, 1),
                new DateTime(2020, 1, 1),
                double.NaN).SetName("IRR-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2009, 1, 1),
                new DateTime(2020, 1, 1),
                0.10888671875).SetName("IRR-DefaultSecAcrossValues");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2010, 1, 1),
                new DateTime(2020, 1, 1),
                0.10760285086157584).SetName("IRR-DefaultSec");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2010, 5, 1),
                new DateTime(2020, 1, 1),
                0.11145534133627866).SetName("IRR-DefaultSec2");
        }

        [TestCaseSource(nameof(IRRData))]
        public void IRRTests(ISecurity valueList, DateTime start, DateTime end, double expectedCar)
        {
            double actualCar = valueList.IRR(start, end);
            Assert.AreEqual(expectedCar, actualCar);
        }

        private static IEnumerable<TestCaseData> IRRAllTimeData()
        {
            yield return new TestCaseData(
                new Security(),
                double.NaN).SetName("IRRAllTime-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                0.10760285086157584).SetName("IRRAllTime-DefaultSec");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                0.01220703125).SetName("IRRAllTime-SecondarySec");
        }

        [TestCaseSource(nameof(IRRAllTimeData))]
        public void IRRAllTimeTests(ISecurity valueList, double expectedCar)
        {
            double actualCar = valueList.IRR();
            Assert.AreEqual(expectedCar, actualCar);
        }

        private static IEnumerable<TestCaseData> TotalInvestmentsData()
        {
            yield return new TestCaseData(
                new Security(),
                0.0m).SetName("TotalInvestments-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                200m).SetName("TotalInvestments-DefaultSec");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                23042.96m).SetName("TotalInvestments-SecondarySec");
        }

        [TestCaseSource(nameof(TotalInvestmentsData))]
        public void TotalInvestmentsTests(ISecurity valueList, decimal expectedCar)
        {
            decimal actualCar = valueList.TotalInvestment();
            Assert.AreEqual(expectedCar, actualCar);
        }

        private static IEnumerable<TestCaseData> LatestValueData()
        {
            yield return new TestCaseData(
                new Security(),
                null).SetName("LatestValue-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DailyValuation(new DateTime(2020, 1, 1), 556.05m)).SetName("LatestValue-DefaultSec");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                new DailyValuation(new DateTime(2020, 1, 1), 25528.05m)).SetName("LatestValue-SecondarySec");
        }

        [TestCaseSource(nameof(LatestValueData))]
        public void LatestValueTests(ISecurity valueList, DailyValuation expectedValue)
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
                new Security(),
                null).SetName("FirstValue-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DailyValuation(new DateTime(2010, 1, 1), 200m)).SetName("FirstValue-DefaultSec");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                new DailyValuation(new DateTime(2010, 1, 5), 2020m)).SetName("FirstValue-SecondarySec");
        }

        [TestCaseSource(nameof(FirstValueData))]
        public void FirstValueTests(ISecurity valueList, DailyValuation expectedValue)
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
                new Security(),
                new DateTime(2020, 1, 1),
                null).SetName("Value-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2010, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 200m)).SetName("Value-DefaultSecFirstValue");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2008, 1, 1),
                new DailyValuation(new DateTime(2008, 1, 1), 0m)).SetName("Value-DefaultSecBeforeFirst");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2015, 1, 1), 1618.9913964386129334582942826m)).SetName("Value-DefaultSecMiddle");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2021, 1, 1),
                new DailyValuation(new DateTime(2021, 1, 1), 556.05m)).SetName("Value-DefaultSecAfterLast");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2015, 1, 1), 19828.324114765570328901329602m)).SetName("Value-SecondarySec");
        }

        [TestCaseSource(nameof(ValueData))]
        public void ValueTests(ISecurity valueList, DateTime dateToQuery, DailyValuation expectedValue)
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
                new Security(),
                new DateTime(2020, 1, 1),
                null).SetName("ValueBefore-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2010, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 0m)).SetName("ValueBefore-DefaultSecFirstValue");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2008, 1, 1),
                new DailyValuation(new DateTime(2008, 1, 1), 0m)).SetName("ValueBefore-DefaultSecBeforeFirst");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2015, 1, 1), 2165.96m)).SetName("ValueBefore-DefaultSecMiddle");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2021, 1, 1),
                new DailyValuation(new DateTime(2021, 1, 1), 556.05m)).SetName("ValueBefore-DefaultSecAfterLast");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2015, 1, 1), 24060.96m)).SetName("ValueBefore-SecondarySec");
        }

        [TestCaseSource(nameof(ValueBeforeData))]
        public void ValueBeforeTests(ISecurity valueList, DateTime dateToQuery, DailyValuation expectedValue)
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
                new Security(),
                new DateTime(2020, 1, 1),
                null).SetName("ValueOnOrBefore-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2010, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 200m)).SetName("ValueOnOrBefore-DefaultSecFirstValue");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2008, 1, 1),
                new DailyValuation(new DateTime(2008, 1, 1), 0m)).SetName("ValueOnOrBefore-DefaultSecBeforeFirst");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2015, 1, 1), 2165.96m)).SetName("ValueOnOrBefore-DefaultSecMiddle");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DateTime(2021, 1, 1),
                new DailyValuation(new DateTime(2021, 1, 1), 556.05m)).SetName("ValueOnOrBefore-DefaultSecAfterLast");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2015, 1, 1), 24060.96m)).SetName("ValueOnOrBefore-SecondarySec");
        }

        [TestCaseSource(nameof(ValueOnOrBeforeData))]
        public void ValueOnOrBeforeTests(ISecurity valueList, DateTime dateToQuery, DailyValuation expectedValue)
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
