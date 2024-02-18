using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.DataAccess
{
    [TestFixture]
    public sealed class TryGetAccountTests
    {
        [Test]
        public void TryGetSecurity()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity("Company", "name");

            global::FinancialStructures.Database.Implementation.Portfolio portfolio = constructor.Database;

            bool result = portfolio.TryGetAccount(Account.Security, new TwoName("Company", "name"), out global::FinancialStructures.FinanceStructures.IValueList desired);

            Assert.AreEqual(true, result);
            Assert.IsNotNull(desired);
            Assert.AreEqual("Company", desired.Names.Company);
            Assert.AreEqual("name", desired.Names.Name);
        }

        [Test]
        public void TryGetNoSecurity()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity("Company", "name");

            global::FinancialStructures.Database.Implementation.Portfolio portfolio = constructor.Database;

            bool result = portfolio.TryGetAccount(Account.Security, new TwoName("Company", "NewName"), out global::FinancialStructures.FinanceStructures.IValueList desired);

            Assert.AreEqual(false, result);
            Assert.IsNull(desired);
        }

        [Test]
        public void TryGetSector()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName("Company", "name");

            global::FinancialStructures.Database.Implementation.Portfolio portfolio = constructor.Database;

            bool result = portfolio.TryGetAccount(Account.Benchmark, new TwoName("Company", "name"), out global::FinancialStructures.FinanceStructures.IValueList desired);

            Assert.AreEqual(true, result);
            Assert.IsNotNull(desired);
            Assert.AreEqual("Company", desired.Names.Company);
            Assert.AreEqual("name", desired.Names.Name);
        }

        [Test]
        public void TryGetNoSector()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName("Company", "name");

            global::FinancialStructures.Database.Implementation.Portfolio portfolio = constructor.Database;

            bool result = portfolio.TryGetAccount(Account.Benchmark, new TwoName("NewCompany", "NewName"), out global::FinancialStructures.FinanceStructures.IValueList desired);

            Assert.AreEqual(false, result);
            Assert.IsNull(desired);
        }
    }
}
