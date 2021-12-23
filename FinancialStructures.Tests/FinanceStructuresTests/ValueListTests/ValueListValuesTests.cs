using System;
using System.Collections.Generic;

using Common.Structure.DataStructures;

using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace FinancialStructures.Tests.FinanceStructuresTests.ValueListTests
{
    [TestFixture]
    public sealed class ValueListValuesTests
    {
        private static IEnumerable<TestCaseData> ValuesData()
        {
            yield return new TestCaseData(
                new ValueList(),
                new DateTime(2010, 1, 1),
                null).SetName($"{nameof(ValueTests)}-NoEntry");

            var values = new TimeList();
            values.SetData(new DateTime(2010, 1, 1), 4.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 4.2m))
                .SetName($"{nameof(ValueTests)}-SingleEntry");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2009, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 4.2m))
                .SetName($"{nameof(ValueTests)}-SingleEntryEarly");

            values.SetData(new DateTime(2011, 1, 1), 7.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2009, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 4.2m))
                .SetName($"{nameof(ValueTests)}-TwoEntry");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2001, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 4.2m))
                .SetName($"{nameof(ValueTests)}-TwoEntryBefore");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2015, 6, 1),
                new DailyValuation(new DateTime(2011, 1, 1), 7.2m))
                .SetName($"{nameof(ValueTests)}-TwoEntryDatesAfter");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2010, 6, 1),
                new DailyValuation(new DateTime(2010, 6, 1), 5.4410958904109589041095890382m))
                .SetName($"{nameof(ValueTests)}-TwoEntryDateMiddle");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2010, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 4.2m))
                .SetName($"{nameof(ValueTests)}-TwoEntryDatesExact");
        }

        [TestCaseSource(nameof(ValuesData))]
        public void ValueTests(IValueList valueList, DateTime dateToTest, DailyValuation expectedValue)
        {
            DailyValuation actualValue = valueList.Value(dateToTest);
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

        private static IEnumerable<TestCaseData> ValuesBeforeData()
        {
            yield return new TestCaseData(
                new ValueList(),
                new DateTime(2010, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 0.0m))
                .SetName($"{nameof(ValueBeforeTests)}-NoEntry");

            var values = new TimeList();
            values.SetData(new DateTime(2010, 1, 1), 4.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2015, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 4.2m))
                .SetName($"{nameof(ValueBeforeTests)}-SingleEntry");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2009, 1, 1),
                new DailyValuation(new DateTime(2009, 1, 1), 0.0m))
                .SetName($"{nameof(ValueBeforeTests)}-SingleEntryEarly");

            values.SetData(new DateTime(2011, 1, 1), 7.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2001, 1, 1),
                new DailyValuation(new DateTime(2001, 1, 1), 0.0m))
                .SetName("ValueBefore-TwoEntryBefore");
            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2015, 6, 1),
                new DailyValuation(new DateTime(2011, 1, 1), 7.2m))
                .SetName($"{nameof(ValueBeforeTests)}-TwoEntryDatesAfter");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2010, 6, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 4.2m))
                .SetName($"{nameof(ValueBeforeTests)}-TwoEntryDateMiddle");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2010, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 0.0m))
                .SetName($"{nameof(ValueBeforeTests)}-TwoEntryDatesExact");
        }

        [TestCaseSource(nameof(ValuesBeforeData))]
        public void ValueBeforeTests(IValueList valueList, DateTime dateToTest, DailyValuation expectedValue)
        {
            DailyValuation actualValue = valueList.ValueBefore(dateToTest);
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

        private static IEnumerable<TestCaseData> ValuesOnOrBeforeData()
        {
            yield return new TestCaseData(
                new ValueList(),
                new DateTime(2010, 1, 1),
                null).SetName($"{nameof(ValueOnOrBeforeTests)}-NoEntry");

            var values = new TimeList();
            values.SetData(new DateTime(2010, 1, 1), 4.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2010, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 4.2m))
                .SetName($"{nameof(ValueOnOrBeforeTests)}-SingleEntry");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2009, 1, 1),
                null).SetName($"{nameof(ValueOnOrBeforeTests)}-SingleEntryEarly");

            values.SetData(new DateTime(2011, 1, 1), 7.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2001, 1, 1),
                null).SetName($"{nameof(ValueOnOrBeforeTests)}-TwoEntryBefore");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2015, 6, 1),
                new DailyValuation(new DateTime(2011, 1, 1), 7.2m))
                .SetName($"{nameof(ValueOnOrBeforeTests)}-TwoEntryDatesAfter");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2010, 6, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 4.2m))
                .SetName($"{nameof(ValueOnOrBeforeTests)}-TwoEntryDateMiddle");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2010, 1, 1),
                new DailyValuation(new DateTime(2010, 1, 1), 4.2m))
                .SetName($"{nameof(ValueOnOrBeforeTests)}-TwoEntryDatesExact");
        }

        [TestCaseSource(nameof(ValuesOnOrBeforeData))]
        public void ValueOnOrBeforeTests(IValueList valueList, DateTime dateToTest, DailyValuation expectedValue)
        {
            DailyValuation actualValue = valueList.ValueOnOrBefore(dateToTest);
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
