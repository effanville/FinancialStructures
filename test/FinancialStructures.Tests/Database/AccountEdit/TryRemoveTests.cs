using System.Linq;

using Effanville.Common.Structure.Reporting;

using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.AccountEdit
{
    [TestFixture]
    public sealed class TryRemoveTests
    {
        private readonly string BaseCompanyName = "someCompany";
        private readonly string BaseName = "someName";

        [Test]
        public void CanRemoveSecurity()
        {
            global::FinancialStructures.Database.Implementation.Portfolio database =
                new DatabaseConstructor()
                .WithSecurity(BaseCompanyName, BaseName)
                .GetInstance();

            _ = database.TryRemove(Account.Security, new TwoName(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.Funds.Count);
        }

        [Test]
        public void CanRemoveSector()
        {
            global::FinancialStructures.Database.Implementation.Portfolio database =
                new DatabaseConstructor()
                .WithSectorFromName(BaseCompanyName, BaseName)
                .GetInstance();

            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.BenchMarks.Count);
        }

        [Test]
        public void CanRemoveBankAccount()
        {
            global::FinancialStructures.Database.Implementation.Portfolio database =
                new DatabaseConstructor()
                .WithBankAccount(BaseCompanyName, BaseName)
                .GetInstance();

            _ = database.TryRemove(Account.BankAccount, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.BankAccounts.Count);
        }

        [Test]
        public void CanRemoveCurrency()
        {
            global::FinancialStructures.Database.Implementation.Portfolio database =
                new DatabaseConstructor()
                .WithCurrency(BaseCompanyName, BaseName)
                .GetInstance();

            _ = database.TryRemove(Account.Currency, new NameData(BaseCompanyName, BaseName));

            Assert.AreEqual(0, database.Currencies.Count);
        }

        [Test]
        public void ReportsSecurityCorrect()
        {
            global::FinancialStructures.Database.Implementation.Portfolio database =
                new DatabaseConstructor()
                .WithSecurity(BaseCompanyName, BaseName)
                .GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryRemove(Account.Security, new TwoName(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.AreEqual(1, reports.Count());

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Information, report.ErrorType);
            Assert.AreEqual("DeletingData", report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Detailed, report.ErrorSeverity);
            Assert.AreEqual($"Security-{BaseCompanyName}-{BaseName} removed from the database.", report.Message);
        }

        [Test]
        public void RemovingSecurityFailReports()
        {
            global::FinancialStructures.Database.Implementation.Portfolio database = new DatabaseConstructor().GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);

            _ = database.TryRemove(Account.Security, new NameData(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.AreEqual(1, reports.Count());

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual("DeletingData", report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Security - {BaseCompanyName}-{BaseName} could not be found in the database.", report.Message);
        }

        [Test]
        public void ReportSectorCorrect()
        {
            global::FinancialStructures.Database.Implementation.Portfolio database =
                    new DatabaseConstructor()
                    .WithSectorFromName(BaseCompanyName, BaseName)
                    .GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.AreEqual(1, reports.Count());

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Information, report.ErrorType);
            Assert.AreEqual("DeletingData", report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Detailed, report.ErrorSeverity);
            Assert.AreEqual($"Benchmark-{BaseCompanyName}-{BaseName} removed from the database.", report.Message);
        }

        [Test]
        public void RemovingSectorFailReports()
        {
            global::FinancialStructures.Database.Implementation.Portfolio database = new DatabaseConstructor().GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);

            _ = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName), logging);

            ErrorReports reports = logging.Reports;
            Assert.AreEqual(1, reports.Count());

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual("DeletingData", report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Benchmark - {BaseCompanyName}-{BaseName} could not be found in the database.", report.Message);
        }
    }
}
