using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database
{
    public sealed class ExistsTests
    {
        [Test]
        public void SecurityExists()
        {
            string company = "Company";
            string name = "Name";
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(company, name);

            Portfolio portfolio = constructor.Database;

            bool exists = portfolio.Exists(Account.Security, new TwoName(company, name));

            Assert.That(exists, Is.EqualTo(true));
        }

        [Test]
        public void SecurityDoesntExist()
        {
            string company = "Company";
            string name = "Name";
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(company, name);

            Portfolio portfolio = constructor.Database;

            bool exists = portfolio.Exists(Account.Security, new TwoName("Man", name));

            Assert.That(exists, Is.EqualTo(false));
        }

        [Test]
        public void SectorExists()
        {
            string company = "Company";
            string name = "Name";
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(company, name);

            Portfolio portfolio = constructor.Database;

            bool exists = portfolio.Exists(Account.Benchmark, new TwoName(company, name));

            Assert.That(exists, Is.EqualTo(true));
        }

        [Test]
        public void SectorDoesntExist()
        {
            string company = "Company";
            string name = "Name";
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(company, name);

            Portfolio portfolio = constructor.Database;

            bool exists = portfolio.Exists(Account.Benchmark, new TwoName("Man", name));

            Assert.That(exists, Is.EqualTo(false));
        }
    }
}
