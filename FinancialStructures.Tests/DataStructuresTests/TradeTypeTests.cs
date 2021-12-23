using System.Collections.Generic;

using FinancialStructures.DataStructures;

using NUnit.Framework;

namespace FinancialStructures.Tests.DataStructuresTests
{
    [TestFixture]
    public sealed class TradeTypeTests
    {
        private static IEnumerable<TestCaseData> SignTestData()
        {
            yield return new TestCaseData(TradeType.Unknown, 1.0m);
            yield return new TestCaseData(TradeType.Buy, 1.0m);
            yield return new TestCaseData(TradeType.Sell, -1.0m);
            yield return new TestCaseData(TradeType.Dividend, -1.0m);
            yield return new TestCaseData(TradeType.DividendReinvestment, 1.0m);
            yield return new TestCaseData(TradeType.ShareReprice, 1.0m);
            yield return new TestCaseData(TradeType.CashPayout, -1.0m);
        }

        [TestCaseSource(nameof(SignTestData))]
        public void SignTests(TradeType tradeType, decimal expectedSign)
        {
            decimal actualSign = tradeType.Sign();
            Assert.AreEqual(expectedSign, actualSign);
        }

        private static IEnumerable<TestCaseData> InvestmentTypeTestData()
        {
            yield return new TestCaseData(TradeType.Unknown, false);
            yield return new TestCaseData(TradeType.Buy, true);
            yield return new TestCaseData(TradeType.Sell, true);
            yield return new TestCaseData(TradeType.Dividend, true);
            yield return new TestCaseData(TradeType.DividendReinvestment, false);
            yield return new TestCaseData(TradeType.ShareReprice, false);
            yield return new TestCaseData(TradeType.CashPayout, true);
        }

        [TestCaseSource(nameof(InvestmentTypeTestData))]
        public void InvestmentTypeTests(TradeType tradeType, bool expectedSign)
        {
            bool actualSign = tradeType.IsInvestmentTradeType();
            Assert.AreEqual(expectedSign, actualSign);
        }

        private static IEnumerable<TestCaseData> ShareAlteringTypeTestData()
        {
            yield return new TestCaseData(TradeType.Unknown, false);
            yield return new TestCaseData(TradeType.Buy, true);
            yield return new TestCaseData(TradeType.Sell, true);
            yield return new TestCaseData(TradeType.Dividend, false);
            yield return new TestCaseData(TradeType.DividendReinvestment, true);
            yield return new TestCaseData(TradeType.ShareReprice, true);
            yield return new TestCaseData(TradeType.CashPayout, false);
        }

        [TestCaseSource(nameof(ShareAlteringTypeTestData))]
        public void ShareAlteringTypeTests(TradeType tradeType, bool expectedSign)
        {
            bool actualSign = tradeType.IsShareNumberAlteringTradeType();
            Assert.AreEqual(expectedSign, actualSign);
        }
    }
}
