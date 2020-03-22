using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;

namespace FPD_UI_UnitTests
{
    public class MainWindowTests
    {
        [Test]
        public void CanOpenNewDatabase()
        {
            var output = TestingGUICode.CreateBasicDataBase();
        }
    }
}
