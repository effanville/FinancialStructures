using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.FinanceStructuresTests.SecurityTests
{
    [TestFixture]
    public sealed class SecurityEditTests
    {
        private static IEnumerable<TestCaseData> TradeData()
        {
            yield return new TestCaseData(
               SecurityConstructor.Default(),
               (new DateTime(2021, 4, 3), 12.5m, 100.0m, 100.0m, 0.0m, 0.0m, 0.0m),
               new[] {
                    (new DateTime(2021, 4, 3), 12.5m, 5.5m, 0.0m),
                    (new DateTime(2010, 1, 1), 100.0m, 2.0m, 200.0m),
                    (new DateTime(2011, 1, 1), 100.0m, 1.5m, 0.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 17.3m, 0.0m),
                    (new DateTime(2015, 4, 3), 90.6m, 4.0m, 0.0m),
                    (new DateTime(2018, 5, 6), 77.7m, 5.7m, 0.0m),
                    (new DateTime(2020, 1, 1), 101.1m, 5.5m, 0.0m)
               }).SetName("One");
            yield return new TestCaseData(
                SecurityConstructor.Default(),
                (new DateTime(2021, 4, 3), 12.5m, 100.0m, 100.0m, 10.0m, 12.3m, 5.0m),
                new[] {
                    (new DateTime(2021, 4, 3), 12.5m, 15.5m, 128.0m),
                    (new DateTime(2010, 1, 1), 100.0m, 2.0m, 200.0m),
                    (new DateTime(2011, 1, 1), 100.0m, 1.5m, 0.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 17.3m, 0.0m),
                    (new DateTime(2015, 4, 3), 90.6m, 4.0m, 0.0m),
                    (new DateTime(2018, 5, 6), 77.7m, 5.7m, 0.0m),
                    (new DateTime(2020, 1, 1), 101.1m, 5.5m, 0.0m)
                }).SetName("Two");
            yield return new TestCaseData(
                SecurityConstructor.Default(),
                (new DateTime(2019, 4, 3), 12.5m, 100.0m, 100.0m, 10.0m, 12.3m, 5.0m),
                new[] {
                    (new DateTime(2019, 4, 3), 12.5m, 15.7m, 128.0m),
                    (new DateTime(2010, 1, 1), 100.0m, 2.0m, 200.0m),
                    (new DateTime(2011, 1, 1), 100.0m, 1.5m, 0.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 17.3m, 0.0m),
                    (new DateTime(2015, 4, 3), 90.6m, 4.0m, 0.0m),
                    (new DateTime(2018, 5, 6), 77.7m, 5.7m, 0.0m),
                    (new DateTime(2020, 1, 1), 101.1m, 5.5m, 0.0m)
                }).SetName("Three");
            yield return new TestCaseData(
                SecurityConstructor.Default(),
                (new DateTime(2015, 4, 3), 12.7m, 100.0m, 100.0m, 10.0m, 12.3m, 5.0m),
                new[] {
                    (new DateTime(2010, 1, 1), 100.0m, 2.0m, 200.0m),
                    (new DateTime(2011, 1, 1), 100.0m, 1.5m, 0.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 17.3m, 0.0m),
                    (new DateTime(2015, 4, 3), 12.7m, 27.3m, 128.0m),
                    (new DateTime(2018, 5, 6), 77.7m, 5.7m, 0.0m),
                    (new DateTime(2020, 1, 1), 101.1m, 5.5m, 0.0m)
                }).SetName("Four");
            yield return new TestCaseData(
                SecurityConstructor.Default(),
                (new DateTime(2010, 1, 1), 12.7m, 100.0m, 100.0m, 10.0m, 12.3m, 5.0m),
                new[] {
                    (new DateTime(2010, 1, 1), 12.7m, 10.0m, 128.0m),
                    (new DateTime(2011, 1, 1), 100.0m, 1.5m, 0.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 17.3m, 0.0m),
                    (new DateTime(2015, 4, 3), 90.6m, 4.0m, 0.0m),
                    (new DateTime(2018, 5, 6), 77.7m, 5.7m, 0.0m),
                    (new DateTime(2020, 1, 1), 101.1m, 5.5m, 0.0m)
                }).SetName("Five");
            yield return new TestCaseData(
                SecurityConstructor.Default(),
                (new DateTime(2009, 1, 1), 12.7m, 100.0m, 101.0m, 10.0m, 12.3m, 5.0m),
                new[] {
                    (new DateTime(2009, 1, 1), 12.7m, 10.0m, 128.0m),
                    (new DateTime(2010, 1, 1), 100.0m, 12.0m, 200.0m),
                    (new DateTime(2011, 1, 1), 100.0m, 1.5m, 0.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 17.3m, 0.0m),
                    (new DateTime(2015, 4, 3), 90.6m, 4.0m, 0.0m),
                    (new DateTime(2018, 5, 6), 77.70m, 5.7m, 0.0m),
                    (new DateTime(2020, 1, 1), 101.1m, 5.5m, 0.0m)
                }).SetName("Six");
            yield return new TestCaseData(
                SecurityConstructor.Secondary(),
                (new DateTime(2015, 4, 3), 12.7m, 100.0m, 100.0m, 10.0m, 12.3m, 5.0m),
                new[] {
                    (new DateTime(2015, 4, 3), 12.7m, 29.8m, 128.0m),
                    (new DateTime(2010, 1, 5), 1010.0m,  2.0m, 2020.0m),
                    (new DateTime(2011, 2, 1), 1110.0m, 2.5m, 0.0m),
                    (new DateTime(2012, 5, 5), 1215.2m, 19.8m, 21022.96m),
                    (new DateTime(2016, 4, 3), 900.6m, 22.5m, 0.0m),
                    (new DateTime(2019, 5, 6), 1770.7m, 22.7m, 0.0m),
                    (new DateTime(2020, 1, 1), 1001.1m, 25.5m, 0.0m)
                }).SetName("Seven");
            yield return new TestCaseData(
                SecurityConstructor.Secondary(),
                (new DateTime(2021, 4, 3), 12.5m, 100.0m, 100.0m, 10.0m, 12.3m, 5.0m),
                new[] {
                    (new DateTime(2021, 4, 3), 12.5m, 35.5m, 128.0m),
                    (new DateTime(2010, 1, 5), 1010.0m, 2.0m, 2020.0m),
                    (new DateTime(2011, 2, 1), 1110.0m, 2.5m, 0.0m),
                    (new DateTime(2012, 5, 5), 1215.2m, 19.80m, 21022.96m),
                    (new DateTime(2016, 4, 3), 900.6m, 22.5m, 0.0m),
                    (new DateTime(2019, 5, 6), 1770.7m, 22.7m, 0.0m),
                    (new DateTime(2020, 1, 1), 1001.1m, 25.5m, 0.0m)
                }).SetName("Eight");
            yield return new TestCaseData(
                SecurityConstructor.Secondary(),
                (new DateTime(2019, 4, 3), 12.5m, 100.0m, 100.0m, 10.0m, 12.3m, 5.0m),
                new[] {
                    (new DateTime(2019, 4, 3), 12.5m, 32.5m, 128.0m),
                    (new DateTime(2010, 1, 5), 1010.0m, 2.0m, 2020.0m),
                    (new DateTime(2011, 2, 1), 1110.0m, 2.5m, 0.0m),
                    (new DateTime(2012, 5, 5), 1215.2m, 19.8m, 21022.96m),
                    (new DateTime(2016, 4, 3), 900.6m, 22.5m, 0.0m),
                    (new DateTime(2019, 5, 6), 1770.7m, 22.7m, 0.0m),
                    (new DateTime(2020, 1, 1), 1001.1m, 25.5m, 0.0m)
                }).SetName("Nine");
        }

        [TestCaseSource(nameof(TradeData))]
        public void SecurityTradeTests(Security sut, (DateTime Date, decimal UP, decimal Shares, decimal Inv, decimal TradeShares, decimal TradePrice, decimal TradeCost) dataToAdd, (DateTime Date, decimal UnitPrice, decimal ShareNo, decimal Investment)[] expectedValues)
        {
            _ = sut.AddOrEditData(dataToAdd.Date, dataToAdd.Date, dataToAdd.UP, dataToAdd.Shares, dataToAdd.Inv, trade: new SecurityTrade(TradeType.Buy, sut.Names.ToTwoName(), dataToAdd.Date, dataToAdd.TradeShares, dataToAdd.TradePrice, dataToAdd.TradeCost), reportLogger: null);

            Assert.Multiple(() =>
            {
                foreach ((DateTime Date, decimal UnitPrice, decimal ShareNo, decimal Investment) in expectedValues)
                {
                    SecurityDayData dayData = sut.DayData(Date);
                    Assert.AreEqual(Investment, dayData.NewInvestment, $"{Date} Investment value wrong");
                    Assert.AreEqual(ShareNo, dayData.ShareNo, $"{Date} Num Shares value wrong");
                    Assert.AreEqual(UnitPrice, dayData.UnitPrice, $"{Date} Unit Price wrong");
                }
            });
        }

        private static IEnumerable<TestCaseData> TradeEditData()
        {
            yield return new TestCaseData(
               SecurityConstructor.DefaultFromTrades(),
               (TradeType.Buy, new DateTime(2010, 1, 1), 2.0m, 100.0m, 2.0m),
               new[] {
                    (new DateTime(2010, 1, 1), 100.0m, 2.0m, 202.0m),
                    (new DateTime(2011, 1, 1), 100.0m, 1.5m, 0.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 17.3m, 0.0m),
               }).SetName("AlterTradeCost");
            yield return new TestCaseData(
               SecurityConstructor.DefaultFromTrades(),
               (TradeType.DividendReinvestment, new DateTime(2011, 1, 1), 0.5m, 100.0m, 0.0m),
               new[] {
                    (new DateTime(2010, 1, 1), 100.0m, 2.0m, 202.0m),
                    (new DateTime(2011, 1, 1), 100.0m, 2.5m, 50.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 18.3m, 0.0m),
               }).SetName("AlterToDivReinv");
            yield return new TestCaseData(
               SecurityConstructor.DefaultFromTrades(),
               (TradeType.ShareReprice, new DateTime(2012, 5, 1), 15.8m, 125.2m, 0.0m),
               new[] {
                    (new DateTime(2010, 1, 1), 100.0m, 2.0m, 202.0m),
                    (new DateTime(2011, 1, 1), 100.0m, 2.5m, 50.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 18.3m, 1978.16m),
               }).SetName("AlterToShareReprice");
            yield return new TestCaseData(
               SecurityConstructor.DefaultFromTrades(),
               (TradeType.ShareReset, new DateTime(2012, 5, 1), 15.8m, 125.2m, 0.0m),
               new[] {
                                (new DateTime(2010, 1, 1), 100.0m, 2.0m, 202.0m),
                                (new DateTime(2011, 1, 1), 100.0m, 2.5m, 50.0m),
                                (new DateTime(2012, 5, 1), 125.2m, 18.3m, 1978.16m),
               }).SetName("AlterToShareReset");
        }

        [TestCaseSource(nameof(TradeEditData))]
        public void EditSecurityTradeTests(
            Security sut,
            (TradeType TradeType, DateTime Date, decimal TradeShares, decimal TradePrice, decimal TradeCost) tradeEditData,
            (DateTime Date, decimal UnitPrice, decimal ShareNo, decimal Investment)[] expectedValues)
        {
            var oldTrade = sut.SecurityTrades.First(trade => trade.Day.Equals(tradeEditData.Date));
            _ = sut.TryAddOrEditTradeData(
                new SecurityTrade(tradeEditData.Date),
                new SecurityTrade(
                    TradeType.Buy,
                    sut.Names.ToTwoName(),
                    tradeEditData.Date,
                    tradeEditData.TradeShares,
                    tradeEditData.TradePrice,
                    tradeEditData.TradeCost),
                reportLogger: null);

            Assert.Multiple(() =>
            {
                foreach ((DateTime Date, decimal UnitPrice, decimal ShareNo, decimal Investment) in expectedValues)
                {
                    SecurityDayData dayData = sut.DayData(Date);
                    Assert.AreEqual(Investment, dayData.NewInvestment, $"{Date} Investment value wrong");
                    Assert.AreEqual(ShareNo, dayData.ShareNo, $"{Date} Num Shares value wrong");
                    Assert.AreEqual(UnitPrice, dayData.UnitPrice, $"{Date} Unit Price wrong");
                }
            });
        }

        private static IEnumerable<TestCaseData> TradeDeleteData()
        {
            yield return new TestCaseData(
               SecurityConstructor.DefaultFromTrades(),
               new DateTime(2010, 1, 1),
               new[] {
                    (new DateTime(2010, 1, 1), 100.0m, 0.0m, 0.0m),
                    (new DateTime(2011, 1, 1), 100.0m, -0.5m, 0.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 15.3m, 0.0m),
               }).SetName("DeleteFirstTrade");
            yield return new TestCaseData(
               SecurityConstructor.DefaultFromTrades(),
               new DateTime(2011, 1, 1),
               new[] {
                    (new DateTime(2010, 1, 1), 100.0m, 2.0m, 200.0m),
                    (new DateTime(2011, 1, 1), 100.0m, 0.0m, 0.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 17.8m, 0.0m),
               }).SetName("DeleteSecondTrade");
            yield return new TestCaseData(
               SecurityConstructor.DefaultFromTrades(),
               new DateTime(2012, 5, 1),
               new[] {
                    (new DateTime(2010, 1, 1), 100.0m, 2.0m, 200.0m),
                    (new DateTime(2011, 1, 1), 100.0m, 1.5m, 0.0m),
                    (new DateTime(2012, 5, 1), 125.2m, 0.0m, 0.0m),
               }).SetName("DeleteLastTrade");
        }

        [TestCaseSource(nameof(TradeDeleteData))]
        public void DeleteSecurityTradeTests(
            Security sut,
            DateTime tradeDeleteData,
            (DateTime Date, decimal UnitPrice, decimal ShareNo, decimal Investment)[] expectedValues)
        {
            _ = sut.TryDeleteTradeData(tradeDeleteData);

            Assert.Multiple(() =>
            {
                foreach ((DateTime Date, decimal UnitPrice, decimal ShareNo, decimal Investment) in expectedValues)
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
