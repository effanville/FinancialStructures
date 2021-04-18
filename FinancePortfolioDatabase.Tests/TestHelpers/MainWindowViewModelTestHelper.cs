using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels;
using FinancialStructures.Database;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests.TestHelpers
{
    public abstract class MainWindowViewModelTestHelper
    {
        protected string PortfolioFilePath = null;

        protected IPortfolio Portfolio
        {
            get
            {
                return ViewModel?.ProgramPortfolio;
            }
        }

        protected MainWindowViewModel ViewModel
        {
            get;
            private set;
        }

        [SetUp]
        public void Setup()
        {
            string filePath = PortfolioFilePath ?? "nothing";
            UiGlobals globals = TestSetupHelper.CreateGlobalsMock(new FileSystem(), TestSetupHelper.CreateFileMock(filePath).Object, TestSetupHelper.CreateDialogMock().Object);
            ViewModel = new MainWindowViewModel(globals);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
        }
    }
}
