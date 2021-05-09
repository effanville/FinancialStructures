﻿using System;
using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class InvestmentsTests
    {
        private static IEnumerable<TestCaseData> TotalInvestmentsCases()
        {
            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.BankAccount, null);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.Security, new List<DayValue_Named>());

            yield return new TestCaseData(TestDatabaseName.OneSec, Totals.BankAccount, null);

            yield return new TestCaseData(TestDatabaseName.OneSec, Totals.Security, new List<DayValue_Named>() { new DayValue_Named(DatabaseConstructor.DefaultSecurityCompany, DatabaseConstructor.DefaultSecurityName, new DateTime(2010, 1, 1), 100) });

            yield return new TestCaseData(TestDatabaseName.TwoBank, Totals.BankAccount, null);

            yield return new TestCaseData(TestDatabaseName.TwoBank, Totals.Security, new List<DayValue_Named>());

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.BankAccount, null);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, new List<DayValue_Named> { new DayValue_Named(DatabaseConstructor.DefaultSecurityCompany, DatabaseConstructor.DefaultSecurityName, new DateTime(2010, 1, 1), 100), new DayValue_Named(DatabaseConstructor.SecondarySecurityCompany, DatabaseConstructor.SecondarySecurityName, new DateTime(2010, 1, 5), 2020), new DayValue_Named(DatabaseConstructor.SecondarySecurityCompany, DatabaseConstructor.SecondarySecurityName, new DateTime(2012, 5, 5), 21022.96) });
        }

        [TestCaseSource(nameof(TotalInvestmentsCases))]
        public void TotalInvestmentsTests(TestDatabaseName databaseName, Totals totals, List<DayValue_Named> expected)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            var investments = portfolio.TotalInvestments(totals);
            CollectionAssert.AreEqual(expected, investments);
        }

        private static IEnumerable<TestCaseData> InvestmentsCases()
        {
            yield return new TestCaseData(TestDatabaseName.OneBank, Account.BankAccount, TestDatabase.Name(Account.BankAccount, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.OneBank, Account.Security, TestDatabase.Name(Account.Security, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.OneSec, Account.BankAccount, TestDatabase.Name(Account.Security, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.OneSec, Account.Security, TestDatabase.Name(Account.Security, NameOrder.Default), new List<DayValue_Named>() { new DayValue_Named(DatabaseConstructor.DefaultSecurityCompany, DatabaseConstructor.DefaultSecurityName, new DateTime(2010, 1, 1), 100) });

            yield return new TestCaseData(TestDatabaseName.TwoBank, Account.BankAccount, TestDatabase.Name(Account.BankAccount, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.TwoBank, Account.Security, TestDatabase.Name(Account.Security, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Account.BankAccount, TestDatabase.Name(Account.BankAccount, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Account.Security, TestDatabase.Name(Account.Security, NameOrder.Secondary), new List<DayValue_Named> { new DayValue_Named(DatabaseConstructor.SecondarySecurityCompany, DatabaseConstructor.SecondarySecurityName, new DateTime(2010, 1, 5), 2020), new DayValue_Named(DatabaseConstructor.SecondarySecurityCompany, DatabaseConstructor.SecondarySecurityName, new DateTime(2012, 5, 5), 21022.96) });
        }

        [TestCaseSource(nameof(InvestmentsCases))]
        public void InvestmentTests(TestDatabaseName databaseName, Account account, TwoName name, List<DayValue_Named> expected)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            var investments = portfolio.Investments(account, name);
            CollectionAssert.AreEqual(expected, investments);
        }
    }
}