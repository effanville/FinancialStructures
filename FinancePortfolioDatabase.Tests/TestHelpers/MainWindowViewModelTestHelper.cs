using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Common.UI;
using FinancePortfolioDatabase.GUI.ViewModels;
using FinancialStructures.Database;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests.TestHelpers
{
    public abstract class MainWindowViewModelTestHelper
    {

        protected IPortfolio Portfolio => ViewModel?.ProgramPortfolio;

        protected MainWindowViewModel ViewModel
        {
            get;
            private set;
        }

        [SetUp]
        public void Setup()
        {
            MockFileSystem tempFileSystem = new MockFileSystem();
            string file = File.ReadAllText(TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml");
            string testPath = "c:/temp/database.xml";

            tempFileSystem.AddFile(testPath, new MockFileData(file));

            UiGlobals globals = TestSetupHelper.CreateGlobalsMock(tempFileSystem, TestSetupHelper.CreateFileMock(testPath).Object, TestSetupHelper.CreateDialogMock().Object);
            ViewModel = new MainWindowViewModel(globals);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
        }
    }
}
