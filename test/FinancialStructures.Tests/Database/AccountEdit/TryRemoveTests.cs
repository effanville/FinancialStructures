using System.Linq;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.AccountEdit
{
    [TestFixture]
    public sealed class TryRemoveTests
    {
        private const string BaseCompanyName = "someCompany";
        private const string BaseName = "someName";

        [Test]
        public void CanRemoveSecurity()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithSecurity(BaseCompanyName, BaseName)
                .GetInstance();

            _ = database.TryRemove(Account.Security, new TwoName(BaseCompanyName, BaseName));

            Assert.That(database.Funds.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanRemoveSector()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithSectorFromName(BaseCompanyName, BaseName)
                .GetInstance();

            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName));

            Assert.That(database.BenchMarks.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanRemoveBankAccount()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithBankAccount(BaseCompanyName, BaseName)
                .GetInstance();

            _ = database.TryRemove(Account.BankAccount, new NameData(BaseCompanyName, BaseName));

            Assert.That(database.BankAccounts.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanRemoveCurrency()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithCurrency(BaseCompanyName, BaseName)
                .GetInstance();

            _ = database.TryRemove(Account.Currency, new NameData(BaseCompanyName, BaseName));

            Assert.That(database.Currencies.Count, Is.EqualTo(0));
        }

        [Test]
        public void ReportsSecurityCorrect()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithSecurity(BaseCompanyName, BaseName)
                .GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryRemove(Account.Security, new TwoName(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.That(reports.Count(), Is.EqualTo(1));

            ErrorReport report = reports.First();
            Assert.That(report.ErrorType, Is.EqualTo(ReportType.Information));
            Assert.That(report.ErrorLocation, Is.EqualTo("DeletingData"));
            Assert.That(report.ErrorSeverity, Is.EqualTo(ReportSeverity.Detailed));
            Assert.That(report.Message, Is.EqualTo($"Security-{BaseCompanyName}-{BaseName} removed from the database."));
        }

        [Test]
        public void RemovingSecurityFailReports()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);

            _ = database.TryRemove(Account.Security, new NameData(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.That(reports.Count(), Is.EqualTo(1));

            ErrorReport report = reports.First();
            Assert.That(report.ErrorType, Is.EqualTo(ReportType.Error));
            Assert.That(report.ErrorLocation, Is.EqualTo("DeletingData"));
            Assert.That(report.ErrorSeverity, Is.EqualTo(ReportSeverity.Useful));
            Assert.That(report.Message, Is.EqualTo($"Security-{BaseCompanyName}-{BaseName} could not be found in the database."));
        }

        [Test]
        public void ReportSectorCorrect()
        {
            Portfolio database =
                    new DatabaseConstructor()
                    .WithSectorFromName(BaseCompanyName, BaseName)
                    .GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.That(reports.Count(), Is.EqualTo(1));

            ErrorReport report = reports.First();
            Assert.That(report.ErrorType, Is.EqualTo(ReportType.Information));
            Assert.That(report.ErrorLocation, Is.EqualTo("DeletingData"));
            Assert.That(report.ErrorSeverity, Is.EqualTo(ReportSeverity.Detailed));
            Assert.That(report.Message, Is.EqualTo($"Benchmark-{BaseCompanyName}-{BaseName} removed from the database."));
        }

        [Test]
        public void RemovingSectorFailReports()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);

            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.That(reports.Count(), Is.EqualTo(1));

            ErrorReport report = reports.First();
            Assert.That(report.ErrorType, Is.EqualTo(ReportType.Error));
            Assert.That(report.ErrorLocation, Is.EqualTo("DeletingData"));
            Assert.That(report.ErrorSeverity, Is.EqualTo(ReportSeverity.Useful));
            Assert.That(report.Message, Is.EqualTo($"Benchmark-{BaseCompanyName}-{BaseName} could not be found in the database."));
        }
    }
}
