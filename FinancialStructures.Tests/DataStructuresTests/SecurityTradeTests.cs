using System;
using System.Collections.Generic;

using FinancialStructures.DataStructures;

using NUnit.Framework;

namespace FinancialStructures.Tests.DataStructuresTests
{
    [TestFixture]
    public sealed class SecurityTradeTests
    {
        private static IEnumerable<TestCaseData> TotalCostData()
        {
            yield return new TestCaseData(TradeType.Buy, 1.5m, 1m, 5m, 6.5m).SetName("BuyTrade");
            yield return new TestCaseData(TradeType.Sell, 1.5m, 1m, 5m, -3.5m).SetName("SellTrade");
            yield return new TestCaseData(TradeType.Dividend, 1.5m, 1m, 5m, -3.5m).SetName("DividendTrade");
            yield return new TestCaseData(TradeType.DividendReinvestment, 1.5m, 1m, 5m, 0.0m).SetName("DividendReinvTrade");
            yield return new TestCaseData(TradeType.ShareReprice, 1.5m, 1m, 5m, 0.0m).SetName("ShareRepriceTrade");
            yield return new TestCaseData(TradeType.CashPayout, 1.5m, 1m, 5m, -3.5m).SetName("CashPayoutTrade");
        }

        [TestCaseSource(nameof(TotalCostData))]
        public void TradeTotalCostTests(TradeType tradeType, decimal unitPrice, decimal numUnits, decimal tradeCosts, decimal expectedCost)
        {
            var trade = new SecurityTrade(tradeType, null, DateTime.Today, numUnits, unitPrice, tradeCosts);
            decimal totalCost = trade.TotalCost;
            Assert.AreEqual(expectedCost, totalCost);
        }
    }
}
