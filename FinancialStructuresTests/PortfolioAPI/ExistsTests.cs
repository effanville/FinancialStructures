using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.PortfolioAPI
{
    public sealed class ExistsTests
    {
        [Test]
        public void SecurityExists()
        {
            string company = "Company";
            string name = "Name";
            var constructor = new DatabaseConstructor();
            constructor.WithSecurityFromName(company, name);

            var portfolio = constructor.database;

            bool exists = portfolio.Exists(AccountType.Security, new TwoName(company, name));

            Assert.AreEqual(true, exists);
        }

        [Test]
        public void SecurityDoesntExist()
        {
            string company = "Company";
            string name = "Name";
            var constructor = new DatabaseConstructor();
            constructor.WithSecurityFromName(company, name);

            var portfolio = constructor.database;

            bool exists = portfolio.Exists(AccountType.Security, new TwoName("Man", name));

            Assert.AreEqual(false, exists);
        }

        [Test]
        public void SectorExists()
        {
            string company = "Company";
            string name = "Name";
            var constructor = new DatabaseConstructor();
            constructor.WithSectorFromName(company, name);

            var portfolio = constructor.database;

            bool exists = portfolio.Exists(AccountType.Benchmark, new TwoName(company, name));

            Assert.AreEqual(true, exists);
        }

        [Test]
        public void SectorDoesntExist()
        {
            string company = "Company";
            string name = "Name";
            var constructor = new DatabaseConstructor();
            constructor.WithSectorFromName(company, name);

            var portfolio = constructor.database;

            bool exists = portfolio.Exists(AccountType.Sector, new TwoName("Man", name));

            Assert.AreEqual(false, exists);
        }
    }
}
