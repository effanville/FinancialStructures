using System;
using System.Linq;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Extensions.DataEdit;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.DataEdit
{
    public sealed class DataDeleteTests
    {
        [Test]
        public void CanDeleteFromSecurity()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity("Company", "Name", dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new[] { 2.0m }, numberUnits: new[] { 100.0m }, investment: new[] { 0.0m });

            Portfolio portfolio = constructor.Database;

            bool success = portfolio.TryDeleteData(Account.Security, new TwoName("Company", "Name"), new DateTime(2010, 1, 1), null);

            Assert.AreEqual(0, portfolio.Funds.Single().Count());

            // This is false as there is no investment value as value 0 is not stored
            Assert.IsFalse(success);
        }

        [Test]
        public void CanDeleteFromSector()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromNameAndData("Company", "Name", date: new DateTime[] { new DateTime(2010, 1, 1) }, value: new[] { 2.0m });

            Portfolio portfolio = constructor.Database;

            bool success = portfolio.TryDeleteData(Account.Benchmark, new NameData("Company", "Name"), new DateTime(2010, 1, 1), null);

            Assert.IsTrue(success);
            Assert.AreEqual(0, portfolio.BenchMarks.Single().Count());
        }
    }
}
