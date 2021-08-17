using System;
using FinancialStructures.Database;
using Moq;
using NUnit.Framework;
using Common.UI.Services;
using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.NamingStructures;
using Common.UI;

namespace FinancePortfolioDatabase.Tests.TestHelpers
{
    public abstract class SelectedSingleDataViewModelHelper
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

        protected SelectedSingleDataViewModel ViewModel
        {
            get;
            private set;
        }

        protected Account AccountType
        {
            get;
            set;
        } = Account.Security;
        protected NameData Name
        {
            get;
            set;
        }

        [SetUp]
        public virtual void Setup()
        {
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            Portfolio = TestSetupHelper.CreateBasicDataBase();

            UiGlobals globals = TestSetupHelper.CreateGlobalsMock(new FileSystem(), fileMock.Object, dialogMock.Object, TestSetupHelper.DummyReportLogger);
            ViewModel = new SelectedSingleDataViewModel(Portfolio, DataUpdater, globals, new NameData("Barclays", "currentAccount"), AccountType);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
            Portfolio.Clear();
        }
    }
}
