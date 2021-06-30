using System.Collections.Generic;
using System.Linq;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;
using Common.Structure.Reporting;

namespace FinancialStructures.Tests.Database.AccountEdit
{
    [TestFixture]
    public sealed class TryRemoveTests
    {
        private readonly string BaseCompanyName = "someCompany";
        private readonly string BaseName = "someName";

        [Test]
        public void CanRemoveSecurity()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSecurityFromName(BaseCompanyName, BaseName);

            var database = constructor.database;

            _ = database.TryRemove(Account.Security, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.Funds.Count);
        }

        [Test]
        public void CanRemoveSector()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(BaseCompanyName, BaseName);

            var database = constructor.database;

            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.BenchMarks.Count);
        }

        [Test]
        public void CanRemoveBankAccount()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithBankAccountFromName(BaseCompanyName, BaseName);

            var database = constructor.database;

            _ = database.TryRemove(Account.BankAccount, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.BankAccounts.Count);
        }

        [Test]
        public void CanRemoveCurrency()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithCurrencyFromName(BaseCompanyName, BaseName);

            var database = constructor.database;

            _ = database.TryRemove(Account.Currency, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.Currencies.Count);
        }

        [Test]
        public void ReportsSecurityCorrect()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSecurityFromName(BaseCompanyName, BaseName);
            var reports = new List<ErrorReport>();
            var database = constructor.database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryRemove(Account.Security, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            var report = reports.First();
            Assert.AreEqual(ReportType.Information, report.ErrorType);
            Assert.AreEqual(ReportLocation.DeletingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Detailed, report.ErrorSeverity);
            Assert.AreEqual($"Security {BaseCompanyName}-{BaseName} removed from the database.", report.Message);
        }

        [Test]
        public void RemovingSecurityFailReports()
        {
            var constructor = new DatabaseConstructor();
            var reports = new List<ErrorReport>();
            var database = constructor.database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryRemove(Account.Security, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            var report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.AddingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Security - {BaseCompanyName}-{BaseName} could not be found in the database.", report.Message);
        }

        [Test]
        public void ReportSectorCorrect()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(BaseCompanyName, BaseName);
            var reports = new List<ErrorReport>();
            var database = constructor.database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            var report = reports.First();
            Assert.AreEqual(ReportType.Information, report.ErrorType);
            Assert.AreEqual(ReportLocation.DeletingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Detailed, report.ErrorSeverity);
            Assert.AreEqual($"Deleted benchmark {BaseName}", report.Message);
        }

        [Test]
        public void RemovingSectorFailReports()
        {
            var constructor = new DatabaseConstructor();
            var reports = new List<ErrorReport>();
            var database = constructor.database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            var report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.AddingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Benchmark - {BaseCompanyName}-{BaseName} could not be found in the database.", report.Message);
        }
    }
}
