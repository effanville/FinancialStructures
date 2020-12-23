using System.Collections.Generic;
using System.Linq;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;
using StructureCommon.Reporting;
using FinancialStructures.Database;

namespace FinancialStructures.Tests.Database.AccountEdit
{
    [TestFixture]
    public sealed class TryAddTests
    {
        private readonly string BaseCompanyName = "someCompany";
        private readonly string BaseName = "someName";

        [Test]
        public void CanAddSecurity()
        {
            var constructor = new DatabaseConstructor();

            var database = constructor.database;

            _ = database.TryAdd(Account.Security, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(1, database.Funds.Count);
            NameData accountNames = database.Funds.First().Names;
            Assert.AreEqual(BaseName, accountNames.Name);
            Assert.AreEqual(BaseCompanyName, accountNames.Company);
        }

        [Test]
        public void CanAddSector()
        {
            var constructor = new DatabaseConstructor();

            var database = constructor.database;

            _ = database.TryAdd(Account.Benchmark, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(1, database.BenchMarks.Count);
            NameData accountNames = database.BenchMarks.First().Names;
            Assert.AreEqual(BaseName, accountNames.Name);
            Assert.AreEqual(BaseCompanyName, accountNames.Company);
        }

        [Test]
        public void CanAddBankAccount()
        {
            var constructor = new DatabaseConstructor();

            var database = constructor.database;

            _ = database.TryAdd(Account.BankAccount, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(1, database.BankAccounts.Count);
            NameData accountNames = database.BankAccounts.First().Names;
            Assert.AreEqual(BaseName, accountNames.Name);
            Assert.AreEqual(BaseCompanyName, accountNames.Company);
        }

        [Test]
        public void CanAddCurrency()
        {
            var constructor = new DatabaseConstructor();

            var database = constructor.database;

            _ = database.TryAdd(Account.Currency, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(1, database.Currencies.Count);
            NameData accountNames = database.Currencies.First().Names;
            Assert.AreEqual(BaseName, accountNames.Name);
            Assert.AreEqual(BaseCompanyName, accountNames.Company);
        }

        [Test]
        public void ReportsSecurityCorrect()
        {
            var constructor = new DatabaseConstructor();
            var reports = new List<ErrorReport>();
            var database = constructor.database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryAdd(Account.Security, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            var report = reports.First();
            Assert.AreEqual(ReportType.Report, report.ErrorType);
            Assert.AreEqual(ReportLocation.AddingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Detailed, report.ErrorSeverity);
            Assert.AreEqual($"Security `{BaseCompanyName}'-`{BaseName}' added to database.", report.Message);
        }

        [Test]
        public void AddingSecurityFailReports()
        {
            var constructor = new DatabaseConstructor();

            _ = constructor.WithSecurityFromName(BaseCompanyName, BaseName);

            var reports = new List<ErrorReport>();
            var database = constructor.database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryAdd(Account.Security, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            var report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.AddingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Critical, report.ErrorSeverity);
            Assert.AreEqual($"Security `{BaseCompanyName}'-`{BaseName}' already exists.", report.Message);
        }

        [Test]
        public void ReportSectorCorrect()
        {
            var constructor = new DatabaseConstructor();
            var reports = new List<ErrorReport>();
            var database = constructor.database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryAdd(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            var report = reports.First();
            Assert.AreEqual(ReportType.Report, report.ErrorType);
            Assert.AreEqual(ReportLocation.AddingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Detailed, report.ErrorSeverity);
            Assert.AreEqual($"Benchmark `{BaseCompanyName}'-`{BaseName}' added to database.", report.Message);
        }

        [Test]
        public void AddingSectorFailReports()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(BaseCompanyName, BaseName);
            var reports = new List<ErrorReport>();
            var database = constructor.database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryAdd(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            Assert.AreEqual(1, reports.Count);

            var report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.AddingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Critical, report.ErrorSeverity);
            Assert.AreEqual($"Benchmark `{BaseCompanyName}'-`{BaseName}' already exists.", report.Message);
        }
    }
}
