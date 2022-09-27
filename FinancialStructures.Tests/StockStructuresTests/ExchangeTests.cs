using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialStructures.StockStructures.Implementation;
using FinancialStructures.StockStructures;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;
using Common.Structure.Reporting;

namespace FinancialStructures.Tests.StockStructuresTests
{
    internal sealed class ExchangeTests
    {
        public string db = @"<?xml version=""1.0"" encoding=""utf-8""?>
<StockExchange xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Stocks>
    <Stock>
      <Ticker>JMAT</Ticker>
      <Name>
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
            string filePath = "c:\\temp\\example.xml";
            var reports = new ErrorReports();
            void reportAction(ReportSeverity severity, ReportType reportType, ReportLocation location, string text)
            {
                reports.AddErrorReport(severity, reportType, location, text);
            }

            var logger = new LogReporter(reportAction);
            var fileSystem = new MockFileSystem();

            fileSystem.AddFile(filePath, db);

            var startDate = new DateTime(2010, 1, 1);
            var endDate = new DateTime(2020, 1, 1);
            IStockExchange exchange = new StockExchange();
            exchange.LoadStockExchange(filePath, fileSystem, logger);
            exchange.Download(startDate, endDate, logger).Wait();
            exchange.SaveStockExchange("c:\\temp\\example2.xml", fileSystem, logger);
        }
    }
}
