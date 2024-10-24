﻿using System;
using System.Collections.Generic;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.NamingStructures;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.Tests.TestDatabaseConstructor;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class InvestmentsTests
    {
        private static IEnumerable<TestCaseData> TotalInvestmentsCases()
        {
            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.BankAccount, null);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.Security, new List<Labelled<TwoName, DailyValuation>>());

            yield return new TestCaseData(TestDatabaseName.OneSec, Totals.BankAccount, null);

            yield return new TestCaseData(TestDatabaseName.OneSec, Totals.Security, new List<Labelled<TwoName, DailyValuation>>()
            {
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2010, 1, 1), 200))
            });

            yield return new TestCaseData(TestDatabaseName.TwoBank, Totals.BankAccount, null);

            yield return new TestCaseData(TestDatabaseName.TwoBank, Totals.Security, new List<Labelled<TwoName, DailyValuation>>());

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.BankAccount, null);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, new List<Labelled<TwoName, DailyValuation>> {
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2010, 1, 1), 200)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation( new DateTime(2010, 1, 5), 2020)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation(new DateTime(2012, 5, 5), 21022.9600m))
            });
        }

        [TestCaseSource(nameof(TotalInvestmentsCases))]
        public void TotalInvestmentsTests(TestDatabaseName databaseName, Totals totals, List<Labelled<TwoName, DailyValuation>> expected)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            List<Labelled<TwoName, DailyValuation>> investments = portfolio.TotalInvestments(totals);
            CollectionAssert.AreEqual(expected, investments);
        }

        private static IEnumerable<TestCaseData> InvestmentsCases()
        {
            yield return new TestCaseData(TestDatabaseName.OneBank, Account.BankAccount, TestDatabase.Name(Account.BankAccount, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.OneBank, Account.Security, TestDatabase.Name(Account.Security, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.OneSec, Account.BankAccount, TestDatabase.Name(Account.Security, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.OneSec, Account.Security, TestDatabase.Name(Account.Security, NameOrder.Default), new List<Labelled<TwoName, DailyValuation>>()
            {
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2010, 1, 1), 200))
            });

            yield return new TestCaseData(TestDatabaseName.TwoBank, Account.BankAccount, TestDatabase.Name(Account.BankAccount, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.TwoBank, Account.Security, TestDatabase.Name(Account.Security, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Account.BankAccount, TestDatabase.Name(Account.BankAccount, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Account.Security, TestDatabase.Name(Account.Security, NameOrder.Secondary), new List<Labelled<TwoName, DailyValuation>>
            {
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation( new DateTime(2010, 1, 5), 2020)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation(new DateTime(2012, 5, 5), 21022.9600m))
            });
        }

        [TestCaseSource(nameof(InvestmentsCases))]
        public void InvestmentTests(TestDatabaseName databaseName, Account account, TwoName name, List<Labelled<TwoName, DailyValuation>> expected)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            List<Labelled<TwoName, DailyValuation>> investments = portfolio.Investments(account, name);
            CollectionAssert.AreEqual(expected, investments);
        }
    }
}
