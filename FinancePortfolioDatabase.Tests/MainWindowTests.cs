using FinancePortfolioDatabase.GUI.ViewModels;
using FinancePortfolioDatabase.Tests.TestConstruction;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests
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
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("testFilePath");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            MainWindowViewModel viewModel = new MainWindowViewModel(fileMock.Object, dialogMock.Object);

            Assert.AreEqual(6, viewModel.Tabs.Count);
        }
    }
}
