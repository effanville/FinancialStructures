using System;
using System.Linq;
using NUnit.Framework;
using FinancePortfolioDatabase.Tests.ViewModelExtensions;
using FinancialStructures.FinanceStructures;
using FinancialStructures.Database;
using FinancePortfolioDatabase.Tests.TestHelpers;

namespace FinancePortfolioDatabase.Tests.SecurityWindowTests
{
    [TestFixture]
    public class SelectedSecurityViewModelTests : SelectedSecurityTestHelper
    {
        [Test]
        public void CanOpenWindow()
        {
            Assert.AreEqual(1, ViewModel.SelectedSecurityData.Count);
        }

        [Test]
        public void CanAddValue()
        {
            Assert.AreEqual(1, ViewModel.SelectedSecurityData.Count);
            ViewModel.SelectItem(null);
            var newItem = ViewModel.AddNewItem();
            ViewModel.BeginEdit();
            newItem.Date = new DateTime(2002, 1, 1);
            newItem.NewInvestment = 1;
            newItem.ShareNo = 1;
            newItem.UnitPrice = 1;
            ViewModel.CompleteEdit(Portfolio);

            Assert.AreEqual(2, ViewModel.SelectedSecurityData.Count);
            Assert.AreEqual(2, Portfolio.FundsThreadSafe.Single().Count());
        }

        [Test]
        public void CanEditValue()
        {
            Assert.AreEqual(1, ViewModel.SelectedSecurityData.Count);
            var item = ViewModel.SelectedSecurityData[0];
            ViewModel.SelectItem(item);
            ViewModel.BeginEdit();
            item.Date = new DateTime(2000, 1, 1);
            item.NewInvestment = 1;
            item.ShareNo = 1;
            item.UnitPrice = 1;
            ViewModel.CompleteEdit(Portfolio);

            Assert.AreEqual(1, ViewModel.SelectedSecurityData.Count);
            Assert.AreEqual(1, Portfolio.FundsThreadSafe.Single().Count());
            Assert.AreEqual(new DateTime(2000, 1, 1), Portfolio.FundsThreadSafe.Single().FirstValue().Day);
        }

        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanAddFromCSV()
        {
            Assert.AreEqual(1, ViewModel.SelectedSecurityData.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanWriteToCSV()
        {
            Assert.AreEqual(1, ViewModel.SelectedSecurityData.Count);
        }

        [Test]
        public void CanDeleteValue()
        {
            Assert.AreEqual(1, ViewModel.SelectedSecurityData.Count);
            ViewModel.SelectItem(ViewModel.SelectedSecurityData[0]);
            ViewModel.DeleteSelected(Portfolio);
            _ = Portfolio.TryGetAccount(Account.Security, Name, out IValueList valueList);
            var security = valueList as ISecurity;
            Assert.AreEqual(0, security.Values.Count());
            Assert.AreEqual(0, security.Shares.Count());
            Assert.AreEqual(0, security.UnitPrice.Count());
            Assert.AreEqual(0, security.Investments.Count());
            Assert.AreEqual(0, ViewModel.SelectedSecurityData.Count);
        }
    }
}
