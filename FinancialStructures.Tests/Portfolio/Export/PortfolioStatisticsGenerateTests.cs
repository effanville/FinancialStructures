using System;
using System.IO.Abstractions;
using System.Linq;

using FinancialStructures.Database.Export.Statistics;
using FinancialStructures.Tests.TestDatabaseConstructor;

using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Export
{
    [TestFixture]
    public sealed class PortfolioStatisticsGenerateTests
    {
        [Test]
        public void CanGenerateWithSingleDataValues()
        {
            DatabaseConstructor generator = new DatabaseConstructor();
            string secCompany = "company1";
            _ = generator.WithSecurity(secCompany, "name1", dates: new[] { new DateTime(2000, 1, 1) }, sharePrice: new[] { 101.0m }, numberUnits: new[] { 12.0m });

            string bankCompany = "Bank";
            _ = generator.WithBankAccount(bankCompany, "AccountName", dates: new[] { new DateTime(2000, 1, 1) }, values: new[] { 53.0m });
            PortfolioStatistics stats = new PortfolioStatistics(generator.Database, PortfolioStatisticsSettings.DefaultSettings(), new FileSystem());

            Assert.AreEqual(1, stats.SecurityStats.Count);
            Assert.AreEqual(secCompany, stats.SecurityStats.First().NameData.Company);
            Assert.AreEqual(1, stats.SecurityCompanyStats.Count);
            Assert.AreEqual(secCompany, stats.SecurityCompanyStats.First().NameData.Company);
            Assert.AreEqual(1, stats.BankAccountStats.Count);
            Assert.AreEqual(bankCompany, stats.BankAccountStats.First().NameData.Company);
            Assert.AreEqual(1, stats.BankAccountCompanyStats.Count);
            Assert.AreEqual(bankCompany, stats.BankAccountCompanyStats.First().NameData.Company);
        }
    }
}
