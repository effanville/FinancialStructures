using System;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using NUnit.Framework;

namespace FinancialStructures.Tests.PortfolioAPI
{
    public sealed class DownloadTests
    {
        [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L")]
        [TestCase("https://markets.ft.com/data/funds/tearsheet/summary?s=gb00b4khn986:gbx")]
        public void CanDownload(string url)
        {
            double value = 0;
            Action<double> getValue = v => value = v;
            PortfolioDataUpdater.DownloadLatestValue(new NameData("company", "name", url: url), getValue, null).Wait();

            Assert.AreNotEqual(0, value);
        }
    }
}
