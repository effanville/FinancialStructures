using System;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancialStructures.Database;
using FinancePortfolioDatabase.Tests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;
using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;

namespace FinancePortfolioDatabase.Tests
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
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio = TestingGUICode.CreateEmptyDataBase();

            UiGlobals globals = TestingGUICode.CreateGlobalsMock(new FileSystem(), fileMock.Object, dialogMock.Object);
            ViewModel = new StatsCreatorWindowViewModel(Portfolio, TestingGUICode.DummyReportLogger, globals);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
            Portfolio.Clear();
        }
    }
}
