using System;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Value
{
    [TestFixture]
    public sealed class ValueTests
    {
        [TestCase(Account.Security, 556.04999999999995)]
        [TestCase(Account.BankAccount, 101.1)]
        public void LatestValueTests(Account accountType, double expectedValue)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultFromType(accountType);
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.LatestValue(accountType, constructor.DefaultNameQuery(accountType)));

        }

        [TestCase("2009/1/2", 0.0)]
        [TestCase("2010/1/1", 200.0)]
        [TestCase("2010/1/2", 200.0)]
        [TestCase("2015/1/2", 1618.4304029990628)]
        [TestCase("2018/1/1", 316.51302037201066)]
        [TestCase("2018/5/5", 310.84570416297606)]
        [TestCase("2018/5/6", 442.89000000000004)]
        [TestCase("2018/5/7", 443.11046280991741)]
        [TestCase("2020/5/1", 556.04999999999995)]
        public void ValueTest(DateTime date, double expectedValue)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultSecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.Value(Account.Security, constructor.DefaultNameQuery(Account.Security), date));
        }

        [TestCase("2009/1/2", 100.0)]
        [TestCase("2010/1/1", 100.0)]
        [TestCase("2010/1/2", 100.0)]
        [TestCase("2015/1/2", 93.550890346766636)]
        [TestCase("2018/1/1", 79.128255093002664)]
        [TestCase("2018/5/5", 77.711426040744016)]
        [TestCase("2018/5/6", 77.700000000000003)]
        [TestCase("2018/5/7", 77.738677685950421)]
        [TestCase("2020/5/1", 101.09999999999999)]
        public void BankAccountValuesTest(DateTime date, double expectedValue)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.Value(Account.BankAccount, constructor.DefaultNameQuery(Account.BankAccount), date));
        }
    }
}
