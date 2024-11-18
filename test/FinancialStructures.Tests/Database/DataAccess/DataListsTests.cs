using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.FinanceStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.DataAccess
{
    [TestFixture]
    public sealed class DataListsTests
    {
        [Test]
        public void CanRetrieveSecurityData()
        {
            DatabaseConstructor generator = new DatabaseConstructor();
            string secCompany = "company1";
            _ = generator.WithSecurity(secCompany, "name1", dates: new[] { new DateTime(2000, 1, 1) }, sharePrice: new[] { 101.0m }, numberUnits: new[] { 12.0m });
            _ = generator.WithSecurity("otherCompany", "name1", dates: new[] { new DateTime(2000, 1, 1) }, sharePrice: new[] { 101.0m }, numberUnits: new[] { 12.0m });

            IReadOnlyList<IValueList> data = generator.Database.Accounts(Totals.SecurityCompany, secCompany);

            Assert.That(data.Count, Is.EqualTo(1));
            Assert.That(data.Single().Names.Name, Is.EqualTo("name1"));
        }

        [Test]
        public void RetrievesNewListForNoSecurityData()
        {
            DatabaseConstructor generator = new DatabaseConstructor();
            string secCompany = "company1";
            _ = generator.WithSecurity(secCompany, "name1", dates: new[] { new DateTime(2000, 1, 1) }, sharePrice: new[] { 101.0m }, numberUnits: new[] { 12.0m });

            IReadOnlyList<IValueList> data = generator.Database.Accounts(Totals.SecurityCompany, "other");
            Assert.That(data.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanDisplayBankAccountData()
        {
            DatabaseConstructor generator = new DatabaseConstructor();

            string bankCompany = "Bank";
            _ = generator.WithBankAccount(bankCompany, "AccountName", dates: new[] { new DateTime(2000, 1, 1) }, values: new[] { 53.0m });

            IReadOnlyList<IValueList> data = generator.Database.Accounts(Totals.BankAccountCompany, bankCompany);

            Assert.That(data.Count, Is.EqualTo(1));
            Assert.That(data.Single().Names.Company, Is.EqualTo(bankCompany));
        }

        [Test]
        public void RetrievesNewListForNoBankAccountData()
        {
            DatabaseConstructor generator = new DatabaseConstructor();

            string bankCompany = "Bank";
            _ = generator.WithBankAccount(bankCompany, "AccountName", dates: new[] { new DateTime(2000, 1, 1) }, values: new[] { 53.0m });
            Portfolio database = generator.Database;

            IReadOnlyList<IValueList> data = database.Accounts(Totals.BankAccountCompany, "name");

            Assert.That(data.Count, Is.EqualTo(0));
        }
    }
}
