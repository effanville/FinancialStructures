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
        private readonly string BaseCompanyName = "someCompany";
        private readonly string BaseName = "someName";
        private readonly string NewCompanyName = "newCompany";
        private readonly string NewName = "newName";

        [Test]
        public void CanEditSecurityName()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName);

            Portfolio database = constructor.Database;

            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName));

            NameData accountNames = database.Funds.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
        }

        [Test]
        public void CanEditSecurityUrl()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName, url: "http://www.google.com");

            Portfolio database = constructor.Database;

            string newUrl = "http://www.amazon.com";
            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(BaseCompanyName, BaseName, url: newUrl));

            NameData accountNames = database.Funds.First().Names;
            Assert.AreEqual(BaseName, accountNames.Name);
            Assert.AreEqual(BaseCompanyName, accountNames.Company);
            Assert.AreEqual(newUrl, accountNames.Url);
        }

        [Test]
        public void CanEditSecurityCurrency()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName, currency: "Pounds");

            Portfolio database = constructor.Database;

            string newCurrency = "Dollars";
            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(BaseCompanyName, BaseName, currency: newCurrency));

            NameData accountNames = database.Funds.First().Names;
            Assert.AreEqual(BaseName, accountNames.Name);
            Assert.AreEqual(BaseCompanyName, accountNames.Company);
            Assert.AreEqual(newCurrency, accountNames.Currency);
        }

        [Test]
        public void CanEditSecuritySectors()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName);

            Portfolio database = constructor.Database;

            HashSet<string> sectorValues = new HashSet<string>() { "Cats", "Dogs" };
            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName, sectors: sectorValues));

            NameData accountNames = database.Funds.First().Names;
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
        public void CanEditSectorName()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(BaseCompanyName, BaseName);

            Portfolio database = constructor.Database;

            _ = database.TryEditName(Account.Benchmark, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName));

            NameData accountNames = database.BenchMarks.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
        }

        [Test]
        public void CanEditBankAccountName()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithBankAccount(BaseCompanyName, BaseName);

            Portfolio database = constructor.Database;

            _ = database.TryEditName(Account.BankAccount, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName));

            NameData accountNames = database.BankAccounts.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
        }

        [Test]
        public void CanEditBankAccountUrl()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithBankAccount(BaseCompanyName, BaseName, url: "http://www.google.com");

            Portfolio database = constructor.Database;

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
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithBankAccount(BaseCompanyName, BaseName, currency: "Pounds");

            Portfolio database = constructor.Database;

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
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithBankAccount(BaseCompanyName, BaseName);

            Portfolio database = constructor.Database;

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
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithCurrencyFromName(BaseCompanyName, BaseName);

            Portfolio database = constructor.Database;

            _ = database.TryEditName(Account.Currency, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName));

            NameData accountNames = database.Currencies.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
        }

        [Test]
        public void ReportsSecurityCorrect()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity(BaseCompanyName, BaseName);
            List<ErrorReport> reports = new List<ErrorReport>();
            Portfolio database = constructor.Database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            NameData accountNames = database.Funds.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);

            Assert.AreEqual(0, reports.Count);

            /*var report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.EditingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Renaming Security name {BaseCompanyName}-{BaseName} to {NewCompanyName}-{NewName}.", report.Message);*/
        }

        [Test]
        public void EditingSecurityFailReports()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            List<ErrorReport> reports = new List<ErrorReport>();
            Portfolio database = constructor.Database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            Assert.AreEqual(1, reports.Count);

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.EditingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Renaming Security: Could not find Security with name {BaseCompanyName}-{BaseName}.", report.Message);
        }

        [Test]
        public void ReportsSectorCorrect()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(BaseCompanyName, BaseName);
            List<ErrorReport> reports = new List<ErrorReport>();
            Portfolio database = constructor.Database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryEditName(Account.Benchmark, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            NameData accountNames = database.BenchMarks.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);

            Assert.AreEqual(0, reports.Count);

            /*var report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.EditingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Renaming Sector name {BaseCompanyName}-{BaseName} to {NewCompanyName}-newName.", report.Message);*/
        }

        [Test]
        public void EditingSectorFailReports()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            List<ErrorReport> reports = new List<ErrorReport>();
            Portfolio database = constructor.Database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryEditName(Account.Benchmark, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            Assert.AreEqual(1, reports.Count);

            ErrorReport report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.EditingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Renaming Benchmark: Could not find Benchmark with name {BaseCompanyName}-{BaseName}.", report.Message);
        }
    }
}
