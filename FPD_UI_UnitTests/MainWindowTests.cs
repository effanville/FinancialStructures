using FinanceWindowsViewModels;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;

namespace FPD_UI_UnitTests
{
    /// <summary>
    /// Tests for the MainWindowViewModel.
    /// </summary>
    public class MainWindowTests
    {
        /// <summary>
        /// Checks whether the MainWindowViewModel can be loaded successfully.
        /// </summary>
        [Test]
        public void CanSuccessfullyCreateMainViewModel()
        {
            var fileMock = TestingGUICode.CreateFileMock("testFilePath");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var viewModel = new MainWindowViewModel(fileMock.Object, dialogMock.Object);

            Assert.AreEqual(6, viewModel.Tabs.Count);
        }
    }
}
