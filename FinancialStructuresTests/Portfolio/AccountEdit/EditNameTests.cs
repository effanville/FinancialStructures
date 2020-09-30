using System.Collections.Generic;
using System.Linq;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;
using StructureCommon.Reporting;

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
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSecurityFromName(BaseCompanyName, BaseName);

            var database = constructor.database;

            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName));

            NameData accountNames = database.Funds.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
        }

        [Test]
        public void CanEditSecurityUrl()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSecurityFromName(BaseCompanyName, BaseName, url: "http://www.google.com");

            var database = constructor.database;

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
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSecurityFromName(BaseCompanyName, BaseName, currency: "Pounds");

            var database = constructor.database;

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
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSecurityFromName(BaseCompanyName, BaseName);

            var database = constructor.database;

            var sectorValues = new HashSet<string>() { "Cats", "Dogs" };
            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName, sectors: sectorValues));

            NameData accountNames = database.Funds.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);

            var actualSectors = accountNames.Sectors.ToList();
            var expectedSectors = sectorValues.ToList();
            Assert.AreEqual(sectorValues.Count, actualSectors.Count);
            for (int sectorIndex = 0; sectorIndex < sectorValues.Count; sectorIndex++)
            {
                Assert.AreEqual(expectedSectors[sectorIndex], actualSectors[sectorIndex]);
            }
        }

        [Test]
        public void CanEditSectorName()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(BaseCompanyName, BaseName);

            var database = constructor.database;

            _ = database.TryEditName(Account.Benchmark, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName));

            NameData accountNames = database.BenchMarks.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
        }

        [Test]
        public void CanEditBankAccountName()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithBankAccountFromName(BaseCompanyName, BaseName);

            var database = constructor.database;

            _ = database.TryEditName(Account.BankAccount, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName));

            NameData accountNames = database.BankAccounts.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
        }

        [Test]
        public void CanEditBankAccountUrl()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithBankAccountFromName(BaseCompanyName, BaseName, url: "http://www.google.com");

            var database = constructor.database;

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
            var constructor = new DatabaseConstructor();
            _ = constructor.WithBankAccountFromName(BaseCompanyName, BaseName, currency: "Pounds");

            var database = constructor.database;

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
            var constructor = new DatabaseConstructor();
            _ = constructor.WithBankAccountFromName(BaseCompanyName, BaseName);

            var database = constructor.database;

            var sectorValues = new HashSet<string>() { "Cats", "Dogs" };
            _ = database.TryEditName(Account.BankAccount, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName, sectors: sectorValues));

            NameData accountNames = database.BankAccounts.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);

            var actualSectors = accountNames.Sectors.ToList();
            var expectedSectors = sectorValues.ToList();
            Assert.AreEqual(sectorValues.Count, actualSectors.Count);
            for (int sectorIndex = 0; sectorIndex < sectorValues.Count; sectorIndex++)
            {
                Assert.AreEqual(expectedSectors[sectorIndex], actualSectors[sectorIndex]);
            }
        }

        [Test]
        public void CanEditCurrencyName()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithCurrencyFromName(BaseCompanyName, BaseName);

            var database = constructor.database;

            _ = database.TryEditName(Account.Currency, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName));

            NameData accountNames = database.Currencies.First().Names;
            Assert.AreEqual(NewName, accountNames.Name);
            Assert.AreEqual(NewCompanyName, accountNames.Company);
        }

        [Test]
        public void ReportsSecurityCorrect()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSecurityFromName(BaseCompanyName, BaseName);
            var reports = new List<ErrorReport>();
            var database = constructor.database;
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
            var constructor = new DatabaseConstructor();
            var reports = new List<ErrorReport>();
            var database = constructor.database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryEditName(Account.Security, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            Assert.AreEqual(1, reports.Count);

            var report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.EditingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Renaming Security: Could not find Security with name {BaseCompanyName}-{BaseName}.", report.Message);
        }

        [Test]
        public void ReportsSectorCorrect()
        {
            var constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName(BaseCompanyName, BaseName);
            var reports = new List<ErrorReport>();
            var database = constructor.database;
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
            var constructor = new DatabaseConstructor();
            var reports = new List<ErrorReport>();
            var database = constructor.database;
            IReportLogger logging = new LogReporter((a, b, c, d) => reports.Add(new ErrorReport(a, b, c, d)));
            _ = database.TryEditName(Account.Benchmark, new NameData(BaseCompanyName, BaseName), new NameData(NewCompanyName, NewName), logging);

            Assert.AreEqual(1, reports.Count);

            var report = reports.First();
            Assert.AreEqual(ReportType.Error, report.ErrorType);
            Assert.AreEqual(ReportLocation.EditingData, report.ErrorLocation);
            Assert.AreEqual(ReportSeverity.Useful, report.ErrorSeverity);
            Assert.AreEqual($"Renaming Benchmark: Could not find Benchmark with name {BaseCompanyName}-{BaseName}.", report.Message);
        }
    }
}
