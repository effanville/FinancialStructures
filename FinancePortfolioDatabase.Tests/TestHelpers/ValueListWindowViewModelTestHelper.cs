using System;
using FinancialStructures.Database;
using Moq;
using NUnit.Framework;
using Common.UI.Services;
using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using Common.UI;

namespace FinancePortfolioDatabase.Tests.TestHelpers
{
    public abstract class ValueListWindowViewModelTestHelper
    {
        private Action<Action<IPortfolio>> DataUpdater
        {
            get
            {
                return action => action(Portfolio);
            }
        }

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

        protected ValueListWindowViewModel ViewModel
        {
            get;
            private set;
        }

        protected Account AccountType
        {
            get;
            set;
        } = Account.Security;

        [SetUp]
        public void Setup()
        {
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            Portfolio = TestSetupHelper.CreateEmptyDataBase();

            UiGlobals globals = TestSetupHelper.CreateGlobalsMock(new FileSystem(), fileMock.Object, dialogMock.Object, TestSetupHelper.DummyReportLogger);
            ViewModel = new ValueListWindowViewModel("Title", Portfolio, DataUpdater, globals, AccountType);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
            Portfolio.Clear();
        }
    }
}
