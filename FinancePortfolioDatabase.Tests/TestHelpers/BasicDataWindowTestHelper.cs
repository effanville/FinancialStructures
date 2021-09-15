using System.IO.Abstractions;
using Common.UI;
using Common.UI.Services;
using FinancePortfolioDatabase.GUI.ViewModels;
using FinancialStructures.Database;
using Moq;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests.TestHelpers
{
    internal class BasicDataWindowTestHelper
    {
        private IPortfolio fPortfolio;

        protected IPortfolio Portfolio
        {
            get
            {
                return fPortfolio;
            }
            set
            {
                fPortfolio = value;
                ViewModel?.UpdateData(fPortfolio);
            }
        }

        protected BasicDataViewModel ViewModel
        {
            get;
            private set;
        }

        [SetUp]
        public void Setup()
        {
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            Portfolio = TestSetupHelper.CreateEmptyDataBase();

            UiGlobals globals = TestSetupHelper.CreateGlobalsMock(new FileSystem(), fileMock.Object, dialogMock.Object, TestSetupHelper.DummyReportLogger);
            ViewModel = new BasicDataViewModel(globals, null, Portfolio);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
            Portfolio.Clear();
        }
    }
}
