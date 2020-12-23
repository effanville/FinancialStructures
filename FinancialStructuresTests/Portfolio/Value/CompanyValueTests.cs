using System;
using FinancialStructures.Database;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Value
{
    [TestFixture]
    public sealed class CompanyValueTests
    {
        [TestCase(Totals.SecurityCompany, 556.04999999999995)]
        [TestCase(Totals.BankAccountCompany, 101.1)]
        public void LatestCompanyValueOneAccountTests(Totals totalsType, double expectedValue)
        {
            var accountType = AccountToTotalsConverter.ConvertTotalToAccount(totalsType);
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultFromType(accountType);
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.TotalValue(totalsType, DateTime.Today, constructor.DefaultNameQuery(accountType)));
        }

        [TestCase(Totals.SecurityCompany, 556.04999999999995)]
        [TestCase(Totals.BankAccountCompany, 101.1)]
        public void LatestCompanyValueTwoAccountTests(Totals totalsType, double expectedValue)
        {
            var accountType = AccountToTotalsConverter.ConvertTotalToAccount(totalsType);
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultFromType(accountType);
            constructor.WithSecondaryFromType(accountType);
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.TotalValue(totalsType, DateTime.Today, constructor.DefaultNameQuery(accountType)));
        }

        [TestCase(Totals.SecurityCompany, 5556.04999999999995)]
        [TestCase(Totals.BankAccountCompany, 201.1)]
        public void LatestCompanyValueAccountTests(Totals totalsType, double expectedValue)
        {
            var accountType = AccountToTotalsConverter.ConvertTotalToAccount(totalsType);
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultFromType(accountType);
            constructor.WithSecondaryFromType(accountType);
            var defaultName = constructor.DefaultNameQuery(accountType);
            constructor.WithAccountFromNameAndData(accountType, defaultName.Company, defaultName.Name, dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new double[] { 50 }, numberUnits: new double[] { 100 }, investment: new double[] { 0 });
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.TotalValue(totalsType, DateTime.Today, defaultName));
        }
    }
}
