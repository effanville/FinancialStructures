using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using NUnit.Framework;
using Common.Structure.DisplayClasses;
using FinancePortfolioDatabase.Tests.ViewModelExtensions;
using FinancePortfolioDatabase.Tests.TestHelpers;

namespace FinancePortfolioDatabase.Tests.CommonWindowTests
{
    [TestFixture]
    public class BankAccountDataNamesViewTests : DataNamesViewTestHelper
    {
        [SetUp]
        public void SetBankAccount()
        {
            AccountType = Account.BankAccount;
            base.Setup();
        }

        [Test]
        public void CanCreateNew()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            NameData newName = new NameData("company", "name", "GBP", "someUrl", new HashSet<string>())
            {
                Company = "Company"
            };

            ViewModel.SelectItem(newName);
            ViewModel.BeginEdit();

            ViewModel.DataNames.Add(new SelectableEquatable<NameData>(newName, false));
            ViewModel.CompleteEdit();
            Assert.AreEqual(2, ViewModel.DataNames.Count);
            Assert.AreEqual(2, Portfolio.BankAccountsThreadSafe.Count);
        }

        [Test]
        [STAThread]
        public void CanEditName()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            var item = ViewModel.DataNames[0].Instance;
            ViewModel.SelectItem(item);
            ViewModel.BeginEdit();
            item.Company = "NewCompany";
            ViewModel.CompleteEdit();

            Assert.AreEqual(1, ViewModel.DataNames.Count);
            Assert.AreEqual(1, Portfolio.BankAccountsThreadSafe.Count);

            Assert.AreEqual("NewCompany", Portfolio.BankAccountsThreadSafe.Single().Names.Company);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanDownload()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            var item = ViewModel.DataNames.First();
            ViewModel.SelectItem(item.Instance);
            ViewModel.DownloadSelected();

            Assert.AreEqual(1, ViewModel.DataNames.Count);
            bool account = Portfolio.TryGetAccount(Account.BankAccount, new TwoName("Barclays", "currentAccount"), out var sec);
            Assert.AreEqual(2, sec.Values.Count());
        }

        [Test]
        public void CanDelete()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            Assert.AreEqual(1, ViewModel.DataStore.FundsThreadSafe.Count);
            Assert.AreEqual(1, Portfolio.BankAccountsThreadSafe.Count);
            var item = new NameData("Barclays", "currentAccount");
            ViewModel.SelectItem(item);
            ViewModel.DeleteSelected();
            Assert.AreEqual(0, ViewModel.DataStore.BankAccountsThreadSafe.Count);
            Assert.AreEqual(0, Portfolio.BankAccountsThreadSafe.Count);
        }
    }
}
