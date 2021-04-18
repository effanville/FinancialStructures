using System;
using System.Linq;
using System.Threading;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using FinancePortfolioDatabase.Tests.TestConstruction;
using NUnit.Framework;
using StructureCommon.DataStructures;
using FinancialStructures.FinanceStructures;

namespace FinancePortfolioDatabase.Tests.CommonWindowTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class SelectedSingleDataViewModelTests : SelectedSingleDataViewModelHelper
    {
        [SetUp]
        public void SetBankAccount()
        {
            AccountType = Account.BankAccount;
            Name = new NameData("Barclays", "currentAccount");
            base.Setup();
        }

        [Test]
        public void CanOpenWindow()
        {
            Assert.AreEqual(1, ViewModel.SelectedData.Count);
        }

        [Test]
        public void CanAddValue()
        {

            Assert.AreEqual(1, ViewModel.SelectedData.Count);
            SelectItem(null);
            var newItem = AddNewItem();

            BeginEdit();
            newItem.Day = new DateTime(2002, 1, 1);
            newItem.Value = 1;
            CompleteEdit();

            Assert.AreEqual(2, ViewModel.SelectedData.Count);
            Assert.AreEqual(2, Portfolio.BankAccounts.Single().Count());
        }

        [Test]
        public void CanEditValue()
        {
            Assert.AreEqual(1, ViewModel.SelectedData.Count);
            var item = ViewModel.SelectedData[0];
            SelectItem(item);
            BeginEdit();
            item.Day = new DateTime(2000, 1, 1);
            item.Value = 1;
            CompleteEdit();

            Assert.AreEqual(1, ViewModel.SelectedData.Count);
            Assert.AreEqual(1, Portfolio.Funds.Single().Count());
            Assert.AreEqual(new DateTime(2000, 1, 1), Portfolio.Funds.Single().FirstValue().Day);
        }


        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanAddFromCSV()
        {
            Assert.AreEqual(1, ViewModel.SelectedData.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanWriteToCSV()
        {
            Assert.AreEqual(1, ViewModel.SelectedData.Count);
        }

        [Test]
        public void CanDeleteValue()
        {
            Assert.AreEqual(1, ViewModel.SelectedData.Count);

            SelectItem(ViewModel.SelectedData[0]);
            DeleteSelected();
            _ = Portfolio.TryGetAccount(AccountType, Name, out IValueList bankAccount);
            Assert.AreEqual(0, bankAccount.Values.Count());
            Assert.AreEqual(0, ViewModel.SelectedData.Count);
        }
    }
}
