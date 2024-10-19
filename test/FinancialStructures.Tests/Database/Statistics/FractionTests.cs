using System;
using System.Collections.Generic;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class FractionTests
    {
        private static IEnumerable<TestCaseData> FractionTestCases()
        {
            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.All, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.Security, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), double.NaN);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.BankAccount, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.All, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), 0.0043886111604674437);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.Security, Account.BankAccount, NameOrder.Default, new DateTime(2015, 5, 4), 0.0046099493496925678);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.Security, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), 0.018367705675060721);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), 0.018367705675060721);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.BankAccount, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), double.NaN);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, Account.Security, NameOrder.Secondary, new DateTime(2015, 5, 4), 0.98163229432493937);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.Security, Account.Security, NameOrder.Default, new DateTime(2015, 5, 4), 0.17855191841350634);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.Security, Account.Security, NameOrder.Secondary, new DateTime(2015, 5, 4), 0.82144808158649374);
        }

        [TestCaseSource(nameof(FractionTestCases))]
        public void FractionTest(TestDatabaseName databaseName, Totals totals, Account account, NameOrder order, DateTime date, double expectedValue)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            double actual = portfolio.Fraction(totals, account, TestDatabase.Name(account, order), date);
            Assert.That(actual, Is.EqualTo(expectedValue));
        }

        private static IEnumerable<TestCaseData> TotalFractionTestCases()
        {
            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.All, null, new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.Security, null, new DateTime(2015, 5, 4), 0.0);

            yield return new TestCaseData(TestDatabaseName.OneBank, Totals.BankAccount, null, new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.All, null, new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.Security, null, new DateTime(2015, 5, 4), 0.95198685008548201);

            yield return new TestCaseData(TestDatabaseName.TwoSecTwoBank, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Secondary).Company, new DateTime(2015, 5, 4), 0.93450103581658372);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.Security, null, new DateTime(2015, 5, 4), 1.0);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.BankAccount, null, new DateTime(2015, 5, 4), 0.0);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Default).Company, new DateTime(2015, 5, 4), 0.018367705675060721);

            yield return new TestCaseData(TestDatabaseName.TwoSec, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Secondary).Company, new DateTime(2015, 5, 4), 0.98163229432493937);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.SecurityCompany, TestDatabase.Name(Account.Security, NameOrder.Secondary).Company, new DateTime(2015, 5, 4), 0.82144808158649374);

            yield return new TestCaseData(TestDatabaseName.TwoSecCur, Totals.Security, null, new DateTime(2015, 5, 4), 1.0);
        }

        [TestCaseSource(nameof(TotalFractionTestCases))]
        public void TotalFractionTest(TestDatabaseName databaseName, Totals totals, string identifier, DateTime date, double expectedValue)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            double actual = portfolio.TotalFraction(totals, identifier, date);
            Assert.That(actual, Is.EqualTo(expectedValue));
        }
    }
}
