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

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.All, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), 0.0043886111604674429);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.Security, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), 0.0046099493496925678);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.Security, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), 0.018367705675060717);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), 0.018367705675060717);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.BankAccount, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), double.NaN);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, Account.Security, NameOrder.Secondary, new DateTime(2015, 5, 4), 0.98163229432493937);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.Security, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), 0.17855191841350632);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.Security, Account.Security, NameOrder.Secondary, new DateTime(2015, 5, 4), 0.82144808158649374);
        }

        [TestCaseSource(nameof(FractionTestCases))]
        public void FractionTest(TestDatabaseName databaseName, Totals totals, Account account, NameOrder order, DateTime date, double expectedValue)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            double actual = portfolio.Fraction(totals, account, TestDatabase.Name(account, order), date);
            Assert.AreEqual(expectedValue, actual);
        }

        private static IEnumerable<TestCaseData> TotalFractionTestCases()
        {
            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.All, new TwoName(), new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.Security, new TwoName(), new DateTime(2015, 5, 4), 0.0);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.BankAccount, new TwoName(), new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.All, new TwoName(), new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.Security, new TwoName(), new DateTime(2015, 5, 4), 0.95198685008548201);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Secondary), new DateTime(2015, 5, 4), 0.93450103581658372);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, new TwoName(), new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.BankAccount, new TwoName(), new DateTime(2015, 5, 4), 0.0);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Default), new DateTime(2015, 5, 4), 0.018367705675060717);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Secondary), new DateTime(2015, 5, 4), 0.98163229432493937);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Secondary), new DateTime(2015, 5, 4), 0.82144808158649374);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.Security, new TwoName(), new DateTime(2015, 5, 4), 1.0);
        }

        [TestCaseSource(nameof(TotalFractionTestCases))]
        public void TotalFractionTest(TestDatabaseName databaseName, Totals totals, TwoName names, DateTime date, double expectedValue)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            double actual = portfolio.TotalFraction(totals, names, date);
            Assert.AreEqual(expectedValue, actual);
        }
    }
}
