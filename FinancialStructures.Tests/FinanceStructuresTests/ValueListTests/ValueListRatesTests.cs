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
    public sealed class ValueListRatesTests
    {
        private static IEnumerable<TestCaseData> CarData()
        {
            yield return new TestCaseData(
                new ValueList(),
                new DateTime(2010, 1, 1),
                new DateTime(2020, 1, 1),
                double.NaN).SetName($"{nameof(CarTests)}-NoEntry");

            var values = new TimeList();
            values.SetData(new DateTime(2010, 1, 1), 4.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2009, 1, 1),
                new DateTime(2020, 1, 1),
                0.0).SetName($"{nameof(CarTests)}-SingleEntry");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2001, 1, 1),
                new DateTime(2004, 1, 1),
                0.0).SetName($"{nameof(CarTests)}-SingleEntryDatesBefore");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2012, 1, 1),
                new DateTime(2020, 1, 1),
                0.0).SetName($"{nameof(CarTests)}-SingleEntryDatesAfter");

            values.SetData(new DateTime(2011, 1, 1), 7.2m);

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2009, 1, 1),
                new DateTime(2020, 1, 1),
                0.71428571428571441).SetName($"{nameof(CarTests)}-TwoEntry");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2001, 1, 1),
                new DateTime(2002, 1, 1),
                0.0).SetName($"{nameof(CarTests)}-TwoEntryDatesBefore");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2015, 6, 1),
                new DateTime(2020, 1, 1),
                0.0).SetName($"{nameof(CarTests)}-TwoEntryDatesAfter");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2010, 6, 1),
                new DateTime(2020, 1, 1),
                0.6124287242648454).SetName($"{nameof(CarTests)}-TwoEntryDatesAcrossOne");

            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                new DateTime(2010, 1, 1),
                new DateTime(2011, 1, 1),
                0.71428571428571441).SetName($"{nameof(CarTests)}-TwoEntryDatesExact");
        }

        [TestCaseSource(nameof(CarData))]
        public void CarTests(IValueList valueList, DateTime start, DateTime end, double expectedCar)
        {
            double actualCar = valueList.CAR(start, end);
            Assert.AreEqual(expectedCar, actualCar);
        }
    }
}
