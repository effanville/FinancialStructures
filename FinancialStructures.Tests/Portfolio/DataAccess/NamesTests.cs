using FinancialStructures.Database;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.DataAccess
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
                    var constructor = new DatabaseConstructor();
                    _ = constructor.WithSecurity(company1, name1);
                    _ = constructor.WithSecurity(company2, name2);
                    _ = constructor.WithSecurity(company3, name3);
                    return constructor;
                }
                case Account.Benchmark:
                {
                    var constructor = new DatabaseConstructor();
                    _ = constructor.WithSectorFromName(company1, name1);
                    _ = constructor.WithSectorFromName(company2, name2);
                    _ = constructor.WithSectorFromName(company3, name3);
                    return constructor;
                }
                case Account.BankAccount:
                {
                    var constructor = new DatabaseConstructor();
                    _ = constructor.WithBankAccount(company1, name1);
                    _ = constructor.WithBankAccount(company2, name2);
                    _ = constructor.WithBankAccount(company3, name3);
                    return constructor;
                }
                case Account.Currency:
                {
                    var constructor = new DatabaseConstructor();
                    _ = constructor.WithCurrencyFromName(company1, name1);
                    _ = constructor.WithCurrencyFromName(company2, name2);
                    _ = constructor.WithCurrencyFromName(company3, name3);
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
            var constructor = CreateThreeAccounts(elementType, company1, name1, company2, name2, company3, name3);
            var database = constructor.Database;

            var names = database.NameData(elementType);
            Assert.AreEqual(3, names.Count);

            Assert.AreEqual(company1, names[0].Company);
            Assert.AreEqual(company2, names[1].Company);
            Assert.AreEqual(company3, names[2].Company);

            Assert.AreEqual(name1, names[0].Name);
            Assert.AreEqual(name2, names[1].Name);
            Assert.AreEqual(name3, names[2].Name);
        }

        [TestCase(Account.Security, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.Benchmark, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.BankAccount, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.Currency, "company1", "name1", "company2", "name2", "company3", "name3")]
        public void NamesTests(Account elementType, string company1, string name1, string company2, string name2, string company3, string name3)
        {
            var constructor = CreateThreeAccounts(elementType, company1, name1, company2, name2, company3, name3);
            var database = constructor.Database;

            var names = database.Names(elementType);
            Assert.AreEqual(3, names.Count);

            Assert.AreEqual(name1, names[0]);
            Assert.AreEqual(name2, names[1]);
            Assert.AreEqual(name3, names[2]);
        }

        [TestCase(Account.Security, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.Benchmark, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.BankAccount, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(Account.Currency, "company1", "name1", "company2", "name2", "company3", "name3")]
        public void CompaniesTests(Account elementType, string company1, string name1, string company2, string name2, string company3, string name3)
        {
            var constructor = CreateThreeAccounts(elementType, company1, name1, company2, name2, company3, name3);
            var database = constructor.Database;

            var names = database.Companies(elementType);
            Assert.AreEqual(3, names.Count);

            Assert.AreEqual(company1, names[0]);
            Assert.AreEqual(company2, names[1]);
            Assert.AreEqual(company3, names[2]);
        }
    }
}
