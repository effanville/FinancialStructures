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
                    (new DateTime(2011, 1, 1), 100.0, 1.5, -50.0),
                    (new DateTime(2012, 5, 1), 125.2, 17.300000000000001, 1978.16),
                    (new DateTime(2015, 4, 3), 90.599999999999994, 4.0, -1204.98),
                    (new DateTime(2018, 5, 6), 77.700000000000003, 5.7000000000000002, 132.09000000000003),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 5.5, -20.220000000000017)
               }).SetName("One");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                (new DateTime(2021, 4, 3), 12.5, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2021, 4, 3), 12.5, 15.5, 128.0),
                    (new DateTime(2010, 1, 1), 100.0, 2.0, 200.0),
                    (new DateTime(2011, 1, 1), 100.0, 1.5, -50.0),
                    (new DateTime(2012, 5, 1), 125.2, 17.300000000000001, 1978.16),
                    (new DateTime(2015, 4, 3), 90.599999999999994, 4.0, -1204.98),
                    (new DateTime(2018, 5, 6), 77.700000000000003, 5.7000000000000002, 132.09000000000003),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 5.5, -20.220000000000017)
                }).SetName("Two");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                (new DateTime(2019, 4, 3), 12.5, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2019, 4, 3), 12.5, 15.7, 128.0),
                    (new DateTime(2010, 1, 1), 100.0, 2.0, 200.0),
                    (new DateTime(2011, 1, 1), 100.0, 1.5, -50.0),
                    (new DateTime(2012, 5, 1), 125.2, 17.300000000000001, 1978.16),
                    (new DateTime(2015, 4, 3), 90.599999999999994, 4.0, -1204.98),
                    (new DateTime(2018, 5, 6), 77.700000000000003, 5.7000000000000002, 132.09000000000003),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 15.5, -20.220000000000017)
                }).SetName("Three");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                (new DateTime(2015, 4, 3), 12.7, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2010, 1, 1), 100.0, 2.0, 200.0),
                    (new DateTime(2011, 1, 1), 100.0, 1.5, -50.0),
                    (new DateTime(2012, 5, 1), 125.2, 17.300000000000001, 1978.16),
                    (new DateTime(2015, 4, 3), 12.699999999999999, 27.300000000000001, 128.0),
                    (new DateTime(2018, 5, 6), 77.700000000000003, 29.0, 132.09000000000003),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 28.800000000000001, -20.220000000000017)
                }).SetName("Four");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                (new DateTime(2010, 1, 1), 12.7, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2010, 1, 1), 12.7, 10.0, 128.0),
                    (new DateTime(2011, 1, 1), 100.0, 9.5, -50.0),
                    (new DateTime(2012, 5, 1), 125.2, 25.300000000000001, 1978.16),
                    (new DateTime(2015, 4, 3), 90.599999999999994, 12.0, -1204.98),
                    (new DateTime(2018, 5, 6), 77.700000000000003, 13.699999999999999, 132.09000000000003),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 13.5, -20.220000000000017)
                }).SetName("Five");
            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                (new DateTime(2009, 1, 1), 12.7, 100.0, 101.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2009, 1, 1), 12.7, 10.0, 128.0),
                    (new DateTime(2010, 1, 1), 100.0, 12.0, 200.0),
                    (new DateTime(2011, 1, 1), 100.0, 11.5, -50.0),
                    (new DateTime(2012, 5, 1), 125.2, 27.3, 1978.16),
                    (new DateTime(2015, 4, 3), 90.599999999999994, 14.0, -1204.98),
                    (new DateTime(2018, 5, 6), 77.70, 15.7, 132.09000000000003),
                    (new DateTime(2020, 1, 1), 101.09999999999999, 15.5, -20.220000000000017)
                }).SetName("Six");
            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                (new DateTime(2015, 4, 3), 12.7, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2015, 4, 3), 12.7, 29.8, 128.0),
                    (new DateTime(2010, 1, 5), 1010.0,  2.0, 2020.0),
                    (new DateTime(2011, 2, 1), 1110.0, 2.5, 555.0),
                    (new DateTime(2012, 5, 5), 1215.2, 19.8, 21022.960000000003),
                    (new DateTime(2016, 4, 3), 900.6, 32.5, 2431.6199999999994),
                    (new DateTime(2019, 5, 6), 1770.7, 32.7, 354.13999999999874),
                    (new DateTime(2020, 1, 1), 1001.1, 35.5, 2803.0800000000008)
                }).SetName("Seven");
            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                (new DateTime(2021, 4, 3), 12.5, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2021, 4, 3), 12.5, 35.5, 128.0),
                    (new DateTime(2010, 1, 5), 1010.0, 2.0, 2020.0),
                    (new DateTime(2011, 2, 1), 1110.0, 2.5, 555.0),
                    (new DateTime(2012, 5, 5), 1215.2, 19.80, 21022.960000000003),
                    (new DateTime(2016, 4, 3), 900.6, 22.5, 2431.6199999999994),
                    (new DateTime(2019, 5, 6), 1770.7, 22.699999999999999, 354.13999999999874),
                    (new DateTime(2020, 1, 1), 1001.1, 25.5, 2803.0800000000008)
                }).SetName("Eight");
            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                (new DateTime(2019, 4, 3), 12.5, 100.0, 100.0, 10.0, 12.3, 5.0),
                new[] {
                    (new DateTime(2019, 4, 3), 12.5, 32.5, 128.0),
                    (new DateTime(2010, 1, 5), 1010.0, 2.0, 2020.0),
                    (new DateTime(2011, 2, 1), 1110.0, 2.5, 555.0),
                    (new DateTime(2012, 5, 5), 1215.2, 19.8, 21022.960000000003),
                    (new DateTime(2016, 4, 3), 900.6, 22.5, 2431.6199999999994),
                    (new DateTime(2019, 5, 6), 1770.7, 32.7, 354.13999999999874),
                    (new DateTime(2020, 1, 1), 1001.1, 35.5, 2803.0800000000008)
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
