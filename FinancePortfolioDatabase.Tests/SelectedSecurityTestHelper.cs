using System;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancialStructures.Database;
using FinancePortfolioDatabase.Tests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;
using System.IO.Abstractions;
using FinancialStructures.NamingStructures;

namespace FinancePortfolioDatabase.Tests
{
    public abstract class SelectedSecurityTestHelper
    {
        private Action<Action<IPortfolio>> DataUpdater
        {
            get
            {
                return action => action(Portfolio);
            }
        }

        protected IPortfolio Portfolio
        {
            get;
            set;
        }

        protected NameData Name
        {
            get;
            set;
        }

        protected SelectedSecurityViewModel ViewModel
        {
            get;
            private set;
        }

        [SetUp]
        public void Setup()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio = TestingGUICode.CreateBasicDataBase();
            Name = new NameData("Fidelity", "China");

            UiGlobals globals = TestingGUICode.CreateGlobalsMock(new FileSystem(), fileMock.Object, dialogMock.Object);
            ViewModel = new SelectedSecurityViewModel(Portfolio, DataUpdater, TestingGUICode.DummyReportLogger, globals, Name);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
            Portfolio.Clear();
        }
    }
}
