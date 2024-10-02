using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.DataAccess
{
    [TestFixture]
    public sealed class DataTests
    {
        [Test]
        public void CanDisplaySecurityData()
        {
            DatabaseConstructor generator = new DatabaseConstructor();
            string secCompany = "company1";
            _ = generator.WithSecurity(secCompany, "name1", dates: new[] { new DateTime(2000, 1, 1) }, sharePrice: new[] { 101.0m }, numberUnits: new[] { 12.0m });
            Portfolio database = generator.Database;

            IReadOnlyList<DailyValuation> data = database.NumberData(Account.Security,new TwoName(secCompany, "name1"));

            Assert.That(data.Count, Is.EqualTo(1));
            Assert.That(data.Single().Value, Is.EqualTo(12 * 101));
        }

        [Test]
        public void RetrievesNewListForNoSecurityData()
        {
            DatabaseConstructor generator = new DatabaseConstructor();
            string secCompany = "company1";
            _ = generator.WithSecurity(secCompany, "name1", dates: new[] { new DateTime(2000, 1, 1) }, sharePrice: new[] { 101.0m }, numberUnits: new[] { 12.0m });

            Portfolio database = generator.Database;

            IReadOnlyList<DailyValuation> data = database.NumberData(Account.Security, new TwoName(secCompany, "name"));

            Assert.That(data.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanDisplayBankAccountData()
        {
            DatabaseConstructor generator = new DatabaseConstructor();

            string bankCompany = "Bank";
            _ = generator.WithBankAccount(bankCompany, "AccountName", dates: new[] { new DateTime(2000, 1, 1) }, values: new[] { 53.0m });
            Portfolio database = generator.Database;

            IReadOnlyList<DailyValuation> data = database.NumberData(Account.BankAccount, new NameData(bankCompany, "AccountName"));

            Assert.That(data.Count, Is.EqualTo(1));
            Assert.That(data.Single().Value, Is.EqualTo(53));
        }

        [Test]
        public void RetrievesNewListForNoBankAccountData()
        {
            DatabaseConstructor generator = new DatabaseConstructor();

            string bankCompany = "Bank";
            _ = generator.WithBankAccount(bankCompany, "AccountName", dates: new[] { new DateTime(2000, 1, 1) }, values: new[] { 53.0m });
            Portfolio database = generator.Database;

            IReadOnlyList<DailyValuation> data = database.NumberData(Account.BankAccount, new NameData(bankCompany, "name"));

            Assert.That(data.Count, Is.EqualTo(0));
        }
    }
}
