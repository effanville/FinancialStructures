using NUnit.Framework;
using FinanceWindowsViewModels;
using FPD_UI_UnitTests.TestConstruction;

namespace FPD_UI_UnitTests
{
    public class StatsWindowTests
    {
        [Test]
        public void CanLoadWithNames()
        {
            var output = TestingGUICode.CreateBasicDataBase();

            var viewModel = new StatsCreatorWindowViewModel(output.Item1, output.Item2, TestingGUICode.DummyReportLogger);
        }
    }
}
