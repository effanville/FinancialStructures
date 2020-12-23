using System;
using FinancialStructures.Database.Statistics;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;
using FinancialStructures.Database;

namespace FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class StatisticsTests
    {
        [Test]
        public void FirstValueDateTests()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithDefaultSecurity();
            constructor.WithSecondaryBankAccount();
            constructor.WithSecondarySecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(new DateTime(2010, 1, 1), portfolio.FirstValueDate(Totals.All));

            Assert.AreEqual(new DateTime(2010, 1, 1), portfolio.FirstValueDate(Totals.Security));
            Assert.AreEqual(new DateTime(2010, 1, 1), portfolio.FirstValueDate(Totals.BankAccount));
        }

        [Test]
        public void CompanyFirstDateTests()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithDefaultSecurity();
            constructor.WithSecondaryBankAccount();
            constructor.WithSecondarySecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(new DateTime(2010, 1, 1), portfolio.FirstValueDate(Totals.SecurityCompany, DatabaseConstructor.DefaultSecurityCompany));
        }
    }
}
