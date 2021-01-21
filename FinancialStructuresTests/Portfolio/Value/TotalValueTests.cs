using System;
using FinancialStructures.Database;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Value
{
    public sealed class TotalValueTests
    {
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, 26084.099999999999)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, 26084.099999999999)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, 1102.2)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.BankAccount, 1102.2)]
        [TestCase(TestDatabaseName.OneSec, Totals.Security, 556.04999999999995)]
        [TestCase(TestDatabaseName.OneSecOneBank, Totals.Security, 556.04999999999995)]
        [TestCase(TestDatabaseName.OneBank, Totals.BankAccount, 101.1)]
        [TestCase(TestDatabaseName.OneSecOneBank, Totals.BankAccount, 101.1)]
        public void LatestTotalValueTests(TestDatabaseName databaseName, Totals totals, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.TotalValue(totals));
        }

        [TestCase(TestDatabaseName.TwoSec, Totals.Security, "2009/1/2", 0.0)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, "2010/1/1", 200.0)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, "2010/1/2", 200.0)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, "2015/1/2", 18939.369577246791)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, "2018/1/1", 31652.961158669885)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, "2018/5/5", 33799.402885014038)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, "2018/5/6", 33948.80289893617)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, "2018/5/7", 33966.379079831197)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, "2020/5/1", 26084.099999999999)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2009/1/2", 0.0)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2010/1/1", 200.0)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2010/1/2", 200.0)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2015/1/2", 18939.369577246791)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2018/1/1", 31652.961158669885)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2018/5/5", 33799.402885014038)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2018/5/6", 33948.80289893617)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2018/5/7", 33966.379079831197)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2020/5/1", 26084.099999999999)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, "2009/1/2", 0.0)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, "2010/1/1", 1200.0)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, "2010/1/2", 1200.0)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, "2015/1/2", 1250.4000000000001)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, "2018/1/1", 991.20000000000005)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, "2018/5/5", 991.20000000000005)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, "2018/5/6", 848.40000000000009)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, "2018/5/7", 848.40000000000009)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, "2020/5/1", 1102.2)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, "2010/1/1", 1400)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, "2015/5/1", 18220.028512967812)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, "2010/5/1", 3479.1836734693879)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, "2020/1/1", 27186.299999999999)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, "2018/10/23", 37785.153651090215)]
        public void TotalValueTest(TestDatabaseName databaseName, Totals totals, DateTime date, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.TotalValue(totals, date));
        }

        [TestCase(TestDatabaseName.OneSec, Account.Security, Totals.Security, "2009/1/2", 0.0)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, Totals.Security, "2010/1/1", 200.0)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, Totals.Security, "2010/1/2", 200.0)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, Totals.Security, "2015/1/2", 1618.4304029990628)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, Totals.Security, "2018/1/1", 316.51302037201066)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, Totals.Security, "2018/5/5", 310.84570416297606)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, Totals.Security, "2018/5/6", 442.89000000000004)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, Totals.Security, "2018/5/7", 443.11046280991741)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, Totals.Security, "2020/5/1", 556.04999999999995)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, Totals.BankAccount, "2010/1/1", 100.0)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, Totals.BankAccount, "2010/1/2", 100.0)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, Totals.BankAccount, "2015/1/2", 125.2)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, Totals.BankAccount, "2018/1/1", 90.599999999999994)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, Totals.BankAccount, "2018/5/5", 90.599999999999994)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, Totals.BankAccount, "2018/5/6", 77.7)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, Totals.BankAccount, "2018/5/7", 77.700000000000003)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, Totals.BankAccount, "2020/5/1", 101.09999999999999)]
        public void TotalValueAndSingleValueAgreeTest(TestDatabaseName databaseName, Account account, Totals totals, DateTime date, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.Value(account, TestDatabase.Name(account, NameOrder.Default), date), "Value not correct");
            Assert.AreEqual(expectedValue, portfolio.TotalValue(totals, date), "TotalValue not correct.");
        }
    }
}
