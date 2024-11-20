using System.Collections.Generic;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.DataAccess
{
    [TestFixture]
    public sealed class NameTests
    {
        private DatabaseConstructor CreateThreeAccounts(Account elementType, string company1, string name1, string company2, string name2, string company3, string name3)
        {
            switch (elementType)
            {
                case Account.Security:
                {
                    DatabaseConstructor constructor = new DatabaseConstructor();
                    _ = constructor.WithSecurity(company1, name1);
                    _ = constructor.WithSecurity(company2, name2);
                    _ = constructor.WithSecurity(company3, name3);
                    return constructor;
                }
                case Account.Benchmark:
                {
                    DatabaseConstructor constructor = new DatabaseConstructor();
                    _ = constructor.WithSectorFromName(company1, name1);
                    _ = constructor.WithSectorFromName(company2, name2);
                    _ = constructor.WithSectorFromName(company3, name3);
                    return constructor;
                }
                case Account.BankAccount:
                {
                    DatabaseConstructor constructor = new DatabaseConstructor();
                    _ = constructor.WithBankAccount(company1, name1);
                    _ = constructor.WithBankAccount(company2, name2);
                    _ = constructor.WithBankAccount(company3, name3);
                    return constructor;
                }
                case Account.Currency:
                {
                    DatabaseConstructor constructor = new DatabaseConstructor();
                    _ = constructor.WithCurrency(company1, name1);
                    _ = constructor.WithCurrency(company2, name2);
                    _ = constructor.WithCurrency(company3, name3);
                    return constructor;
                }
                default:
                    return null;
            }
        }

        [TestCase(Account.Security, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.Benchmark, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.BankAccount, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.Currency, "company1", "name1", "company2", "name2", "company3", "name3")]
        public void NameDataTests(Account elementType, string company1, string name1, string company2, string name2, string company3, string name3)
        {
            DatabaseConstructor constructor = CreateThreeAccounts(elementType, company1, name1, company2, name2, company3, name3);
            Portfolio database = constructor.Database;

            IReadOnlyList<NameData> names = database.NameDataForAccount(elementType);
            Assert.That(names.Count, Is.EqualTo(3));

            Assert.That(names[0].Company, Is.EqualTo(company1));
            Assert.That(names[1].Company, Is.EqualTo(company2));
            Assert.That(names[2].Company, Is.EqualTo(company3));

            Assert.That(names[0].Name, Is.EqualTo(name1));
            Assert.That(names[1].Name, Is.EqualTo(name2));
            Assert.That(names[2].Name, Is.EqualTo(name3));
        }

        [TestCase(Account.Security, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.Benchmark, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.BankAccount, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.Currency, "company1", "name1", "company2", "name2", "company3", "name3")]
        public void NamesTests(Account elementType, string company1, string name1, string company2, string name2, string company3, string name3)
        {
            DatabaseConstructor constructor = CreateThreeAccounts(elementType, company1, name1, company2, name2, company3, name3);
            Portfolio database = constructor.Database;

            IReadOnlyList<string> names = database.Names(elementType);
            Assert.That(names.Count, Is.EqualTo(3));

            Assert.That(names[0], Is.EqualTo(name1));
            Assert.That(names[1], Is.EqualTo(name2));
            Assert.That(names[2], Is.EqualTo(name3));
        }

        [TestCase(Account.Security, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.Benchmark, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.BankAccount, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.Currency, "company1", "name1", "company2", "name2", "company3", "name3")]
        public void CompaniesTests(Account elementType, string company1, string name1, string company2, string name2, string company3, string name3)
        {
            DatabaseConstructor constructor = CreateThreeAccounts(elementType, company1, name1, company2, name2, company3, name3);
            Portfolio database = constructor.Database;

            IReadOnlyList<string> names = database.Companies(elementType);
            Assert.That(names.Count, Is.EqualTo(3));

            Assert.That(names[0], Is.EqualTo(company1));
            Assert.That(names[1], Is.EqualTo(company2));
            Assert.That(names[2], Is.EqualTo(company3));
        }
    }
}
