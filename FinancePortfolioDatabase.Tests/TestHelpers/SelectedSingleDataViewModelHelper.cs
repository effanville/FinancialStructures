using System;
using FinancialStructures.Database;
using FinancePortfolioDatabase.Tests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;
using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using System.Windows.Controls;
using System.Linq;

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
        protected NameData Name
        {
            get;
            set;
        }

        public DailyValuation AddNewItem()
        {
            SelectItem(null);
            ViewModel.AddDefaultDataCommand?.Execute(new AddingNewItemEventArgs());
            ViewModel.SelectedData.Add(new DailyValuation());
            var newItem = ViewModel.SelectedData.Last();
            SelectItem(newItem);
            BeginEdit();

            return newItem;
        }

        public void SelectItem(DailyValuation valueToSelect)
        {
            ViewModel.SelectionChangedCommand?.Execute(valueToSelect);
        }

        public void BeginEdit()
        {
            ViewModel.PreEditCommand?.Execute(null);
        }

        public void CompleteEdit()
        {
            ViewModel.EditDataCommand?.Execute(null);
            ViewModel.UpdateData(Portfolio);
        }

        public void DeleteSelected()
        {
            ViewModel.DeleteValuationCommand?.Execute(null);
            ViewModel.UpdateData(Portfolio);
        }

        [SetUp]
        public void Setup()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio = TestingGUICode.CreateBasicDataBase();

            UiGlobals globals = TestingGUICode.CreateGlobalsMock(new FileSystem(), fileMock.Object, dialogMock.Object);
            ViewModel = new SelectedSingleDataViewModel(Portfolio, DataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), AccountType);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
            Portfolio.Clear();
        }
    }
}
