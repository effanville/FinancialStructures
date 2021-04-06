using System;
using FinancialStructures.Database;
using FinancePortfolioDatabase.Tests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;
using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.NamingStructures;
using StructureCommon.DisplayClasses;
using System.Linq;
using System.Windows.Controls;

namespace FinancePortfolioDatabase.Tests
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

        public NameData AddNewItem()
        {

            return newItem.Instance;
        }

        public void SelectItem(NameData name)
        {
            ViewModel.SelectionChangedCommand?.Execute(new SelectableEquatable<NameData>(name, false));
        }

        public void BeginEdit()
        {
        }

        public void CompleteEdit()
        {
        }

        public void DeleteSelected()
        {
            ViewModel.DeleteCommand?.Execute(null);
        }

        [SetUp]
        public void Setup()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio = TestingGUICode.CreateEmptyDataBase();

            UiGlobals globals = TestingGUICode.CreateGlobalsMock(new FileSystem(), fileMock.Object, dialogMock.Object);
            ViewModel = new SelectedSingleDataViewModel(Portfolio, DataUpdater, TestingGUICode.DummyReportLogger, obj => { }, AccountType);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
            Portfolio.Clear();
        }
    }
}
