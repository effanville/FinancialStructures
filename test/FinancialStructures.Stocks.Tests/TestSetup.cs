using System.IO.Abstractions;

using Effanville.FinancialStructures.Stocks.Persistence.Database;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Effanville.FinancialStructures.Stocks.Tests
{
    [TestFixture]
    public class Setup
    {
        [Test]
        public void SetupDBFromWeb()
        {
            var fileSystem = new FileSystem();
            var dbContext = new DatabaseFactory(new LoggerFactory())
                .GetDbBuilder(fileSystem, "C:\\dev\\SampleDB2.db")
                .EnsureCreated()
                .WithDataSources();
        }
    }
}