using System;
using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class FractionTests
    {
        private static IEnumerable<TestCaseData> FractionTestCases()
        {
            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.All, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.Security, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), double.NaN);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.BankAccount, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.All, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), 0.0049757076150343726);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.Security, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), 0.005262159689187711);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.Security, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), 0.02096634757876778);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), 0.02096634757876778);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.BankAccount, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), double.NaN);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, Account.Security, NameOrder.Secondary, new DateTime(2015, 5, 4), 0.9790336524212323);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.Security, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), 0.19921403831205922);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.Security, Account.Security, NameOrder.Secondary, new DateTime(2015, 5, 4), 0.80078596168794081d);
        }

        [TestCaseSource(nameof(FractionTestCases))]
        public void FractionTest(TestDatabaseName databaseName, Totals totals, Account account, NameOrder order, DateTime date, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            var actual = portfolio.Fraction(totals, account, TestDatabase.Name(account, order), date);
            Assert.AreEqual(expectedValue, actual);
        }

        private static IEnumerable<TestCaseData> TotalFractionTestCases()
        {
            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.All, new TwoName(), new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.Security, new TwoName(), new DateTime(2015, 5, 4), 0.0);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.BankAccount, new TwoName(), new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.All, new TwoName(), new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.Security, new TwoName(), new DateTime(2015, 5, 4), 0.94556378158916032);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Secondary), new DateTime(2015, 5, 4), 0.92573876268646793);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, new TwoName(), new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.BankAccount, new TwoName(), new DateTime(2015, 5, 4), 0.0);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Default), new DateTime(2015, 5, 4), 0.02096634757876778);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Secondary), new DateTime(2015, 5, 4), 0.9790336524212323);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Secondary), new DateTime(2015, 5, 4), 0.80078596168794081);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.Security, new TwoName(), new DateTime(2015, 5, 4), 1.0);
        }

        [TestCaseSource(nameof(TotalFractionTestCases))]
        public void TotalFractionTest(TestDatabaseName databaseName, Totals totals, TwoName names, DateTime date, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            var actual = portfolio.TotalFraction(totals, names, date);
            Assert.AreEqual(expectedValue, actual);
        }
    }
}
