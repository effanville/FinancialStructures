using System;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures.Implementation;
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
               (new DateTime(2021, 4, 3), 12.5),
               new[] {
                   (new DateTime(2021, 4, 3), 12.5),
                   (new DateTime(2010, 1, 1), 100.0),
                   (new DateTime(2011, 1, 1), 100.0),
                   (new DateTime(2012, 5, 5), 125.07029053420807),
                   (new DateTime(2016, 4, 3), 86.418069087688224),
                   (new DateTime(2019, 5, 6), 91.817355371900817),
                   (new DateTime(2020, 1, 1), 101.09999999999999)
               });
            yield return new TestCaseData(
                BankAccountConstructor.Default().Item,
                (new DateTime(2021, 4, 3), 12.5),
                new[] {
                    (new DateTime(2021, 4, 3), 12.5),
                    (new DateTime(2010, 1, 1), 100.0),
                    (new DateTime(2011, 1, 1), 100.0),
                    (new DateTime(2012, 5, 5), 125.07029053420807),
                    (new DateTime(2016, 4, 3), 86.418069087688224),
                    (new DateTime(2019, 5, 6), 91.817355371900817),
                    (new DateTime(2020, 1, 1), 101.09999999999999)
                });
            yield return new TestCaseData(
                BankAccountConstructor.Default().Item,
                (new DateTime(2019, 4, 3), 12.5),
                new[] {
                    (new DateTime(2019, 4, 3), 12.5),
                    (new DateTime(2010, 1, 1), 100.0),
                    (new DateTime(2011, 1, 1), 100.0),
                    (new DateTime(2012, 5, 5), 125.07029053420807),
                    (new DateTime(2016, 4, 3), 86.418069087688224),
                    (new DateTime(2019, 5, 6), 23.209890109890111),
                    (new DateTime(2020, 1, 1), 101.09999999999999)
                });
            yield return new TestCaseData(
                BankAccountConstructor.Default().Item,
                (new DateTime(2015, 4, 3), 12.7),
                new[] {
                    (new DateTime(2015, 4, 3), 12.7),
                    (new DateTime(2010, 1, 1), 100.0),
                    (new DateTime(2011, 1, 1), 100.0),
                    (new DateTime(2012, 5, 5), 124.77825679475164),
                    (new DateTime(2016, 4, 3), 33.771744906997341),
                    (new DateTime(2019, 5, 6), 91.817355371900817),
                    (new DateTime(2020, 1, 1), 101.09999999999999)
                });
            yield return new TestCaseData(
                BankAccountConstructor.Default().Item,
                (new DateTime(2010, 1, 1), 12.7),
                new[] {
                    (new DateTime(2010, 1, 1), 12.7),
                    (new DateTime(2011, 1, 1), 100.0),
                    (new DateTime(2012, 5, 5), 125.07029053420807),
                    (new DateTime(2016, 4, 3), 86.418069087688224),
                    (new DateTime(2019, 5, 6), 91.817355371900817),
                    (new DateTime(2020, 1, 1), 101.09999999999999)
                });
            yield return new TestCaseData(
                BankAccountConstructor.Default().Item,
                (new DateTime(2009, 1, 1), 12.7),
                new[] {
                    (new DateTime(2009, 1, 1), 12.7),
                    (new DateTime(2010, 1, 1), 100.0),
                    (new DateTime(2011, 1, 1), 100.0),
                    (new DateTime(2012, 5, 5), 125.07029053420807),
                    (new DateTime(2016, 4, 3), 86.418069087688224),
                    (new DateTime(2019, 5, 6), 91.817355371900817),
                    (new DateTime(2020, 1, 1), 101.09999999999999)
                });
            yield return new TestCaseData(
                BankAccountConstructor.Secondary().Item,
                (new DateTime(2015, 4, 3), 12.7),
                new[] {
                    (new DateTime(2015, 4, 3), 12.7),
                    (new DateTime(2010, 1, 1), 1100.0),
                    (new DateTime(2011, 2, 1), 2037.8213991769546),
                    (new DateTime(2012, 5, 5), 1121.0294283036551),
                    (new DateTime(2016, 4, 3), 258.42896368467672),
                    (new DateTime(2019, 5, 6), 909.70165289256204),
                    (new DateTime(2020, 1, 1), 1001.1)
                });
            yield return new TestCaseData(
                BankAccountConstructor.Secondary().Item,
                (new DateTime(2021, 4, 3), 12.5),
                new[] {
                    (new DateTime(2021, 4, 3), 12.5),
                    (new DateTime(2010, 1, 1), 1100),
                    (new DateTime(2011, 2, 1), 2037.8213991769546),
                    (new DateTime(2012, 5, 5), 1124.3580131208998),
                    (new DateTime(2016, 4, 3), 858.48892825509301),
                    (new DateTime(2019, 5, 6), 909.70165289256204),
                    (new DateTime(2020, 1, 1), 1001.1)
                });
            yield return new TestCaseData(
                BankAccountConstructor.Secondary().Item,
                (new DateTime(2019, 4, 3), 12.5),
                new[] {
                    (new DateTime(2019, 4, 3), 12.5),
                    (new DateTime(2010, 1, 1), 1100),
                    (new DateTime(2011, 2, 1), 2037.8213991769546),
                    (new DateTime(2012, 5, 5), 1124.3580131208998),
                    (new DateTime(2016, 4, 3), 858.48892825509301),
                    (new DateTime(2019, 5, 6), 132.00109890109889),
                    (new DateTime(2020, 1, 1), 1001.1)
                });
        }

        [TestCaseSource(nameof(EditDataValues))]
        public void TryEditDataTests(CashAccount sut, (DateTime Date, double Value) dataToAdd, (DateTime Date, double Value)[] expectedValues)
        {
            _ = sut.TryEditData(dataToAdd.Date, dataToAdd.Date, dataToAdd.Value, reportLogger: null);

            Assert.Multiple(() =>
            {
                foreach (var (Date, Value) in expectedValues)
                {
                    var dayData = sut.Value(Date, null);
                    Assert.AreEqual(Value, dayData.Value, $"{Date} Vsalue wrong");
                }
            });
        }
    }
}
