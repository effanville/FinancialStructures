using System;
using System.IO.Abstractions;
using Common.UI;
using Common.UI.Services;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using Moq;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests.TestHelpers
{
    public abstract class DataNamesViewTestHelper
    {
        private Action<Action<IPortfolio>> DataUpdater => action => action(Portfolio);

        private IPortfolio fPortfolio;

        protected IPortfolio Portfolio
        {
            get => fPortfolio;
            set
            {
                fPortfolio = value;
                ViewModel?.UpdateData(fPortfolio);
            }
        }

        protected DataNamesViewModel ViewModel
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

            UiGlobals globals = TestSetupHelper.CreateGlobalsMock(new FileSystem(), fileMock.Object, dialogMock.Object);
            ViewModel = new DataNamesViewModel(Portfolio, DataUpdater, TestSetupHelper.DummyReportLogger, null, obj => { }, AccountType);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
            Portfolio.Clear();
        }
    }
}
