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
            _ = constructor.WithSecurity(BaseCompanyName, BaseName, currency: "poinds");
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

            NameData accountNames = database.FundsThreadSafe[0].Names;
            Assert.AreEqual(newName, accountNames.Name);
            Assert.AreEqual(newComp, accountNames.Company);
            Assert.AreEqual(newUrl, accountNames.Url);
            Assert.AreEqual(newCurrency, accountNames.Currency);
            List<string> actualSectors = accountNames.Sectors.ToList();
            if (newSectors != null)
            {
                List<string> expectedSectors = newSectors.ToList();
                Assert.AreEqual(newSectors.Count, actualSectors.Count);
                for (int sectorIndex = 0; sectorIndex < newSectors.Count; sectorIndex++)
                {
                    Assert.AreEqual(expectedSectors[sectorIndex], actualSectors[sectorIndex]);
                }
            }
            else
            {
                Assert.AreEqual(0, actualSectors.Count);
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
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
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
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
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
            Assert.AreEqual(BaseName, accountNames.Name);
            Assert.AreEqual(BaseCompanyName, accountNames.Company);
            Assert.AreEqual(newUrl, accountNames.Url);
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
            Assert.AreEqual(BaseName, accountNames.Name);
            Assert.AreEqual(BaseCompanyName, accountNames.Company);
            Assert.AreEqual(newCurrency, accountNames.Currency);
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
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);

            List<string> actualSectors = accountNames.Sectors.ToList();
            List<string> expectedSectors = sectorValues.ToList();
            Assert.AreEqual(sectorValues.Count, actualSectors.Count);
            for (int sectorIndex = 0; sectorIndex < sectorValues.Count; sectorIndex++)
            {
                Assert.AreEqual(expectedSectors[sectorIndex], actualSectors[sectorIndex]);
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
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
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
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);

            Assert.AreEqual(0, logging.Reports.Count());
        }

        [Test]
        public void EditingSecurityFailReports()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            var reports = logging.Reports;
            Assert.AreEqual(1, reports.Count());

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual("EditingData", report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Critical, report.ErrorSeverity);
            Assert.AreEqual($"Could not find Security - {BaseCompanyName}-{BaseName}.", report.Message);
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
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
            Assert.AreEqual(0, logging.Reports.Count());
        }

        [Test]
        public void EditingSectorFailReports()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            IReportLogger logging = new LogReporter(null, saveInternally: true);
            _ = database.TryEditName(Account.Benchmark, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            ErrorReports reports = logging.Reports;
            Assert.AreEqual(1, reports.Count());

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual("EditingData", report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Critical, report.ErrorSeverity);
            Assert.AreEqual($"Could not find Benchmark - {BaseCompanyName}-{BaseName}.", report.Message);
        }
    }
}
