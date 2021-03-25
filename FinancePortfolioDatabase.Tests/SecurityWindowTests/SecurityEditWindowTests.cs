using System.Linq;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database.Implementation;
using FinancialStructures.NamingStructures;
using FinancePortfolioDatabase.Tests.TestConstruction;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests.SecurityWindowTests
{
    /// <summary>
    /// Tests for window displaying security data.
    /// </summary>
    [TestFixture]
    public class SecurityEditWindowTests : SecurityWindowTestHelper
    {
        [Test]
        public void CanLoadSuccessfully()
        {
            Portfolio = TestingGUICode.CreateBasicDataBase();
            Assert.AreEqual(1, ViewModel.Tabs.Count);
            object tab = ViewModel.Tabs.Single();
            DataNamesViewModel nameModel = tab as DataNamesViewModel;
            Assert.AreEqual(1, nameModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            Portfolio newData = TestingGUICode.CreateBasicDataBase();
            ViewModel.UpdateData(newData);

            Assert.AreEqual("TestFilePath", ViewModel.DataStore.FilePath);
            Assert.AreEqual(1, ViewModel.DataStore.Funds.Count);
        }

        [Test]
        public void CanUpdateDataAndRemoveOldTab()
        {
            NameData newNameData = new NameData("Fidelity", "Europe");
            ViewModel.LoadTabFunc(newNameData);

            Assert.AreEqual(2, ViewModel.Tabs.Count);

            Portfolio newData = TestingGUICode.CreateBasicDataBase();
            ViewModel.UpdateData(newData);
            Assert.AreEqual(1, ViewModel.Tabs.Count);
            Assert.AreEqual("TestFilePath", ViewModel.DataStore.FilePath);
            Assert.AreEqual(1, ViewModel.DataStore.Funds.Count);
        }

        [Test]
        public void CanAddTab()
        {
            Portfolio = TestingGUICode.CreateBasicDataBase();

            NameData newData = new NameData("Fidelity", "China");
            ViewModel.LoadTabFunc(newData);

            Assert.AreEqual(2, ViewModel.Tabs.Count);
        }
    }
}
