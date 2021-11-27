using System;
using System.Collections.Generic;

using Common.Structure.DataStructures;

using FinancialStructures.FinanceStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;

using NUnit.Framework;

namespace FinancialStructures.Tests.FinanceStructuresTests
{
    [TestFixture]
    public sealed class CashAccountTests
    {
        public static IEnumerable<TestCaseData> EditDataValues()
        {
            yield return new TestCaseData(
               BankAccountConstructor.Default().Item,
               (new DateTime(2021, 4, 3), 12.5m),
               new[] {
                   (new DateTime(2021, 4, 3), 12.5m),
                   (new DateTime(2010, 1, 1), 100.0m),
                   (new DateTime(2011, 1, 1), 100.0m),
                   (new DateTime(2012, 5, 5), 125.07029053420807m),
                   (new DateTime(2016, 4, 3), 86.418069087688224m),
                   (new DateTime(2019, 5, 6), 91.817355371900817m),
                   (new DateTime(2020, 1, 1), 101.09999999999999m)
               }).SetName("One");
            yield return new TestCaseData(
                BankAccountConstructor.Default().Item,
                (new DateTime(2021, 4, 3), 12.5m),
                new[] {
                    (new DateTime(2021, 4, 3), 12.5m),
                    (new DateTime(2010, 1, 1), 100.0m),
                    (new DateTime(2011, 1, 1), 100.0m),
                    (new DateTime(2012, 5, 5), 125.07029053420807m),
                    (new DateTime(2016, 4, 3), 86.418069087688224m),
                    (new DateTime(2019, 5, 6), 91.817355371900817m),
                    (new DateTime(2020, 1, 1), 101.09999999999999m)
                }).SetName("Two");
            yield return new TestCaseData(
                BankAccountConstructor.Default().Item,
                (new DateTime(2019, 4, 3), 12.5m),
                new[] {
                    (new DateTime(2019, 4, 3), 12.5m),
                    (new DateTime(2010, 1, 1), 100.0m),
                    (new DateTime(2011, 1, 1), 100.0m),
                    (new DateTime(2012, 5, 5), 125.07029053420807m),
                    (new DateTime(2016, 4, 3), 86.418069087688224m),
                    (new DateTime(2019, 5, 6), 23.209890109890111m),
                    (new DateTime(2020, 1, 1), 101.09999999999999m)
                }).SetName("Three");
            yield return new TestCaseData(
                BankAccountConstructor.Default().Item,
                (new DateTime(2015, 4, 3), 12.7m),
                new[] {
                    (new DateTime(2015, 4, 3), 12.7m),
                    (new DateTime(2010, 1, 1), 100.0m),
                    (new DateTime(2011, 1, 1), 100.0m),
                    (new DateTime(2012, 5, 5), 124.77825679475164m),
                    (new DateTime(2016, 4, 3), 33.771744906997341m),
                    (new DateTime(2019, 5, 6), 91.817355371900817m),
                    (new DateTime(2020, 1, 1), 101.09999999999999m)
                }).SetName("Four");
            yield return new TestCaseData(
                BankAccountConstructor.Default().Item,
                (new DateTime(2010, 1, 1), 12.7m),
                new[] {
                    (new DateTime(2010, 1, 1), 12.7m),
                    (new DateTime(2011, 1, 1), 100.0m),
                    (new DateTime(2012, 5, 5), 125.07029053420807m),
                    (new DateTime(2016, 4, 3), 86.418069087688224m),
                    (new DateTime(2019, 5, 6), 91.817355371900817m),
                    (new DateTime(2020, 1, 1), 101.09999999999999m)
                }).SetName("Five");
            yield return new TestCaseData(
                BankAccountConstructor.Default().Item,
                (new DateTime(2009, 1, 1), 12.7m),
                new[] {
                    (new DateTime(2009, 1, 1), 12.7m),
                    (new DateTime(2010, 1, 1), 100.0m),
                    (new DateTime(2011, 1, 1), 100.0m),
                    (new DateTime(2012, 5, 5), 125.07029053420807m),
                    (new DateTime(2016, 4, 3), 86.418069087688224m),
                    (new DateTime(2019, 5, 6), 91.817355371900817m),
                    (new DateTime(2020, 1, 1), 101.09999999999999m)
                }).SetName("Six");
            yield return new TestCaseData(
                BankAccountConstructor.Secondary().Item,
                (new DateTime(2015, 4, 3), 12.7m),
                new[] {
                    (new DateTime(2015, 4, 3), 12.7m),
                    (new DateTime(2010, 1, 1), 1100.0m),
                    (new DateTime(2011, 2, 1), 2037.8213991769546m),
                    (new DateTime(2012, 5, 5), 1121.0294283036551m),
                    (new DateTime(2016, 4, 3), 258.42896368467672m),
                    (new DateTime(2019, 5, 6), 909.70165289256204m),
                    (new DateTime(2020, 1, 1), 1001.1m)
                }).SetName("Seven");
            yield return new TestCaseData(
                BankAccountConstructor.Secondary().Item,
                (new DateTime(2021, 4, 3), 12.5m),
                new[] {
                    (new DateTime(2021, 4, 3), 12.5m),
                    (new DateTime(2010, 1, 1), 1100m),
                    (new DateTime(2011, 2, 1), 2037.8213991769546m),
                    (new DateTime(2012, 5, 5), 1124.3580131208998m),
                    (new DateTime(2016, 4, 3), 858.48892825509301m),
                    (new DateTime(2019, 5, 6), 909.70165289256204m),
                    (new DateTime(2020, 1, 1), 1001.1m)
                }).SetName("Eight");
            yield return new TestCaseData(
                BankAccountConstructor.Secondary().Item,
                (new DateTime(2019, 4, 3), 12.5m),
                new[] {
                    (new DateTime(2019, 4, 3), 12.5m),
                    (new DateTime(2010, 1, 1), 1100m),
                    (new DateTime(2011, 2, 1), 2037.8213991769546m),
                    (new DateTime(2012, 5, 5), 1124.3580131208998m),
                    (new DateTime(2016, 4, 3), 858.48892825509301m),
                    (new DateTime(2019, 5, 6), 132.00109890109889m),
                    (new DateTime(2020, 1, 1), 1001.1m)
                }).SetName("Nine");
        }

        [TestCaseSource(nameof(EditDataValues))]
        public void TryEditDataTests(IExchangableValueList sut, (DateTime Date, decimal Value) dataToAdd, (DateTime Date, decimal Value)[] expectedValues)
        {
            _ = sut.TryEditData(dataToAdd.Date, dataToAdd.Date, dataToAdd.Value, reportLogger: null);

            Assert.Multiple(() =>
            {
                foreach ((DateTime Date, decimal value) in expectedValues)
                {
                    DailyValuation dayData = sut.Value(Date, null);

                    Assert.That(dayData.Value, Is.EqualTo(value).Within(1e-12m), $"{Date} Vsalue wrong");
                }
            });
        }
    }
}
