using System;
using System.Linq;
using System.Threading;
using Common.Structure.DataStructures;
using FinancePortfolioDatabase.Tests.TestHelpers;
using FinancePortfolioDatabase.Tests.ViewModelExtensions;
using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using NUnit.Framework;

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
        public void CanAddTradeValue()
        {
            Assert.AreEqual(1, ViewModel.Trades.Count);
            ViewModel.SelectTrade(null);
            SecurityTrade newItem = ViewModel.AddNewTrade();
            ViewModel.BeginEditTrade();
            newItem.Day = new DateTime(2002, 1, 1);
            newItem.NumberShares = 5;
            ViewModel.CompleteEditTrade(Portfolio);

            Assert.AreEqual(2, ViewModel.Trades.Count);
            Assert.AreEqual(1, Portfolio.FundsThreadSafe.Single().Count());
        }

        [Test]
        public void CanEditTradeValue()
        {
            Assert.AreEqual(1, ViewModel.Trades.Count);
            SecurityTrade item = ViewModel.Trades[0];
            ViewModel.SelectTrade(item);
            ViewModel.BeginEditTrade();
            item.Day = new DateTime(2000, 1, 1);
            item.NumberShares = 1;
            ViewModel.CompleteEditTrade(Portfolio);

            Assert.AreEqual(1, ViewModel.Trades.Count);
            Assert.AreEqual(1, Portfolio.FundsThreadSafe.Single().Count());
            Assert.AreEqual(new DateTime(2000, 1, 1), Portfolio.FundsThreadSafe.Single().FirstValue().Day);
        }

        [Test]
        [RequiresThread(ApartmentState.STA)]
        public void CanDeleteTrade()
        {
            Assert.AreEqual(1, ViewModel.Trades.Count);
            ViewModel.SelectTrade(ViewModel.Trades[0]);
            ViewModel.DeleteSelectedTrade(Portfolio);
            _ = Portfolio.TryGetAccount(Account.Security, Name, out IValueList valueList);
            ISecurity security = valueList as ISecurity;
            Assert.AreEqual(0, security.Values.Count());
            Assert.AreEqual(1, security.UnitPrice.Count());
            Assert.AreEqual(0, security.SecurityTrades.Count);
        }

        [Test]
        public void CanAddValue()
        {
            Assert.AreEqual(1, ViewModel.TLVM.Valuations.Count);
            ViewModel.SelectUnitPrice(null);
            DailyValuation newItem = ViewModel.AddNewUnitPrice();
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
            DailyValuation item = ViewModel.TLVM.Valuations[0];
            ViewModel.SelectUnitPrice(item);
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
            ViewModel.SelectUnitPrice(ViewModel.TLVM.Valuations[0]);
            ViewModel.DeleteSelected(Portfolio);
            _ = Portfolio.TryGetAccount(Account.Security, Name, out IValueList valueList);
            ISecurity security = valueList as ISecurity;
            Assert.AreEqual(0, security.Values.Count());
            Assert.AreEqual(0, security.UnitPrice.Count());
            Assert.AreEqual(1, security.SecurityTrades.Count);
        }
    }
}
