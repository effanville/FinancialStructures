using Effanville.Common.Structure.DataEdit;
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
           UpdateResult<(Account, NameData)> result = database.TryRemove(Account.Security, new TwoName(BaseCompanyName, BaseName));

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.IsChange, Is.False);
                Assert.That(result.IsAdd, Is.False);
                Assert.That(result.IsDelete, Is.True);
                Assert.That(result.Message, Is.Null);
            });
        }

        [Test]
        public void RemovingSecurityFailReports()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();

            UpdateResult<(Account, NameData)> result = database.TryRemove(Account.Security, new NameData(BaseCompanyName, BaseName));

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.IsChange, Is.False);
                Assert.That(result.IsAdd, Is.False);
                Assert.That(result.IsDelete, Is.True);
                Assert.That(result.Message, Is.EqualTo($"Security-{BaseCompanyName}-{BaseName} could not be found in the database."));
            });
        }

        [Test]
        public void ReportSectorCorrect()
        {
            Portfolio database =
                    new DatabaseConstructor()
                    .WithSectorFromName(BaseCompanyName, BaseName)
                    .GetInstance();
            UpdateResult<(Account, NameData)> result = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName));

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.IsChange, Is.False);
                Assert.That(result.IsAdd, Is.False);
                Assert.That(result.IsDelete, Is.True);
                Assert.That(result.Message, Is.Null);
            });
        }

        [Test]
        public void RemovingSectorFailReports()
        {
            Portfolio database = new DatabaseConstructor().GetInstance();

            UpdateResult<(Account, NameData)> result = database.TryRemove(Account.Benchmark, new NameData(BaseCompanyName, BaseName));

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.IsChange, Is.False);
                Assert.That(result.IsAdd, Is.False);
                Assert.That(result.IsDelete, Is.True);
                Assert.That(result.Message, Is.EqualTo($"Benchmark-{BaseCompanyName}-{BaseName} could not be found in the database."));
            });
        }
    }
}
