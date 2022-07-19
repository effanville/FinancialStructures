using System;
using System.Collections.Generic;
using Common.Structure.DataStructures;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.FinanceStructures.Statistics;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;

using NUnit.Framework;

namespace FinancialStructures.Tests.FinanceStructuresTests
{
    [TestFixture]
    public sealed class StatisticsTests
    {
        private static IEnumerable<TestCaseData> MeanSharePriceData()
        {
            yield return new TestCaseData(
                SecurityConstructor.Empty().Item,
                TradeType.Buy,
                0.0m)
                .SetName("MeanSharePrice-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                TradeType.Buy,
                100m)
                .SetName("MeanSharePrice-DefaultSec");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                TradeType.Buy,
                1193.9357512953367875647668394m)
                .SetName("MeanSharePrice-SecondarySecBuy");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                TradeType.Sell,
                0m)
                .SetName("MeanSharePrice-SecondarySecSell");
        }

        [TestCaseSource(nameof(MeanSharePriceData))]
        public void MeanSharePriceTests(ISecurity valueList, TradeType tradeType, decimal expectedValue)
        {
            decimal actualValue = valueList.MeanSharePrice(tradeType);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedValue, actualValue);
            });
        }

        private static IEnumerable<TestCaseData> SecurityProfitData()
        {
            yield return new TestCaseData(
                SecurityConstructor.Empty().Item,
                0.0m)
                .SetName($"{nameof(SecurityProfitTests)}-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                356.05m)
                .SetName($"{nameof(SecurityProfitTests)}-DefaultSec");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                2485.09m)
                .SetName($"{nameof(SecurityProfitTests)}-SecondarySec");
        }

        [TestCaseSource(nameof(SecurityProfitData))]
        public void SecurityProfitTests(ISecurity valueList, decimal expectedValue)
        {
            decimal actualValue = valueList.Profit(null);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedValue, actualValue);
            });
        }

        private static IEnumerable<TestCaseData> SecurityProfitAsIExchData()
        {
            yield return new TestCaseData(
                SecurityConstructor.Empty().Item,
                0.0m)
                .SetName($"{nameof(SecurityProfitAsIExchTests)}-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                356.05m)
                .SetName($"{nameof(SecurityProfitAsIExchTests)}-DefaultSec");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                2485.09m)
                .SetName($"{nameof(SecurityProfitAsIExchTests)}-SecondarySec");
        }

        [TestCaseSource(nameof(SecurityProfitAsIExchData))]
        public void SecurityProfitAsIExchTests(IExchangableValueList valueList, decimal expectedValue)
        {
            decimal actualValue = valueList.Profit(null);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedValue, actualValue);
            });
        }

        private static IEnumerable<TestCaseData> ValueListProfitData()
        {
            yield return new TestCaseData(new ValueList(), 0m)
                .SetName($"{nameof(ValueListProfitTests)}-NoEntry");
            var values = new TimeList();
            values.SetData(new DateTime(2010, 1, 1), 4.2m);
            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                0.0m)
                .SetName($"{nameof(ValueListProfitTests)}-SingleEntry");

            values.SetData(new DateTime(2011, 1, 1), 6.2m);
            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                2.0m)
                .SetName($"{nameof(ValueListProfitTests)}-TwoEntry");
        }

        [TestCaseSource(nameof(ValueListProfitData))]
        public void ValueListProfitTests(IValueList valueList, decimal expectedAny)
        {
            decimal actualAny = valueList.Profit();
            Assert.AreEqual(expectedAny, actualAny);
        }

        private static IEnumerable<TestCaseData> ValueListRecentChangeData()
        {
            yield return new TestCaseData(new ValueList(), 0m)
                .SetName($"{nameof(ValueListRecentChangeTests)}-NoEntry");
            var values = new TimeList();
            values.SetData(new DateTime(2010, 1, 1), 4.2m);
            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                4.2m)
                .SetName($"{nameof(ValueListRecentChangeTests)}-SingleEntry");

            values.SetData(new DateTime(2011, 1, 1), 6.2m);
            yield return new TestCaseData(
                new ValueList(
                    new NameData(),
                    values.Copy()),
                2.0m)
                .SetName($"{nameof(ValueListRecentChangeTests)}-TwoEntry");
        }

        [TestCaseSource(nameof(ValueListRecentChangeData))]
        public void ValueListRecentChangeTests(IValueList valueList, decimal expectedAny)
        {
            decimal actualAny = valueList.RecentChange();
            Assert.AreEqual(expectedAny, actualAny);
        }

        private static IEnumerable<TestCaseData> SecurityRecentChangeData()
        {
            yield return new TestCaseData(
                SecurityConstructor.Empty().Item,
                0.0m)
                .SetName($"{nameof(SecurityRecentChangeTests)}-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                113.16m)
                .SetName($"{nameof(SecurityRecentChangeTests)}-DefaultSec");

            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                -14666.84m)
                .SetName($"{nameof(SecurityRecentChangeTests)}-SecondarySec");
        }

        [TestCaseSource(nameof(SecurityRecentChangeData))]
        public void SecurityRecentChangeTests(ISecurity valueList, decimal expectedValue)
        {
            decimal actualValue = valueList.RecentChange(null);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedValue, actualValue);
            });
        }

        private static IEnumerable<TestCaseData> BankAccRecentChangeData()
        {
            yield return new TestCaseData(
                BankAccountConstructor.Empty().Item,
                0.0m)
                .SetName($"{nameof(BankAccRecentChangeTests)}-NoEntry");

            yield return new TestCaseData(
                BankAccountConstructor.Default().Item,
                23.4m)
                .SetName($"{nameof(BankAccRecentChangeTests)}-DefaultSec");

            yield return new TestCaseData(
                BankAccountConstructor.Secondary().Item,
                230.4m)
                .SetName($"{nameof(BankAccRecentChangeTests)}-SecondarySec");
        }

        [TestCaseSource(nameof(BankAccRecentChangeData))]
        public void BankAccRecentChangeTests(IExchangableValueList valueList, decimal expectedValue)
        {
            decimal actualValue = valueList.RecentChange(null);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedValue, actualValue);
            });
        }
    }
}
