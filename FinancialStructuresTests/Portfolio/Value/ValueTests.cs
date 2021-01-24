using System;
using FinancialStructures.Database;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Value
{
    [TestFixture]
    public sealed class ValueTests
    {
        [TestCase(TestDatabaseName.OneSec, Account.Security, NameOrder.Default, 556.04999999999995)]
        [TestCase(TestDatabaseName.OneSec, Account.BankAccount, NameOrder.Default, double.NaN)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, NameOrder.Default, 101.1)]
        [TestCase(TestDatabaseName.OneBank, Account.Security, NameOrder.Default, double.NaN)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.Security, NameOrder.Default, 556.04999999999995)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.BankAccount, NameOrder.Default, 101.1)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, NameOrder.Default, 101.1)]
        [TestCase(TestDatabaseName.OneBank, Account.Security, NameOrder.Default, double.NaN)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, NameOrder.Default, 556.04999999999995)]
        [TestCase(TestDatabaseName.TwoSec, Account.BankAccount, NameOrder.Default, double.NaN)]
        [TestCase(TestDatabaseName.TwoBank, Account.BankAccount, NameOrder.Default, 101.1)]
        [TestCase(TestDatabaseName.TwoBank, Account.Security, NameOrder.Default, double.NaN)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.BankAccount, NameOrder.Secondary, 96.799388079399137)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.Security, NameOrder.Secondary, double.NaN)]
        [TestCase(TestDatabaseName.TwoSecCur, Account.BankAccount, NameOrder.Secondary, double.NaN)]
        [TestCase(TestDatabaseName.TwoSecCur, Account.Security, NameOrder.Secondary, 2519.6185349999996)]
        [TestCase(TestDatabaseName.TwoSecTwoBankCur, Account.BankAccount, NameOrder.Secondary, 96.799388079399137)]
        [TestCase(TestDatabaseName.TwoSecTwoBankCur, Account.Security, NameOrder.Secondary, 2519.6185349999996)]
        public void LatestValueTests(TestDatabaseName databaseName, Account account, NameOrder order, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.LatestValue(account, TestDatabase.Name(account, order)));
        }

        [TestCase(TestDatabaseName.OneSec, Account.Security, "2009/1/2", 0.0)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, "2010/1/1", 200.0)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, "2010/1/2", 200.0)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, "2015/1/2", 1618.4304029990628)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, "2018/1/1", 316.51302037201066)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, "2018/5/5", 310.84570416297606)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, "2018/5/6", 442.89000000000004)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, "2018/5/7", 443.11046280991741)]
        [TestCase(TestDatabaseName.OneSec, Account.Security, "2020/5/1", 556.04999999999995)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, "2009/1/2", 0.0)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, "2010/1/1", 200.0)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, "2010/1/2", 200.0)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, "2015/1/2", 1618.4304029990628)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, "2018/1/1", 316.51302037201066)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, "2018/5/5", 310.84570416297606)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, "2018/5/6", 442.89000000000004)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, "2018/5/7", 443.11046280991741)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, "2020/5/1", 556.04999999999995)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.Security, "2009/1/2", 0.0)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.Security, "2010/1/1", 200.0)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.Security, "2010/1/2", 200.0)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.Security, "2015/1/2", 1618.4304029990628)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.Security, "2018/1/1", 316.51302037201066)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.Security, "2018/5/5", 310.84570416297606)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.Security, "2018/5/6", 442.89000000000004)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.Security, "2018/5/7", 443.11046280991741)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.Security, "2020/5/1", 556.04999999999995)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, "2009/1/2", 0.0)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, "2010/1/1", 100.0)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, "2010/1/2", 100.0)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, "2015/1/2", 125.2)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, "2018/1/1", 90.599999999999994)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, "2018/5/5", 90.599999999999994)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, "2018/5/6", 77.700000000000003)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, "2018/5/7", 77.700000000000003)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, "2020/5/1", 101.09999999999999)]
        [TestCase(TestDatabaseName.TwoBank, Account.BankAccount, "2009/1/2", 0.0)]
        [TestCase(TestDatabaseName.TwoBank, Account.BankAccount, "2010/1/1", 100.0)]
        [TestCase(TestDatabaseName.TwoBank, Account.BankAccount, "2010/1/2", 100.0)]
        [TestCase(TestDatabaseName.TwoBank, Account.BankAccount, "2015/1/2", 125.2)]
        [TestCase(TestDatabaseName.TwoBank, Account.BankAccount, "2018/1/1", 90.599999999999994)]
        [TestCase(TestDatabaseName.TwoBank, Account.BankAccount, "2018/5/5", 90.599999999999994)]
        [TestCase(TestDatabaseName.TwoBank, Account.BankAccount, "2018/5/6", 77.700000000000003)]
        [TestCase(TestDatabaseName.TwoBank, Account.BankAccount, "2018/5/7", 77.700000000000003)]
        [TestCase(TestDatabaseName.TwoBank, Account.BankAccount, "2020/5/1", 101.09999999999999)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.BankAccount, "2009/1/2", 0.0)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.BankAccount, "2010/1/1", 100.0)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.BankAccount, "2010/1/2", 100.0)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.BankAccount, "2015/1/2", 125.2)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.BankAccount, "2018/1/1", 90.599999999999994)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.BankAccount, "2018/5/5", 90.599999999999994)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.BankAccount, "2018/5/6", 77.700000000000003)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.BankAccount, "2018/5/7", 77.700000000000003)]
        [TestCase(TestDatabaseName.OneSecOneBank, Account.BankAccount, "2020/5/1", 101.09999999999999)]
        public void ValueTest(TestDatabaseName databaseName, Account account, DateTime date, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.Value(account, TestDatabase.Name(account, NameOrder.Default), date));
        }

        [TestCase(TestDatabaseName.TwoBankCur, Account.BankAccount, NameOrder.Secondary, "2009/1/2", 0.0)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.BankAccount, NameOrder.Secondary, "2010/1/1", 89.100000000000009)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.BankAccount, NameOrder.Secondary, "2010/1/2", 89.100000000000009)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.BankAccount, NameOrder.Secondary, "2015/1/2", 91.954561694616075)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.BankAccount, NameOrder.Secondary, "2018/1/1", 77.416227802294799)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.BankAccount, NameOrder.Secondary, "2018/5/5", 77.416227802294799)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.BankAccount, NameOrder.Secondary, "2018/5/6", 70.168761888412021)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.BankAccount, NameOrder.Secondary, "2018/5/7", 70.168761888412021)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.BankAccount, NameOrder.Secondary, "2020/5/1", 96.799388079399137)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.Security, NameOrder.Secondary, "2012/5/2",double.NaN)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.Security, NameOrder.Secondary, "2020/5/12", double.NaN)]
        [TestCase(TestDatabaseName.TwoSecCur, Account.BankAccount, NameOrder.Secondary, "2012/5/2", double.NaN)]
        [TestCase(TestDatabaseName.TwoSecCur, Account.BankAccount, NameOrder.Secondary,"2020/4/12", double.NaN)]
        [TestCase(TestDatabaseName.TwoSecCur, Account.Security, NameOrder.Secondary, "2018/5/4", 3046.7771657689655)]
        [TestCase(TestDatabaseName.TwoSecTwoBankCur, Account.BankAccount, NameOrder.Secondary, "2018/5/4", 77.416227802294799)]
        [TestCase(TestDatabaseName.TwoSecTwoBankCur, Account.Security, NameOrder.Secondary, "2018/5/4", 3046.7771657689655)]
        public void ValueWithCurrencyTest(TestDatabaseName databaseName, Account account, NameOrder order, DateTime date, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.Value(account, TestDatabase.Name(account, order), date));
        }
    }
}
