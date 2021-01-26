using System;
using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class FractionTests
    {
        private static IEnumerable<TestCaseData> FractionTestCases()
        {
            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.All, TestDatabase.Name(Account.BankAccount, NameOrder.Default), new DateTime(), 1.0);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.Security, TestDatabase.Name(Account.BankAccount, NameOrder.Default), new DateTime(), 0.0);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.BankAccount, TestDatabase.Name(Account.BankAccount, NameOrder.Default), new DateTime(), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.All, TestDatabase.Name(Account.BankAccount, NameOrder.Default), new DateTime(), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.Security, TestDatabase.Name(Account.BankAccount, NameOrder.Default), new DateTime(), 0.0);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.Security, TestDatabase.Name(Account.Security, NameOrder.Default), new DateTime(), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, 26081.099999999999);
            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.Security, 3023.2716572779873);
        }

        [TestCaseSource(nameof(FractionTestCases))]
        public void FracionTests(TestDatabaseName databaseName, Totals totals, TwoName name, DateTime date, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.Fraction(totals, name, date));
        }
    }
}
