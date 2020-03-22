using FinanceWindowsViewModels;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;

namespace FPD_UI_UnitTests
{
    public class StatsWindowTests
    {
        [Test]
        public void CanLoadWithNames()
        {
            var output = TestingGUICode.CreateBasicDataBase();

            var viewModel = new StatsCreatorWindowViewModel(output, TestingGUICode.DummyReportLogger);
        }
    }
}
