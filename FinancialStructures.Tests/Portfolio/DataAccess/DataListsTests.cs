using System;
using System.Linq;
using FinancialStructures.Database;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.DataAccess
{
    [TestFixture]
    public sealed class DataListsTests
    {
        [Test]
        public void CanRetrieveSecurityData()
        {
            var generator = new DatabaseConstructor();
            string secCompany = "company1";
            _ = generator.WithSecurityFromNameAndDataPoint(secCompany, "name1", date: new DateTime(2000, 1, 1), sharePrice: 101, numberUnits: 12);
            _ = generator.WithSecurityFromNameAndDataPoint("otherCompany", "name1", date: new DateTime(2000, 1, 1), sharePrice: 101, numberUnits: 12);

            var data = generator.database.CompanyAccounts(Account.Security, secCompany);

            Assert.AreEqual(1, data.Count);
            Assert.AreEqual("name1", data.Single().Names.Name);
        }

        [Test]
        public void RetrievesNewListForNoSecurityData()
        {
            var generator = new DatabaseConstructor();
            string secCompany = "company1";
            _ = generator.WithSecurityFromNameAndDataPoint(secCompany, "name1", date: new DateTime(2000, 1, 1), sharePrice: 101, numberUnits: 12);

            var data = generator.database.CompanyAccounts(Account.Security, "other");
            Assert.AreEqual(0, data.Count);
        }

        [Test]
        public void CanDisplayBankAccountData()
        {
            var generator = new DatabaseConstructor();

            string bankCompany = "Bank";
            _ = generator.WithBankAccountFromNameAndDataPoint(bankCompany, "AccountName", date: new DateTime(2000, 1, 1), value: 53);

            var data = generator.database.CompanyAccounts(Account.BankAccount, bankCompany);

            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(bankCompany, data.Single().Names.Company);
        }

        [Test]
        public void RetrievesNewListForNoBankAccountData()
        {
            DatabaseConstructor generator = new DatabaseConstructor();

            string bankCompany = "Bank";
            _ = generator.WithBankAccountFromNameAndDataPoint(bankCompany, "AccountName", date: new DateTime(2000, 1, 1), value: 53);
            var database = generator.database;

            var data = database.CompanyAccounts(Account.BankAccount, "name");

            Assert.AreEqual(0, data.Count);
        }
    }
}
