using FinancialStructures.Database;
using Moq;
using NUnit.Framework;
using Common.UI.Services;
using System.IO.Abstractions;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using Common.UI;
using FinancePortfolioDatabase.GUI.Configuration;

namespace FinancePortfolioDatabase.Tests.TestHelpers
{
    public abstract class StatsWindowTestHelper
    {
        protected IPortfolio Portfolio
        {
            get;
            set;
        }

        protected StatsViewModel ViewModel
        {
            get;
            set;
        }

        protected UiGlobals Globals
        {
            get;
            private set;
        }

        protected IConfiguration VMConfiguration
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

            Globals = TestSetupHelper.CreateGlobalsMock(new FileSystem(), fileMock.Object, dialogMock.Object);
            VMConfiguration = new StatsDisplayConfiguration();
            ViewModel = new StatsViewModel(Portfolio, null, Globals, VMConfiguration, Account.All);
        }

        [TearDown]
        public void TearDown()
        {
            ViewModel = null;
            Portfolio.Clear();
        }
    }
}
