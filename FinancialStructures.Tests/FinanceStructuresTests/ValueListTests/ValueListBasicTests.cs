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
    public sealed class ValueListBasicTests
    {
        private static IEnumerable<TestCaseData> AnyData()
        {
            yield return new TestCaseData(new ValueList(), false)
                .SetName($"{nameof(AnyTests)}-NoEntry");

            var values = new TimeList();
            values.SetData(new DateTime(2010, 1, 1), 4.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                true)
                .SetName($"{nameof(AnyTests)}-SingleEntry");

            values.SetData(new DateTime(2011, 1, 1), 4.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                true)
                .SetName($"{nameof(AnyTests)}-TwoEntry");
        }

        [TestCaseSource(nameof(AnyData))]
        public void AnyTests(IValueList valueList, bool expectedAny)
        {
            bool actualAny = valueList.Any();
            Assert.AreEqual(expectedAny, actualAny);
        }

        private static IEnumerable<TestCaseData> CountData()
        {
            yield return new TestCaseData(new ValueList(), 0)
                .SetName($"{nameof(CountTests)}-NoEntry");

            var values = new TimeList();
            values.SetData(new DateTime(2010, 1, 1), 4.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                1).SetName($"{nameof(CountTests)}-SingleEntry");

            values.SetData(new DateTime(2011, 1, 1), 4.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                2).SetName($"{nameof(CountTests)}-TwoEntry");
        }

        [TestCaseSource(nameof(CountData))]
        public void CountTests(IValueList valueList, int expectedAny)
        {
            int actualAny = valueList.Count();
            Assert.AreEqual(expectedAny, actualAny);
        }
    }
}
