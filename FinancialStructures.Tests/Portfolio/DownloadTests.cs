using FinancialStructures.Database.Download;
using FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace FinancialStructures.Tests.Database
{
    [TestFixture]
    public sealed class DownloadTests
    {
        [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L")]
        [TestCase("https://markets.ft.com/data/funds/tearsheet/summary?s=gb00b4khn986:gbx")]
        public void CanDownload(string url)
        {
            decimal value = 0;
            void getValue(decimal v)
            {
                value = v;
            }

            PortfolioDataUpdater.DownloadLatestValue(new NameData("company", "name", url: url), getValue, null).Wait();

            Assert.AreNotEqual(0m, value);
        }
    }
}
