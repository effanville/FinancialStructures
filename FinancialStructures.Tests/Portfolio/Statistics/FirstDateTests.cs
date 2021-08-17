﻿using System;
using FinancialStructures.Database.Statistics;
using NUnit.Framework;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;

namespace FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class FirstDateTests
    {
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, null, "1/1/2010")]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, null, "1/1/2010")]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.BankAccount, null, "1/1/2010")]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.SecurityCompany, SecurityConstructor.DefaultCompany, "1/1/2010")]
        public void FirstValueDateTests(TestDatabaseName databaseName, Totals totals, string companyName, DateTime expectedDate)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedDate, portfolio.FirstValueDate(totals, new TwoName(companyName)));
        }
    }
}
