using FinanceWindowsViewModels;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;

namespace FPD_UI_UnitTests
{
    /// <summary>
    /// Tests to ensure that the stats window displays what it should do.
    /// </summary>
    public class StatsWindowTests
    {
        /// <summary>
        /// The defaults are loaded correctly.
        /// </summary>
        [Test]
        [Ignore("not yet implemented")]
        public void CanLoadWithNames()
        {
            var output = TestingGUICode.CreateBasicDataBase();
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var viewModel = new StatsCreatorWindowViewModel(output, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);
        }
    }
}
