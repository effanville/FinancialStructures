using System.Collections.Generic;
using System.Linq;
using Common.Structure.Reporting;
using FinancialStructures.Database;
using FinancialStructures.Database.Implementation;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

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
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName);

            Portfolio database = constructor.Database;

            _ = database.TryRemove(Account.Security, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.Funds.Count);
        }

        [Test]
        public void CanRemoveSector()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(BaseCompanyName, BaseName);

            Portfolio database = constructor.Database;

            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.BenchMarks.Count);
        }

        [Test]
        public void CanRemoveBankAccount()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithBankAccount(BaseCompanyName, BaseName);

            Portfolio database = constructor.Database;

            _ = database.TryRemove(Account.BankAccount, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.BankAccounts.Count);
        }

        [Test]
        public void CanRemoveCurrency()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithCurrencyFromName(BaseCompanyName, BaseName);

            Portfolio database = constructor.Database;

            _ = database.TryRemove(Account.Currency, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.Currencies.Count);
        }

        [Test]
        public void ReportsSecurityCorrect()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName);
            List<ErrorReport> reports = new List<ErrorReport>();
            Portfolio database = constructor.Database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryRemove(Account.Security, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Information, report.ErrorType);
            Assert.AreEqual(ReportLocation.DeletingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Detailed, report.ErrorSeverity);
            Assert.AreEqual($"Security {BaseCompanyName}-{BaseName} removed from the database.", report.Message);
        }

        [Test]
        public void RemovingSecurityFailReports()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            List<ErrorReport> reports = new List<ErrorReport>();
            Portfolio database = constructor.Database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryRemove(Account.Security, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.AddingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Security - {BaseCompanyName}-{BaseName} could not be found in the database.", report.Message);
        }

        [Test]
        public void ReportSectorCorrect()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(BaseCompanyName, BaseName);
            List<ErrorReport> reports = new List<ErrorReport>();
            Portfolio database = constructor.Database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Information, report.ErrorType);
            Assert.AreEqual(ReportLocation.DeletingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Detailed, report.ErrorSeverity);
            Assert.AreEqual($"Deleted benchmark {BaseName}", report.Message);
        }

        [Test]
        public void RemovingSectorFailReports()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            List<ErrorReport> reports = new List<ErrorReport>();
            Portfolio database = constructor.Database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.AddingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Benchmark - {BaseCompanyName}-{BaseName} could not be found in the database.", report.Message);
        }
    }
}
