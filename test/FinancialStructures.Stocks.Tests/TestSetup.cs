using System.IO.Abstractions;
using System.Threading.Tasks;

using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Stocks.Persistence.Database;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Stocks.Tests
{
    [TestFixture]
    public class Setup
    {
        [Test]
        public async Task SetupDBFromWeb()
        {
            var logger = new LogReporter(null, true);
            var fileSystem = new FileSystem();
            var dbContext = new DatabaseFactory()
                .GetDbBuilder(fileSystem, "C:\\dev\\SampleDB2.db")
                .EnsureCreated()
                .WithDataSources(logger);
        }
    }
}