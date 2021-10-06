using System.Linq;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancePortfolioDatabase.Tests.TestHelpers;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
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
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            Assert.AreEqual(1, ViewModel.Tabs.Count);
            object tab = ViewModel.Tabs.Single();
            DataNamesViewModel nameModel = tab as DataNamesViewModel;
            Assert.AreEqual(1, nameModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            IPortfolio newData = TestSetupHelper.CreateBasicDataBase();
            ViewModel.UpdateData(newData);

            Assert.AreEqual("TestFilePath", ViewModel.DataStore.FilePath);
            Assert.AreEqual(1, ViewModel.DataStore.FundsThreadSafe.Count);
        }

        [Test]
        public void CanUpdateDataAndRemoveOldTab()
        {
            NameData newNameData = new NameData("Fidelity", "Europe");
            ViewModel.LoadTabFunc(newNameData);

            Assert.AreEqual(2, ViewModel.Tabs.Count);

            IPortfolio newData = TestSetupHelper.CreateBasicDataBase();
            ViewModel.UpdateData(newData);
            Assert.AreEqual(1, ViewModel.Tabs.Count);
            Assert.AreEqual("TestFilePath", ViewModel.DataStore.FilePath);
            Assert.AreEqual(1, ViewModel.DataStore.FundsThreadSafe.Count);
        }

        [Test]
        public void CanAddTab()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();

            NameData newData = new NameData("Fidelity", "China");
            ViewModel.LoadTabFunc(newData);

            Assert.AreEqual(2, ViewModel.Tabs.Count);
            DataNamesViewModel dataNames = DataNames;
            Assert.AreEqual(1, dataNames.DataNames.Count);
            SelectedSecurityViewModel selected = SelectedViewModel(newData);
            Assert.IsNotNull(selected);
            Assert.AreEqual(1, selected.TLVM.Valuations.Count);
        }
    }
}
