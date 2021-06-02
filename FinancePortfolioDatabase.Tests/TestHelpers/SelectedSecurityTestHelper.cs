using System;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancialStructures.Database;
using Moq;
using NUnit.Framework;
using UICommon.Services;
using System.IO.Abstractions;
using FinancialStructures.NamingStructures;

namespace FinancePortfolioDatabase.Tests.TestHelpers
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
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            Name = new NameData("Fidelity", "China");

            UiGlobals globals = TestSetupHelper.CreateGlobalsMock(new FileSystem(), fileMock.Object, dialogMock.Object);
            ViewModel = new SelectedSecurityViewModel(Portfolio, DataUpdater, TestSetupHelper.DummyReportLogger, globals, Name);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
            Portfolio.Clear();
        }
    }
}
