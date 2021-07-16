using System;
using FinancialStructures.Database;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Value
{
    [TestFixture]
    public sealed class CompanyValueTests
    {
        [TestCase(TestDatabaseName.OneSec, Totals.SecurityCompany, 556.04999999999995)]
        [TestCase(TestDatabaseName.OneBank, Totals.BankAccountCompany, 101.1)]
        [TestCase(TestDatabaseName.OneSecOneBank, Totals.SecurityCompany, 556.04999999999995)]
        [TestCase(TestDatabaseName.OneSecOneBank, Totals.BankAccountCompany, 101.1)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.SecurityCompany, 556.04999999999995)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.BankAccountCompany, 101.1)]
        public void LatestCompanyValueTests(TestDatabaseName databaseName, Totals totalsType, double expectedValue)
        {
            var accountType = totalsType.ToAccount();
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.TotalValue(totalsType, DateTime.Today, TestDatabase.Name(accountType, NameOrder.Default)));
        }

        [TestCase(Totals.SecurityCompany, 5556.04999999999995)]
        [TestCase(Totals.BankAccountCompany, 201.1)]
        public void LatestCompanyValueAccountTests(Totals totalsType, double expectedValue)
        {
            var accountType = totalsType.ToAccount();
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultFromType(accountType);
            constructor.WithSecondaryFromType(accountType);
            var defaultName = constructor.DefaultName(accountType);
            constructor.WithAccountFromNameAndData(accountType, defaultName.Company, defaultName.Name, dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new double[] { 50 }, numberUnits: new double[] { 100 }, investment: new double[] { 0 });
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.TotalValue(totalsType, DateTime.Today, defaultName));
        }
    }
}
