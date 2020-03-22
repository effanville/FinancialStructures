using FinanceCommonViewModels;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;

namespace FPD_UI_UnitTests
{
    public class CommonWindowTests
    {
        [Test]
        public void CanOpenNewDatabase()
        {
            var output = TestingGUICode.CreateBasicDataBase();

            var viewModel = new SingleValueEditWindowViewModel("Dummy", output, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyEditMethods);
        }

        public class SecurityNamesTests
        {
            [Test]
            public void CanOpenNewDatabase()
            {
                var output = TestingGUICode.CreateBasicDataBase();

                var viewModel = new DataNamesViewModel(output, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, TestingGUICode.DummyEditMethods);
            }
        }

        public class SelectedSecurityDataTests
        {
            [Test]
            public void CanOpenNewDatabase()
            {
                var output = TestingGUICode.CreateBasicDataBase();

                var viewModel = new SelectedSingleDataViewModel(output, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyEditMethods, null);
            }
        }
    }
}
