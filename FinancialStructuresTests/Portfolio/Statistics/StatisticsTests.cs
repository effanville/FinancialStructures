using System;
using FinancialStructures.Database.Statistics;
using FinancialStructures.Tests.TestDatabaseConstructor;
using FinancialStructures.FinanceInterfaces;
using NUnit.Framework;

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
            Assert.AreEqual(new DateTime(2010, 1, 1), portfolio.FirstValueDate(Account.All));

            Assert.AreEqual(new DateTime(2010, 1, 1), portfolio.FirstValueDate(Account.Security));
            Assert.AreEqual(new DateTime(2010, 1, 1), portfolio.FirstValueDate(Account.BankAccount));
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
            Assert.AreEqual(new DateTime(2010, 1, 1), portfolio.CompanyFirstDate(DatabaseConstructor.DefaultSecurityCompany));
        }
    }
}
