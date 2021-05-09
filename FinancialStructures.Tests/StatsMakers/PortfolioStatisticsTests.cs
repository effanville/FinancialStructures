﻿using System;
using System.Linq;
using FinancialStructures.DataExporters;
using FinancialStructures.DataExporters.ExportOptions;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.StatsMakers
{
    [TestFixture]
    public sealed class PortfolioStatisticsTests
    {
        [Test]
        public void CanGenerateWithSingleDataValues()
        {
            var generator = new DatabaseConstructor();
            string secCompany = "company1";
            generator.WithSecurityFromNameAndDataPoint(secCompany, "name1", date: new DateTime(2000, 1, 1), sharePrice: 101, numberUnits: 12);

            string bankCompany = "Bank";
            generator.WithBankAccountFromNameAndDataPoint(bankCompany, "AccountName", date: new DateTime(2000, 1, 1), value: 53);
            var database = generator.database;
            var stats = new PortfolioStatistics(database, new UserDisplayOptions());

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