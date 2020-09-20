using System;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Value
{
    [TestFixture]
    public sealed class CompanyValueTests
    {
        [TestCase(AccountType.Security, 556.04999999999995)]
        [TestCase(AccountType.BankAccount, 101.1)]
        public void LatestCompanyValueOneAccountTests(AccountType accountType, double expectedValue)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultFromType(accountType);
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.CompanyValue(accountType, constructor.DefaultNameQuery(accountType).Company, DateTime.Today));
        }

        [TestCase(AccountType.Security, 556.04999999999995)]
        [TestCase(AccountType.BankAccount, 101.1)]
        public void LatestCompanyValueTwoAccountTests(AccountType accountType, double expectedValue)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultFromType(accountType);
            constructor.WithSecondaryFromType(accountType);
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.CompanyValue(accountType, constructor.DefaultNameQuery(accountType).Company, DateTime.Today));
        }

        [TestCase(AccountType.Security, 5556.04999999999995)]
        [TestCase(AccountType.BankAccount, 201.1)]
        public void LatestCompanyValueAccountTests(AccountType accountType, double expectedValue)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultFromType(accountType);
            constructor.WithSecondaryFromType(accountType);
            var defaultName = constructor.DefaultNameQuery(accountType);
            constructor.WithAccountFromNameAndData(accountType, defaultName.Company, defaultName.Name, dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new double[] { 50 }, numberUnits: new double[] { 100 }, investment: new double[] { 0 });
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.CompanyValue(accountType, defaultName.Company, DateTime.Today));
        }
    }
}
