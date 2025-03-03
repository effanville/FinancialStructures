using System.Linq;
using Effanville.Common.Structure.DataEdit;
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
            UpdateResult<(Account, NameData)> result = database.TryAdd(Account.Security, new NameData(BaseCompanyName, BaseName));

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.IsChange, Is.False);
                Assert.That(result.IsAdd, Is.True);
                Assert.That(result.IsDelete, Is.False);
                Assert.That(result.Message, Is.Null);
            });
        }

        [Test]
        public void AddingSecurityFailReports()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithSecurity(BaseCompanyName, BaseName)
                .GetInstance();
            UpdateResult<(Account, NameData)> result = database.TryAdd(Account.Security, new NameData(BaseCompanyName, BaseName));

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.IsChange, Is.False);
                Assert.That(result.Message, Is.EqualTo($"Security-{BaseCompanyName}-{BaseName} already exists."));
            });
        }

        [Test]
        public void ReportSectorCorrect()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();
            UpdateResult<(Account, NameData)> result = database.TryAdd(Account.Benchmark, new NameData(BaseCompanyName, BaseName));

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.IsChange, Is.False);
                Assert.That(result.IsAdd, Is.True);
                Assert.That(result.IsDelete, Is.False);
                Assert.That(result.Message, Is.Null);
            });
        }

        [Test]
        public void AddingSectorFailReports()
        {
            Portfolio database =
                new DatabaseConstructor()
                .WithSectorFromName(BaseCompanyName, BaseName)
                .GetInstance();
            UpdateResult<(Account, NameData)> result = database.TryAdd(Account.Benchmark, new NameData(BaseCompanyName, BaseName));

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.IsChange, Is.False);
                Assert.That(result.Message, Is.EqualTo($"Benchmark-{BaseCompanyName}-{BaseName} already exists."));
            });
        }
    }
}
