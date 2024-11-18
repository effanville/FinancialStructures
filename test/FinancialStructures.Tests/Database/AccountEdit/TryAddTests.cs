using System.Linq;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.AccountEdit
{
    [TestFixture]
    public sealed class TryAddTests
    {
        private const string BaseCompanyName = "someCompany";
        private const string BaseName = "someName";

        [Test]
        public void CanAddSecurity()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            _ = database.TryAdd(Account.Security, new NameData(BaseCompanyName, BaseName));

            Assert.That(database.Funds.Count, Is.EqualTo(1));
            NameData accountNames = database.Funds.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(BaseName));
            Assert.That(accountNames.Company, Is.EqualTo(BaseCompanyName));
        }

        [Test]
        public void CanAddSector()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            _ = database.TryAdd(Account.Benchmark, new NameData(BaseCompanyName, BaseName));

            Assert.That(database.BenchMarks.Count, Is.EqualTo(1));
            NameData accountNames = database.BenchMarks.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(BaseName));
            Assert.That(accountNames.Company, Is.EqualTo(BaseCompanyName));
        }

        [Test]
        public void CanAddBankAccount()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            _ = database.TryAdd(Account.BankAccount, new NameData(BaseCompanyName, BaseName));

            Assert.That(database.BankAccounts.Count, Is.EqualTo(1));
            NameData accountNames = database.BankAccounts.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(BaseName));
            Assert.That(accountNames.Company, Is.EqualTo(BaseCompanyName));
        }

        [Test]
        public void CanAddCurrency()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            _ = database.TryAdd(Account.Currency, new NameData(BaseCompanyName, BaseName));

            Assert.That(database.Currencies.Count, Is.EqualTo(1));
            NameData accountNames = database.Currencies.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(BaseName));
            Assert.That(accountNames.Company, Is.EqualTo(BaseCompanyName));
        }

        [Test]
        public void ReportsSecurityCorrect()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryAdd(Account.Security, new NameData(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.That(reports.Count(), Is.EqualTo(1));
            ErrorReport report = reports.First();
            Assert.That(report.ErrorType, Is.EqualTo(ReportType.Information));
            Assert.That(report.ErrorLocation, Is.EqualTo("AddingData"));
            Assert.That(report.ErrorSeverity, Is.EqualTo(ReportSeverity.Detailed));
            Assert.That(report.Message, Is.EqualTo($"Security-{BaseCompanyName}-{BaseName} added to database."));
        }

        [Test]
        public void AddingSecurityFailReports()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithSecurity(BaseCompanyName, BaseName)
                .GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryAdd(Account.Security, new NameData(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.That(reports.Count(), Is.EqualTo(1));
            ErrorReport report = reports.First();
            Assert.That(report.ErrorType, Is.EqualTo(ReportType.Error));
            Assert.That(report.ErrorLocation, Is.EqualTo("AddingData"));
            Assert.That(report.ErrorSeverity, Is.EqualTo(ReportSeverity.Critical));
            Assert.That(report.Message, Is.EqualTo($"Security-{BaseCompanyName}-{BaseName} already exists."));
        }

        [Test]
        public void ReportSectorCorrect()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryAdd(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.That(reports.Count(), Is.EqualTo(1));
            ErrorReport report = reports.First();
            Assert.That(report.ErrorType, Is.EqualTo(ReportType.Information));
            Assert.That(report.ErrorLocation, Is.EqualTo("AddingData"));
            Assert.That(report.ErrorSeverity, Is.EqualTo(ReportSeverity.Detailed));
            Assert.That(report.Message, Is.EqualTo($"Benchmark-{BaseCompanyName}-{BaseName} added to database."));
        }

        [Test]
        public void AddingSectorFailReports()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithSectorFromName(BaseCompanyName, BaseName)
                .GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryAdd(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.That(reports.Count(), Is.EqualTo(1));
            ErrorReport report = reports.First();
            Assert.That(report.ErrorType, Is.EqualTo(ReportType.Error));
            Assert.That(report.ErrorLocation, Is.EqualTo("AddingData"));
            Assert.That(report.ErrorSeverity, Is.EqualTo(ReportSeverity.Critical));
            Assert.That(report.Message, Is.EqualTo($"Benchmark-{BaseCompanyName}-{BaseName} already exists."));
        }
    }
}
