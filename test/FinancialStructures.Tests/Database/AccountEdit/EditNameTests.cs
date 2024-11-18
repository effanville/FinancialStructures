using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.AccountEdit
{
    [TestFixture]
    public sealed class EditNameTests
    {
        private const string BaseCompanyName = "someCompany";
        private const string BaseName = "someName";
        private const string NewCompanyName = "newCompany";
        private const string NewName = "newName";

        private static IEnumerable<TestCaseData> SecurityCases()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName);
            yield return new TestCaseData(constructor.Database, NewCompanyName, NewName, null, null, new HashSet<string>()).SetName("CanEditSecurityName");
            constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName);
            yield return new TestCaseData(constructor.Database, NewCompanyName, NewName, null, null, null).SetName("CanEditSecurityNameNullSectors");
            constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName, url: "http://www.google.com");
            yield return new TestCaseData(constructor.Database, BaseCompanyName, BaseName, "http://www.amazon.com", null, new HashSet<string>()).SetName("CanEditSecurityUrl");
            constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName, currency: "pounds");
            yield return new TestCaseData(constructor.Database, BaseCompanyName, BaseName, null, "dollars", new HashSet<string>()).SetName("CanEditSecurityCurrency");
            constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName);
            yield return new TestCaseData(constructor.Database, BaseCompanyName, BaseName, null, null, new HashSet<string>() { "Cats", "Dogs" }).SetName("CanEditSecuritySectors");
            constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName);
            yield return new TestCaseData(constructor.Database, BaseCompanyName, BaseName, "http://www.cats.com", "dollars", new HashSet<string>() { "Cats", "Dogs" }).SetName("CanEditSecurity");
        }

        [TestCaseSource(nameof(SecurityCases))]
        public void CanEditSecurity(IPortfolio database, string newComp, string newName, string newUrl, string newCurrency, HashSet<string> newSectors)
        {
            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(newComp, newName, newCurrency, newUrl, newSectors));

            NameData accountNames = database.Funds[0].Names;
            Assert.That(accountNames.Name, Is.EqualTo(newName));
            Assert.That(accountNames.Company, Is.EqualTo(newComp));
            Assert.That(accountNames.Url, Is.EqualTo(newUrl));
            Assert.That(accountNames.Currency, Is.EqualTo(newCurrency));
            List<string> actualSectors = accountNames.Sectors.ToList();
            if (newSectors != null)
            {
                List<string> expectedSectors = newSectors.ToList();
                Assert.That(actualSectors.Count, Is.EqualTo(newSectors.Count));
                for (int sectorIndex = 0; sectorIndex < newSectors.Count; sectorIndex++)
                {
                    Assert.That(actualSectors[sectorIndex], Is.EqualTo(expectedSectors[sectorIndex]));
                }
            }
            else
            {
                Assert.That(actualSectors.Count, Is.EqualTo(0));
            }
        }

        [Test]
        public void CanEditSectorName()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithSectorFromName(BaseCompanyName, BaseName)
                .GetInstance();
            _ = database.TryEditName(Account.Benchmark, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName));

            NameData accountNames = database.BenchMarks.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(NewName));
            Assert.That(accountNames.Company, Is.EqualTo(NewCompanyName));
        }

        [Test]
        public void CanEditBankAccountName()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithBankAccount(BaseCompanyName, BaseName)
                .GetInstance();
            _ = database.TryEditName(Account.BankAccount, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName));

            NameData accountNames = database.BankAccounts.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(NewName));
            Assert.That(accountNames.Company, Is.EqualTo(NewCompanyName));
        }

        [Test]
        public void CanEditBankAccountUrl()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithBankAccount(BaseCompanyName, BaseName, url: "http://www.google.com")
                .GetInstance();
            string newUrl = "http://www.amazon.com";
            _ = database.TryEditName(Account.BankAccount, new NameData(BaseCompanyName, BaseName), new NameData(BaseCompanyName, BaseName, url: newUrl));

            NameData accountNames = database.BankAccounts.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(BaseName));
            Assert.That(accountNames.Company, Is.EqualTo(BaseCompanyName));
            Assert.That(accountNames.Url, Is.EqualTo(newUrl));
        }

        [Test]
        public void CanEditBankAccountCurrency()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithBankAccount(BaseCompanyName, BaseName, currency: "Pounds")
                .GetInstance();
            string newCurrency = "Dollars";
            _ = database.TryEditName(Account.BankAccount, new NameData(BaseCompanyName, BaseName), new NameData(BaseCompanyName, BaseName, currency: newCurrency));

            NameData accountNames = database.BankAccounts.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(BaseName));
            Assert.That(accountNames.Company, Is.EqualTo(BaseCompanyName));
            Assert.That(accountNames.Currency, Is.EqualTo(newCurrency));
        }

        [Test]
        public void CanEditBankAccountSectors()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithBankAccount(BaseCompanyName, BaseName)
                .GetInstance();
            HashSet<string> sectorValues = new HashSet<string>() { "Cats", "Dogs" };
            _ = database.TryEditName(Account.BankAccount, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName, sectors: sectorValues));

            NameData accountNames = database.BankAccounts.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(NewName));
            Assert.That(accountNames.Company, Is.EqualTo(NewCompanyName));

            List<string> actualSectors = accountNames.Sectors.ToList();
            List<string> expectedSectors = sectorValues.ToList();
            Assert.That(actualSectors.Count, Is.EqualTo(sectorValues.Count));
            for (int sectorIndex = 0; sectorIndex < sectorValues.Count; sectorIndex++)
            {
                Assert.That(actualSectors[sectorIndex], Is.EqualTo(expectedSectors[sectorIndex]));
            }
        }

        [Test]
        public void CanEditCurrencyName()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithCurrency(BaseCompanyName, BaseName)
                .GetInstance();
            _ = database.TryEditName(Account.Currency, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName));

            NameData accountNames = database.Currencies.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(NewName));
            Assert.That(accountNames.Company, Is.EqualTo(NewCompanyName));
        }

        [Test]
        public void ReportsSecurityCorrect()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithSecurity(BaseCompanyName, BaseName)
                .GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            NameData accountNames = database.Funds.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(NewName));
            Assert.That(accountNames.Company, Is.EqualTo(NewCompanyName));

            Assert.That(logging.Reports.Count(), Is.EqualTo(0));
        }

        [Test]
        public void EditingSecurityFailReports()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            ErrorReports reports = logging.Reports;
            Assert.That(reports.Count(), Is.EqualTo(1));

            ErrorReport report = reports.First();
            Assert.That(report.ErrorType, Is.EqualTo(ReportType.Error));
            Assert.That(report.ErrorLocation, Is.EqualTo("EditingData"));
            Assert.That(report.ErrorSeverity, Is.EqualTo(ReportSeverity.Critical));
            Assert.That(report.Message, Is.EqualTo($"Could not find Security - {BaseCompanyName}-{BaseName}."));
        }

        [Test]
        public void ReportsSectorCorrect()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithSectorFromName(BaseCompanyName, BaseName)
                .GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryEditName(Account.Benchmark, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            NameData accountNames = database.BenchMarks.First().Names;
            Assert.That(accountNames.Name, Is.EqualTo(NewName));
            Assert.That(accountNames.Company, Is.EqualTo(NewCompanyName));
            Assert.That(logging.Reports.Count(), Is.EqualTo(0));
        }

        [Test]
        public void EditingSectorFailReports()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryEditName(Account.Benchmark, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            ErrorReports reports = logging.Reports;
            Assert.That(reports.Count(), Is.EqualTo(1));

            ErrorReport report = reports.First();
            Assert.That(report.ErrorType, Is.EqualTo(ReportType.Error));
            Assert.That(report.ErrorLocation, Is.EqualTo("EditingData"));
            Assert.That(report.ErrorSeverity, Is.EqualTo(ReportSeverity.Critical));
            Assert.That(report.Message, Is.EqualTo($"Could not find Benchmark - {BaseCompanyName}-{BaseName}."));
        }
    }
}
