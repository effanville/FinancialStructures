using FinanceWindowsViewModels;
using FinancialStructures.Database.Implementation;
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
            Portfolio output = TestingGUICode.CreateBasicDataBase();
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            StatsCreatorWindowViewModel viewModel = new StatsCreatorWindowViewModel(output, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);
        }
    }
}
