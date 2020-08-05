using FinancialStructures.PortfolioAPI;
using FinancialStructures_UnitTests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures_UnitTests.PortfolioAPI.DataAccess
{
    [TestFixture]
    public sealed class NameTests
    {
        private DatabaseConstructor CreateThreeAccounts(AccountType elementType, string company1, string name1, string company2, string name2, string company3, string name3)
        {
            switch (elementType)
            {
                case AccountType.Security:
                {
                    var constructor = new DatabaseConstructor();
                    _ = constructor.WithSecurityFromName(company1, name1);
                    _ = constructor.WithSecurityFromName(company2, name2);
                    _ = constructor.WithSecurityFromName(company3, name3);
                    return constructor;
                }
                case AccountType.Sector:
                {
                    var constructor = new DatabaseConstructor();
                    _ = constructor.WithSectorFromName(company1, name1);
                    _ = constructor.WithSectorFromName(company2, name2);
                    _ = constructor.WithSectorFromName(company3, name3);
                    return constructor;
                }
                case AccountType.BankAccount:
                {
                    var constructor = new DatabaseConstructor();
                    _ = constructor.WithBankAccountFromName(company1, name1);
                    _ = constructor.WithBankAccountFromName(company2, name2);
                    _ = constructor.WithBankAccountFromName(company3, name3);
                    return constructor;
                }
                case AccountType.Currency:
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

        [TestCase(AccountType.Security, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(AccountType.Sector, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(AccountType.BankAccount, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(AccountType.Currency, "company1", "name1", "company2", "name2", "company3", "name3")]
        public void NameDataTests(AccountType elementType, string company1, string name1, string company2, string name2, string company3, string name3)
        {
            var constructor = CreateThreeAccounts(elementType, company1, name1, company2, name2, company3, name3);
            var database = constructor.database;

            var names = database.NameData(elementType);
            Assert.AreEqual(3, names.Count);

            Assert.AreEqual(company1, names[0].Company);
            Assert.AreEqual(company2, names[1].Company);
            Assert.AreEqual(company3, names[2].Company);

            Assert.AreEqual(name1, names[0].Name);
            Assert.AreEqual(name2, names[1].Name);
            Assert.AreEqual(name3, names[2].Name);
        }

        [TestCase(AccountType.Security, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(AccountType.Sector, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(AccountType.BankAccount, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(AccountType.Currency, "company1", "name1", "company2", "name2", "company3", "name3")]
        public void NamesTests(AccountType elementType, string company1, string name1, string company2, string name2, string company3, string name3)
        {
            var constructor = CreateThreeAccounts(elementType, company1, name1, company2, name2, company3, name3);
            var database = constructor.database;

            var names = database.Names(elementType);
            Assert.AreEqual(3, names.Count);

            Assert.AreEqual(name1, names[0]);
            Assert.AreEqual(name2, names[1]);
            Assert.AreEqual(name3, names[2]);
        }

        [TestCase(AccountType.Security, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(AccountType.Sector, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(AccountType.BankAccount, "company1", "name1", "company2", "name2", "company3", "name3")]
        [TestCase(AccountType.Currency, "company1", "name1", "company2", "name2", "company3", "name3")]
        public void CompaniesTests(AccountType elementType, string company1, string name1, string company2, string name2, string company3, string name3)
        {
            var constructor = CreateThreeAccounts(elementType, company1, name1, company2, name2, company3, name3);
            var database = constructor.database;

            var names = database.Companies(elementType);
            Assert.AreEqual(3, names.Count);

            Assert.AreEqual(company1, names[0]);
            Assert.AreEqual(company2, names[1]);
            Assert.AreEqual(company3, names[2]);
        }
    }
}
