using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using FinancialStructures.Database.Implementation;
using FinancePortfolioDatabase.Tests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;

namespace FinancePortfolioDatabase.Tests
{
    /// <summary>
    /// Tests to ensure that the stats window displays what it should do.
    /// </summary>
    public class StatsWindowTests
    {
        /// <summary>
        /// The defaults are loaded correctly.
        /// </summary>
        [Test]
        [Ignore("not yet implemented")]
        public void CanLoadWithNames()
        {
            Portfolio output = TestingGUICode.CreateBasicDataBase();
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            StatsCreatorWindowViewModel viewModel = new StatsCreatorWindowViewModel(output, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);
        }
    }
}
