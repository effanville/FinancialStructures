using NUnit.Framework;
using FinanceWindowsViewModels;
using FPD_UI_UnitTests.TestConstruction;

namespace FPD_UI_UnitTests
{
    public class OptionsToolbarTests
    {
        [Test]
        [Ignore("Archecture Does not allow - requires DialogBox interaction abstraction")]
        public void CanOpenNewDatabase()
        {
            var output = TestingGUICode.CreateBasicDataBase();

            var viewModel = new OptionsToolbarViewModel(output, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger);
            //viewModel.NewDatabaseCommand.Execute(1);
            //Check that data held is an empty database
        }

        [Test]
        [Ignore("Archecture Does not allow - requires DialogBox interaction abstraction")]
        public void CanOpenDatabase()
        {
            var output = TestingGUICode.CreateBasicDataBase();

            var viewModel = new OptionsToolbarViewModel(output, TestingGUICode.DummyDataUpdater, TestingGUICode.DummyReportLogger);
            //viewModel.NewDatabaseCommand.Execute(1);
            //Input prespecified example database
            //Check that data held is the expected database.
        }
    }
}
