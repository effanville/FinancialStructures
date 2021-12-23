using System;
using System.IO.Abstractions;
using System.Linq;

using FinancialStructures.Database.Export.Statistics;
using FinancialStructures.Tests.TestDatabaseConstructor;

using NUnit.Framework;

namespace FinancialStructures.Tests.DataExporters.Statistics
{
    [TestFixture]
    public sealed class PortfolioStatisticsTests
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

            Assert.AreEqual(1, stats.IndividualSecurityStats.Count);
            Assert.AreEqual(secCompany, stats.IndividualSecurityStats.First().NameData.Company);
            Assert.AreEqual(1, stats.CompanyTotalsStats.Count);
            Assert.AreEqual(secCompany, stats.CompanyTotalsStats.First().NameData.Company);
            Assert.AreEqual(1, stats.BankAccountStats.Count);
            Assert.AreEqual(bankCompany, stats.BankAccountStats.First().NameData.Company);
            Assert.AreEqual(1, stats.BankAccountCompanyStats.Count);
            Assert.AreEqual(bankCompany, stats.BankAccountCompanyStats.First().NameData.Company);
        }
    }
}
