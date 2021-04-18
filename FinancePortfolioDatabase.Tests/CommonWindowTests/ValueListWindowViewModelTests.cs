using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database.Implementation;
using FinancialStructures.NamingStructures;
using NUnit.Framework;
using FinancePortfolioDatabase.Tests.ViewModelExtensions;
using FinancePortfolioDatabase.Tests.TestHelpers;

namespace FinancePortfolioDatabase.Tests.CommonWindowTests
{
    /// <summary>
    /// Tests for window displaying single data stream data.
    /// </summary>
    [TestFixture]
    public class ValueListWindowViewModelTests : ValueListWindowViewModelTestHelper
    {
        [Test]
        public void CanLoadSuccessfully()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            Assert.AreEqual(1, ViewModel.Tabs.Count);
            DataNamesViewModel nameModel = ViewModel.DataNames();
            Assert.AreEqual(1, nameModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            Portfolio newData = TestSetupHelper.CreateBasicDataBase();
            ViewModel.UpdateData(newData);

            Assert.AreEqual("TestFilePath", ViewModel.DataStore.FilePath);
            Assert.AreEqual(1, ViewModel.DataStore.BankAccountsThreadSafe.Count);
        }

        [Test]
        public void CanUpdateDataAndRemoveOldTab()
        {
            NameData newNameData = new NameData("Fidelity", "Europe");
            ViewModel.LoadTabFunc(newNameData);

            Assert.AreEqual(2, ViewModel.Tabs.Count);

            Portfolio newData = TestSetupHelper.CreateBasicDataBase();
            ViewModel.UpdateData(newData);
            Assert.AreEqual(1, ViewModel.Tabs.Count);
            Assert.AreEqual("TestFilePath", ViewModel.DataStore.FilePath);
            Assert.AreEqual(1, ViewModel.DataStore.BankAccountsThreadSafe.Count);
        }

        [Test]
        public void CanAddTab()
        {
            Portfolio Portfolio = TestSetupHelper.CreateBasicDataBase();
            NameData newData = new NameData("Fidelity", "China");
            ViewModel.LoadTabFunc(newData);

            Assert.AreEqual(2, ViewModel.Tabs.Count);
        }
    }
}
