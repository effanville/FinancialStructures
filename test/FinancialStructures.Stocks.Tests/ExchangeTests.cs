using System;
using System.IO.Abstractions;
using System.Linq;

using NUnit.Framework;

using System.IO.Abstractions.TestingHelpers;
using System.Net;

using Effanville.Common.Structure.Reporting;

using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.Persistence;
using FinancialStructures.Stocks.Implementation;
using FinancialStructures.Stocks.Persistence;

namespace FinancialStructures.Stocks.Tests
{
    internal sealed class ExchangeTests
    {
        public string db = @"<?xml version=""1.0"" encoding=""utf-8""?>
<StockExchange xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Stocks>
    <Stock>
      <Name>
        <Ticker>JMAT</Ticker>
        <Company>Johnson Matthew</Company>
        <Name />
        <Url>https://uk.finance.yahoo.com/quote/JMAT.L</Url>
        <Currency />
        <Sectors />
      </Name>
      <Valuations />
    </Stock>
  </Stocks>
</StockExchange>";

        [Test]
        public void Test()
        {
            string filePath = "c:/temp/example.xml";
            var reports = new ErrorReports();

            void reportAction(ReportSeverity severity, ReportType reportType, string location, string text)
            {
                reports.AddErrorReport(severity, reportType, location, text);
            }

            var logger = new LogReporter(reportAction);
            var fileSystem = new MockFileSystem();

            fileSystem.AddFile(filePath, db);

            var startDate = new DateTime(2010, 1, 1);
            var endDate = new DateTime(2023, 1, 1);
            var persistence = new XmlExchangePersistence();
            var exchange = persistence.Load(new XmlFilePersistenceOptions(filePath, fileSystem), logger);
            exchange.Download(startDate, endDate, logger).Wait();
            persistence.Save(exchange, new XmlFilePersistenceOptions("c:/temp/example2.xml", fileSystem), logger);
        }

        [Test]
        public void AddValueTest()
        {
            string filePath = "c:/temp/example.xml";
            var reports = new ErrorReports();

            void reportAction(ReportSeverity severity, ReportType reportType, string location, string text)
            {
                reports.AddErrorReport(severity, reportType, location, text);
            }

            var logger = new LogReporter(reportAction);
            var fileSystem = new MockFileSystem();

            fileSystem.AddFile(filePath, db);

            var startDate = new DateTime(2010, 1, 1);
            var endDate = new DateTime(2020, 1, 1);
            var persistence = new XmlExchangePersistence();
            IStockExchange exchange = persistence.Load(new XmlFilePersistenceOptions(filePath, fileSystem), logger);
            var stock = exchange.Stocks.First();
            stock.AddValue(new DateTime(2022, 1, 1), 12, 12, 12, 12, 1444);
            persistence.Save(exchange, new XmlFilePersistenceOptions("c:/temp/example2.xml", fileSystem), logger);
        }

        public static readonly string CurrentPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        public const string ExampleDatabaseFolder = "ExampleDatabases";

        public static readonly string ExampleDatabaseLocation = $"{CurrentPath}\\{ExampleDatabaseFolder}";
        
        [Test]
        public void CreateSqliteDbTest()
        {           
            void reportAction(ReportSeverity severity, ReportType reportType, string location, string text)
            {
            }

            var fileSystem = new MockFileSystem();
            if (System.IO.File.Exists(fileSystem.Path.Combine(ExampleDatabaseLocation, "test.db")))
            {
                System.IO.File.Delete(fileSystem.Path.Combine(ExampleDatabaseLocation, "test.db"));
            }

            var logger = new LogReporter(reportAction, saveInternally: true);
            var testDbPath = fileSystem.Path.Combine(ExampleDatabaseLocation, "test.db");
            var exchange = new StockExchange();
            exchange.ExchangeIdentifier = "LSE";
            exchange.Name = "London Stock Exchange";
            var stock = new Stock();
            stock.Name = new NameData() { Ric = "BARC.L", Company = "Barclays", Exchange = "LSE" };
            stock.Valuations.Add(new StockDay(new DateTime(2023, 1, 1, 9, 0, 0), 23, 24, 22, 23.5m, 100000));
            exchange.Stocks.Add(stock);
            var sqlitePersistence = new SqliteExchangePersistence();
            sqlitePersistence.Save(exchange, new SqlitePersistenceOptions(true, testDbPath, new FileSystem()), logger);
        }
    }
}