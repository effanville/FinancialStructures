using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels;
using FinancePortfolioDatabase.Tests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;

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
            var fileSystem = new FileSystem();
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("testFilePath");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            MainWindowViewModel viewModel = new MainWindowViewModel(TestingGUICode.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));

            Assert.AreEqual(6, viewModel.Tabs.Count);
        }
    }
}
