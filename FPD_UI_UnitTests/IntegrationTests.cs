using NUnit.Framework;
using FinanceWindowsViewModels;
using FPD_UI_UnitTests.TestConstruction;

namespace FPD_UI_UnitTests
{
    public class IntegrationTests
    {
        [Test]
        public void CanOpenNewDatabase()
        {
            var output = TestingGUICode.CreateBasicDataBase();
        }
    }
}
