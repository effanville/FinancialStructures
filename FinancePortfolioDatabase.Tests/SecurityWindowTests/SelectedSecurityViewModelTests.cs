using System;
using System.Linq;
using NUnit.Framework;
using FinancePortfolioDatabase.Tests.ViewModelExtensions;
using FinancialStructures.FinanceStructures;
using FinancialStructures.Database;
using FinancePortfolioDatabase.Tests.TestHelpers;
using System.Threading;

namespace FinancePortfolioDatabase.Tests.SecurityWindowTests
{
    [TestFixture]
    public class SelectedSecurityViewModelTests : SelectedSecurityTestHelper
    {
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
            var newItem = ViewModel.AddNewItem();
            ViewModel.BeginEdit();
            newItem.Day = new DateTime(2002, 1, 1);
            newItem.Value = 1;
            ViewModel.CompleteEdit(Portfolio);

            Assert.AreEqual(2, ViewModel.TLVM.Valuations.Count);
            Assert.AreEqual(2, Portfolio.FundsThreadSafe.Single().Count());
        }

        [Test]
        public void CanEditValue()
        {
            Assert.AreEqual(1, ViewModel.TLVM.Valuations.Count);
            var item = ViewModel.TLVM.Valuations[0];
            ViewModel.SelectItem(item);
            ViewModel.BeginEdit();
            item.Day = new DateTime(2000, 1, 1);
            item.Value = 1;
            ViewModel.CompleteEdit(Portfolio);

            Assert.AreEqual(1, ViewModel.TLVM.Valuations.Count);
            Assert.AreEqual(1, Portfolio.FundsThreadSafe.Single().Count());
            Assert.AreEqual(new DateTime(2000, 1, 1), Portfolio.FundsThreadSafe.Single().FirstValue().Day);
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
        [RequiresThread(ApartmentState.STA)]
        public void CanDeleteValue()
        {
            Assert.AreEqual(1, ViewModel.TLVM.Valuations.Count);
            ViewModel.SelectItem(ViewModel.TLVM.Valuations[0]);
            ViewModel.DeleteSelected(Portfolio);
            _ = Portfolio.TryGetAccount(Account.Security, Name, out IValueList valueList);
            var security = valueList as ISecurity;
            Assert.AreEqual(0, security.Values.Count());
            Assert.AreEqual(0, security.UnitPrice.Count());
        }
    }
}
