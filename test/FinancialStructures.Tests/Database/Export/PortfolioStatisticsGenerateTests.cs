using System;
using System.IO.Abstractions;
using System.Linq;

using Effanville.FinancialStructures.Database.Export.Statistics;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.Export
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

            Assert.That(stats.SecurityStats.Count, Is.EqualTo(1));
            Assert.That(stats.SecurityStats.First().NameData.Company, Is.EqualTo(secCompany));
            Assert.That(stats.SecurityCompanyStats.Count, Is.EqualTo(1));
            Assert.That(stats.SecurityCompanyStats.First().NameData.Company, Is.EqualTo(secCompany));
            Assert.That(stats.BankAccountStats.Count, Is.EqualTo(1));
            Assert.That(stats.BankAccountStats.First().NameData.Company, Is.EqualTo(bankCompany));
            Assert.That(stats.BankAccountCompanyStats.Count, Is.EqualTo(1));
            Assert.That(stats.BankAccountCompanyStats.First().NameData.Company, Is.EqualTo(bankCompany));
        }
    }
}
