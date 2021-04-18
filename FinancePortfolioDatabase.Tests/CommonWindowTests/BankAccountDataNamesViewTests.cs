using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using FinancePortfolioDatabase.Tests.TestConstruction;
using NUnit.Framework;
using StructureCommon.DisplayClasses;

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
            Portfolio = TestingGUICode.CreateBasicDataBase();
            NameData newName = new NameData("company", "name", "GBP", "someUrl", new HashSet<string>())
            {
                Company = "Company"
            };

            SelectItem(newName);
            BeginEdit();

            ViewModel.DataNames.Add(new SelectableEquatable<NameData>(newName, false));
            CompleteEdit();
            Assert.AreEqual(2, ViewModel.DataNames.Count);
            Assert.AreEqual(2, Portfolio.BankAccounts.Count);
        }

        [Test]
        [STAThread]
        public void CanEditName()
        {
            Portfolio = TestingGUICode.CreateBasicDataBase();
            var item = ViewModel.DataNames[0].Instance;
            SelectItem(item);
            BeginEdit();
            item.Company = "NewCompany";
            CompleteEdit();

            Assert.AreEqual(1, ViewModel.DataNames.Count);
            Assert.AreEqual(1, Portfolio.BankAccounts.Count);

            Assert.AreEqual("NewCompany", Portfolio.BankAccounts.Single().Names.Company);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanDownload()
        {
            Portfolio = TestingGUICode.CreateBasicDataBase();
            var item = ViewModel.DataNames.First();
            SelectItem(item.Instance);
            DownloadSelected();

            Assert.AreEqual(1, ViewModel.DataNames.Count);
            bool account = Portfolio.TryGetAccount(Account.BankAccount, new TwoName("Barclays", "currentAccount"), out var sec);
            Assert.AreEqual(2, sec.Values.Count());
        }

        [Test]
        public void CanDelete()
        {
            Portfolio = TestingGUICode.CreateBasicDataBase();
            Assert.AreEqual(1, ViewModel.DataStore.Funds.Count);
            Assert.AreEqual(1, Portfolio.BankAccounts.Count);
            var item = new NameData("Barclays", "currentAccount");
            SelectItem(item);
            DeleteSelected();
            Assert.AreEqual(0, ViewModel.DataStore.BankAccounts.Count);
            Assert.AreEqual(0, Portfolio.BankAccounts.Count);
        }
    }
}
