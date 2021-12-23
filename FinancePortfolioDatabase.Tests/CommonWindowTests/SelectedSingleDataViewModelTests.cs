using System;
using System.Linq;
using Common.Structure.DataStructures;
using FinancePortfolioDatabase.Tests.TestHelpers;
using FinancePortfolioDatabase.Tests.ViewModelExtensions;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests.CommonWindowTests
{
    [TestFixture]
    public class SelectedSingleDataViewModelTests : SelectedSingleDataViewModelHelper
    {
        [SetUp]
        public override void Setup()
        {
            AccountType = Account.BankAccount;
            Name = new NameData("Barclays", "currentAccount");
            base.Setup();
        }

        [Test]
        public void CanOpenWindow()
        {
            Assert.AreEqual(1, ViewModel.TLVM.Valuations.Count);
        }

        [Test]
        public void CanAddValue()
        {
            Assert.AreEqual(1, ViewModel.TLVM.Valuations.Count);
            ViewModel.SelectItem(null);
            DailyValuation newItem = ViewModel.AddNewItem();

            ViewModel.BeginEdit();
            newItem.Day = new DateTime(2002, 1, 1);
            newItem.Value = 1;
            ViewModel.CompleteEdit(Portfolio);

            Assert.AreEqual(2, ViewModel.TLVM.Valuations.Count);
            Assert.AreEqual(2, Portfolio.BankAccountsThreadSafe.Single().Count());
        }

        [Test]
        public void CanEditValue()
        {
            Assert.AreEqual(1, ViewModel.TLVM.Valuations.Count);
            DailyValuation item = ViewModel.TLVM.Valuations[0];
            ViewModel.SelectItem(item);
            ViewModel.BeginEdit();
            item.Day = new DateTime(2000, 1, 1);
            item.Value = 1;
            ViewModel.CompleteEdit(Portfolio);

            Assert.AreEqual(1, ViewModel.TLVM.Valuations.Count);
            Assert.AreEqual(1, Portfolio.BankAccountsThreadSafe.Single().Count());
            Assert.AreEqual(new DateTime(2000, 1, 1), Portfolio.BankAccountsThreadSafe.Single().FirstValue().Day);
        }


        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanAddFromCSV()
        {
            Assert.AreEqual(1, ViewModel.TLVM.Valuations.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanWriteToCSV()
        {
            Assert.AreEqual(1, ViewModel.TLVM.Valuations.Count);
        }

        [Test]
        public void CanDeleteValue()
        {
            Assert.AreEqual(1, ViewModel.TLVM.Valuations.Count);

            ViewModel.SelectItem(ViewModel.TLVM.Valuations[0]);
            ViewModel.DeleteSelected(Portfolio);
            _ = Portfolio.TryGetAccount(AccountType, Name, out IValueList bankAccount);
            Assert.AreEqual(0, bankAccount.Values.Count());
            Assert.AreEqual(0, ViewModel.TLVM.Valuations.Count);
        }
    }
}
