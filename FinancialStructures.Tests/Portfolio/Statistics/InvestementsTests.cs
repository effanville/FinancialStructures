using System;
using System.Collections.Generic;
using Common.Structure.DataStructures;
using Common.Structure.NamingStructures;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Statistics
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
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2010, 1, 1), 200)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2011, 1, 1), -50)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2012, 5, 1), 1978.16)),
                 new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2015, 4, 3), -1204.98)),
                  new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2018, 5, 6),  132.09000000000003)) });

            yield return new TestCaseData(TestDatabaseName.TwoBank, Totals.BankAccount, null);

            yield return new TestCaseData(TestDatabaseName.TwoBank, Totals.Security, new List<Labelled<TwoName, DailyValuation>>());

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.BankAccount, null);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, new List<Labelled<TwoName, DailyValuation>> {
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2010, 1, 1), 200)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2011, 1, 1), -50)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2012, 5, 1), 1978.16)),
                 new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2015, 4, 3), -1204.98)),
                  new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2018, 5, 6),  132.09000000000003)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation( new DateTime(2010, 1, 5), 2020)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation(new DateTime(2011, 2, 1), 555)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation(new DateTime(2012, 5, 5), 21022.960000000003)) ,
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation(new DateTime(2016, 4, 3), 2431.6199999999994)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation(new DateTime(2019, 5, 6), 354.13999999999874))
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
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2010, 1, 1), 200)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2011, 1, 1), -50)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2012, 5, 1), 1978.16)),
                 new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2015, 4, 3), -1204.98)),
                  new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName), new DailyValuation(new DateTime(2018, 5, 6),  132.09000000000003))
            });

            yield return new TestCaseData(TestDatabaseName.TwoBank, Account.BankAccount, TestDatabase.Name(Account.BankAccount, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.TwoBank, Account.Security, TestDatabase.Name(Account.Security, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Account.BankAccount, TestDatabase.Name(Account.BankAccount, NameOrder.Default), null);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Account.Security, TestDatabase.Name(Account.Security, NameOrder.Secondary), new List<Labelled<TwoName, DailyValuation>>
            {
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation( new DateTime(2010, 1, 5), 2020)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation(new DateTime(2011, 2, 1), 555)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation(new DateTime(2012, 5, 5), 21022.960000000003)) ,
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation(new DateTime(2016, 4, 3), 2431.6199999999994)),
                new Labelled<TwoName, DailyValuation>(new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName), new DailyValuation(new DateTime(2019, 5, 6), 354.13999999999874))
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
