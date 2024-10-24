﻿using System;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.Value
{
    [TestFixture]
    public sealed class CompanyValueTests
    {
        [TestCase(TestDatabaseName.OneSec, Totals.SecurityCompany, 556.04999999999995)]
        [TestCase(TestDatabaseName.OneBank, Totals.BankAccountCompany, 101.1)]
        [TestCase(TestDatabaseName.OneSecOneBank, Totals.SecurityCompany, 556.04999999999995)]
        [TestCase(TestDatabaseName.OneSecOneBank, Totals.BankAccountCompany, 101.1)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.SecurityCompany, 556.04999999999995)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.BankAccountCompany, 101.1)]
        public void LatestCompanyValueTests(TestDatabaseName databaseName, Totals totalsType, double expectedValue)
        {
            Account accountType = totalsType.ToAccount();
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.TotalValue(totalsType, DateTime.Today, TestDatabase.Name(accountType, NameOrder.Default)));
        }

        [TestCase(Totals.SecurityCompany, 5556.04999999999995)]
        [TestCase(Totals.BankAccountCompany, 201.1)]
        public void LatestCompanyValueAccountTests(Totals totalsType, double expectedValue)
        {
            Account accountType = totalsType.ToAccount();
            DatabaseConstructor constructor = new DatabaseConstructor();
            TwoName defaultName = DatabaseConstructor.DefaultName(accountType);
            _ = constructor.WithDefaultFromType(accountType)
                .WithSecondaryFromType(accountType)
                .WithAccountFromNameAndData(
                    accountType,
                    defaultName.Company,
                    defaultName.Name + "2",
                    dates: new DateTime[] { new DateTime(2010, 1, 1) },
                    sharePrice: new decimal[] { 50 },
                    numberUnits: new decimal[] { 100 },
                    investment: new decimal[] { 0 });
            Portfolio portfolio = constructor.Database;
            Assert.AreEqual(expectedValue, portfolio.TotalValue(totalsType, DateTime.Today, defaultName));
        }
    }
}
