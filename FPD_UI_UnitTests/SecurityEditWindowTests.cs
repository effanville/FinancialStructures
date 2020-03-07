using NUnit.Framework;
using FinanceWindowsViewModels;
using FPD_UI_UnitTests.TestConstruction;

namespace FPD_UI_UnitTests
{
    public class SecurityEditWindowTests
    {
        [Test]
        public void CanLoadWithNames()
        {
            var output = TestingGUICode.CreateBasicDataBase();

            var viewModel = new SecurityEditWindowViewModel(output.Item1, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger);
        }

        [Test]
        public void CanUpdateData()
        {
            var output = TestingGUICode.CreateEmptyDataBase();

            var viewModel = new SecurityEditWindowViewModel(output.Item1, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger);
            var newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(output.Item1, output.Item2);
        }

        public class SecurityNamesTests
        {
            [Test]
            public void CanOpenNewDatabase()
            {
                var output = TestingGUICode.CreateBasicDataBase();

                var viewModel = new SecurityNamesViewModel(output.Item1, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab);
            }
        }

        public class SelectedSecurityDataTests
        {
            [Test]
            public void CanOpenNewDatabase()
            {
                var output = TestingGUICode.CreateBasicDataBase();

                var viewModel = new SelectedSecurityViewModel(output.Item1, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger, null);
            }
        }
    }
}
