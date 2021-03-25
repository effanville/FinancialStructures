using System;
using System.Linq;
using System.Threading;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancialStructures.Database;
using FinancialStructures.Database.Implementation;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using FinancePortfolioDatabase.Tests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;

namespace FinancePortfolioDatabase.Tests.SecurityWindowTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
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
            SecurityDayData newValue = new SecurityDayData(new DateTime(2002, 1, 1), 1, 1, 1);
            ViewModel.SelectedSecurityData.Add(newValue);

            ViewModel.fOldSelectedValues = newValue.Copy();

            var dataGridArgs = TestingGUICode.CreateRowArgs(ViewModel.SelectedSecurityData.Last());
            ViewModel.AddEditSecurityDataCommand.Execute(dataGridArgs);
            Assert.AreEqual(2, ViewModel.SelectedSecurityData.Count);
            Assert.AreEqual(2, Portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanEditValue()
        {
            Assert.AreEqual(1, ViewModel.SelectedSecurityData.Count);
            ViewModel.fOldSelectedValues = ViewModel.SelectedSecurityData[0].Copy();
            SecurityDayData newValue = new SecurityDayData(new DateTime(2000, 1, 1), 1, 1, 1);
            ViewModel.SelectedSecurityData[0] = newValue;

            var dataGridArgs = TestingGUICode.CreateRowArgs(ViewModel.SelectedSecurityData.Last());
            ViewModel.AddEditSecurityDataCommand.Execute(dataGridArgs);
            Assert.AreEqual(1, ViewModel.SelectedSecurityData.Count);
            Assert.AreEqual(1, Portfolio.Funds.Single().Count());
            Assert.AreEqual(new DateTime(2000, 1, 1), Portfolio.Funds.Single().FirstValue().Day);
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
            ViewModel.fOldSelectedValues = ViewModel.SelectedSecurityData.Single();
            Assert.AreEqual(1, ViewModel.SelectedSecurityData.Count);

            ViewModel.DeleteValuationCommand.Execute(1);

            Assert.AreEqual(0, Portfolio.Funds.Single().Count());
        }
    }
}
