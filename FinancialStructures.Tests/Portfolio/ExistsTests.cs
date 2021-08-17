using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database
{
    public sealed class ExistsTests
    {
        [Test]
        public void SecurityExists()
        {
            string company = "Company";
            string name = "Name";
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(company, name);

            var portfolio = constructor.Database;

            bool exists = portfolio.Exists(Account.Security, new TwoName(company, name));

            Assert.AreEqual(true, exists);
        }

        [Test]
        public void SecurityDoesntExist()
        {
            string company = "Company";
            string name = "Name";
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(company, name);

            var portfolio = constructor.Database;

            bool exists = portfolio.Exists(Account.Security, new TwoName("Man", name));

            Assert.AreEqual(false, exists);
        }

        [Test]
        public void SectorExists()
        {
            string company = "Company";
            string name = "Name";
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(company, name);

            var portfolio = constructor.Database;

            bool exists = portfolio.Exists(Account.Benchmark, new TwoName(company, name));

            Assert.AreEqual(true, exists);
        }

        [Test]
        public void SectorDoesntExist()
        {
            string company = "Company";
            string name = "Name";
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(company, name);

            var portfolio = constructor.Database;

            bool exists = portfolio.Exists(Account.Benchmark, new TwoName("Man", name));

            Assert.AreEqual(false, exists);
        }
    }
}
