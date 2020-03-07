using NUnit.Framework;
using FinanceWindowsViewModels;
using FPD_UI_UnitTests.TestConstruction;
using FinanceCommonViewModels;

namespace FPD_UI_UnitTests
{
    public class CommonWindowTests
    {
        [Test]
        public void CanOpenNewDatabase()
        {
            var output = TestingGUICode.CreateBasicDataBase();

            var viewModel = new SingleValueEditWindowViewModel("Dummy", output.Item1, output.Item2, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyEditMethods);
        }

        public class SecurityNamesTests
        {
            [Test]
            public void CanOpenNewDatabase()
            {
                var output = TestingGUICode.CreateBasicDataBase();

                var viewModel = new DataNamesViewModel(output.Item1, output.Item2, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, TestingGUICode.DummyEditMethods);
            }
        }

        public class SelectedSecurityDataTests
        {
            [Test]
            public void CanOpenNewDatabase()
            {
                var output = TestingGUICode.CreateBasicDataBase();

                var viewModel = new SelectedSingleDataViewModel(output.Item1, output.Item2, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyEditMethods, null);
            }
        }
    }
}
