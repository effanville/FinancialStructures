using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.FinanceStructuresTests
{
    [TestFixture]
    public sealed class SecurityTests
    {
        public static IEnumerable<TestCaseData> TradeData()
        {
            yield return new TestCaseData(
               SecurityConstructor.Default().Item,
               (new DateTime(2021, 4, 3), 12.5, 100.0, 100.0, 0.0, 0.0, 0.0),
               new[] {
                    (new DateTime(2021, 4, 3), 12.5, 5.5, 0.0),
                    (new DateTime(2010, 1, 1), 100.0, 2.0, 200.0),
                    (new DateTime(2011, 1, 1), 100.0, 1.5, 0.0),
                    (new DateTime(2012, 5, 1), 125.2, 17.300000000000001, 0.0),
                    (new DateTime(2015, 4, 3), 90.599999999999994, 4.0, 0.0),
                    (new DateTime(2018, 5, 6), 77.700000000000003, 5.7000000000000002, 0.0),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 5.5, 0.0)
               }).SetName("One");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                (new DateTime(2021, 4, 3), 12.5, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2021, 4, 3), 12.5, 15.5, 128.0),
                    (new DateTime(2010, 1, 1), 100.0, 2.0, 200.0),
                    (new DateTime(2011, 1, 1), 100.0, 1.5, 0.0),
                    (new DateTime(2012, 5, 1), 125.2, 17.300000000000001, 0.0),
                    (new DateTime(2015, 4, 3), 90.599999999999994, 4.0, 0.0),
                    (new DateTime(2018, 5, 6), 77.700000000000003, 5.7000000000000002, 0.0),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 5.5, 0.0)
                }).SetName("Two");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                (new DateTime(2019, 4, 3), 12.5, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2019, 4, 3), 12.5, 15.7, 128.0),
                    (new DateTime(2010, 1, 1), 100.0, 2.0, 200.0),
                    (new DateTime(2011, 1, 1), 100.0, 1.5, 0.0),
                    (new DateTime(2012, 5, 1), 125.2, 17.300000000000001, 0.0),
                    (new DateTime(2015, 4, 3), 90.599999999999994, 4.0, 0.0),
                    (new DateTime(2018, 5, 6), 77.700000000000003, 5.7000000000000002, 0.0),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 5.5, 0.0)
                }).SetName("Three");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                (new DateTime(2015, 4, 3), 12.7, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2010, 1, 1), 100.0, 2.0, 200.0),
                    (new DateTime(2011, 1, 1), 100.0, 1.5, 0.0),
                    (new DateTime(2012, 5, 1), 125.2, 17.300000000000001, 0.0),
                    (new DateTime(2015, 4, 3), 12.699999999999999, 27.300000000000001, 128.0),
                    (new DateTime(2018, 5, 6), 77.700000000000003, 5.7, 0.0),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 5.5, 0.0)
                }).SetName("Four");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                (new DateTime(2010, 1, 1), 12.7, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2010, 1, 1), 12.7, 10.0, 128.0),
                    (new DateTime(2011, 1, 1), 100.0, 1.5, 0.0),
                    (new DateTime(2012, 5, 1), 125.2, 17.300000000000001, 0.0),
                    (new DateTime(2015, 4, 3), 90.599999999999994, 4.0, 0.0),
                    (new DateTime(2018, 5, 6), 77.700000000000003, 5.7, 0.0),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 5.5, 0.0)
                }).SetName("Five");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                (new DateTime(2009, 1, 1), 12.7, 100.0, 101.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2009, 1, 1), 12.7, 10.0, 128.0),
                    (new DateTime(2010, 1, 1), 100.0, 2.0, 200.0),
                    (new DateTime(2011, 1, 1), 100.0, 1.5, 0.0),
                    (new DateTime(2012, 5, 1), 125.2, 17.3, 0.0),
                    (new DateTime(2015, 4, 3), 90.599999999999994, 4.0, 0.0),
                    (new DateTime(2018, 5, 6), 77.70, 5.7, 0.0),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 5.5, 0.0)
                }).SetName("Six");
            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                (new DateTime(2015, 4, 3), 12.7, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2015, 4, 3), 12.7, 29.8, 128.0),
                    (new DateTime(2010, 1, 5), 1010.0,  2.0, 2020.0),
                    (new DateTime(2011, 2, 1), 1110.0, 2.5, 0.0),
                    (new DateTime(2012, 5, 5), 1215.2, 19.8, 21022.960000000003),
                    (new DateTime(2016, 4, 3), 900.6, 22.5, 0.0),
                    (new DateTime(2019, 5, 6), 1770.7, 22.7, 0.0),
                    (new DateTime(2020, 1, 1), 1001.1, 25.5, 0.0)
                }).SetName("Seven");
            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                (new DateTime(2021, 4, 3), 12.5, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2021, 4, 3), 12.5, 35.5, 128.0),
                    (new DateTime(2010, 1, 5), 1010.0, 2.0, 2020.0),
                    (new DateTime(2011, 2, 1), 1110.0, 2.5, 0.0),
                    (new DateTime(2012, 5, 5), 1215.2, 19.80, 21022.960000000003),
                    (new DateTime(2016, 4, 3), 900.6, 22.5, 0.0),
                    (new DateTime(2019, 5, 6), 1770.7, 22.699999999999999, 0.0),
                    (new DateTime(2020, 1, 1), 1001.1, 25.5, 0.0)
                }).SetName("Eight");
            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                (new DateTime(2019, 4, 3), 12.5, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2019, 4, 3), 12.5, 32.5, 128.0),
                    (new DateTime(2010, 1, 5), 1010.0, 2.0, 2020.0),
                    (new DateTime(2011, 2, 1), 1110.0, 2.5, 0.0),
                    (new DateTime(2012, 5, 5), 1215.2, 19.8, 21022.960000000003),
                    (new DateTime(2016, 4, 3), 900.6, 22.5, 0.0),
                    (new DateTime(2019, 5, 6), 1770.7, 22.7, 0.0),
                    (new DateTime(2020, 1, 1), 1001.1, 25.5, 0.0)
                }).SetName("Nine");
        }

        [TestCaseSource(nameof(TradeData))]
        public void SecurityTradeTests(Security sut, (DateTime Date, double UP, double Shares, double Inv, double TradeShares, double TradePrice, double TradeCost) dataToAdd, (DateTime Date, double UnitPrice, double ShareNo, double Investment)[] expectedValues)
        {
            _ = sut.AddOrEditData(dataToAdd.Date, dataToAdd.Date, dataToAdd.UP, dataToAdd.Shares, dataToAdd.Inv, trade: new SecurityTrade(TradeType.Buy, sut.Names.ToTwoName(), dataToAdd.Date, dataToAdd.TradeShares, dataToAdd.TradePrice, dataToAdd.TradeCost), reportLogger: null);

            Assert.Multiple(() =>
            {
                foreach ((DateTime Date, double UnitPrice, double ShareNo, double Investment) in expectedValues)
                {
                    SecurityDayData dayData = sut.DayData(Date);
                    Assert.AreEqual(Investment, dayData.NewInvestment, $"{Date} Investment value wrong");
                    Assert.AreEqual(ShareNo, dayData.ShareNo, $"{Date} Num Shares value wrong");
                    Assert.AreEqual(UnitPrice, dayData.UnitPrice, $"{Date} Unit Price wrong");
                }
            });
        }
    }
}
