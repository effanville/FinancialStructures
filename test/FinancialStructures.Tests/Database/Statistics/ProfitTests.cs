﻿using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Extensions.Statistics;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class ProfitTests
    {
        [TestCase(TestDatabaseName.OneBank, Account.All, NameOrder.Default, 0)]
        [TestCase(TestDatabaseName.OneBank, Account.Security, NameOrder.Default, 0)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, NameOrder.Default, 1.1)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Account.All, NameOrder.Default, 0)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Account.Security, NameOrder.Default, 356.04999999999995)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, NameOrder.Default, 356.04999999999995)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.Currency, NameOrder.Default, 0.017699999999999994)]
        public void ProfitTest(TestDatabaseName databaseName, Account totals, NameOrder order, decimal expectedValue)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.Profit(totals, TestDatabase.Name(totals, order)));
        }

        [TestCase(TestDatabaseName.OneBank, Totals.All, 1.1)]
        [TestCase(TestDatabaseName.OneBank, Totals.Security, 0.0)]
        [TestCase(TestDatabaseName.OneBank, Totals.BankAccount, 1.1)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, 2743.3399999999965)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, 2841.1399999999967)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, 2841.1399999999967)]
        [TestCase(TestDatabaseName.TwoSecCur, Totals.Security, 942.42399390640803)]
        public void TotalProfitTests(TestDatabaseName databaseName, Totals totals, decimal expectedValue)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            Assert.That(portfolio.TotalProfit(totals), Is.EqualTo(expectedValue).Within(1e-12m));
        }
    }
}
