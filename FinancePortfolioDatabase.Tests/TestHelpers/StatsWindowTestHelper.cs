using FinancialStructures.Database;
using Moq;
using NUnit.Framework;
using Common.UI.Services;
using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;

namespace FinancePortfolioDatabase.Tests.TestHelpers
{
    public abstract class StatsWindowTestHelper
    {
        protected IPortfolio Portfolio
        {
            get;
            set;
        }

        protected StatsCreatorWindowViewModel ViewModel
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

            UiGlobals globals = TestSetupHelper.CreateGlobalsMock(new FileSystem(), fileMock.Object, dialogMock.Object);
            ViewModel = new StatsCreatorWindowViewModel(Portfolio, TestSetupHelper.DummyReportLogger, globals);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
            Portfolio.Clear();
        }
    }
}
