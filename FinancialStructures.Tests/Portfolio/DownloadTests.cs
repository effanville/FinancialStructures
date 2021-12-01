using FinancialStructures.Database.Download;
using FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace FinancialStructures.Tests.Database
{
    [TestFixture]
    public sealed class DownloadTests
    {
        [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/VUKE.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L/history?p=VWRL.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/VUKE.L/options?p=VUKE.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/%5EFVX")]
        [TestCase("https://uk.finance.yahoo.com/quote/%5ESTOXX50E")]
        [TestCase("https://finance.yahoo.com/quote/AW01.FGI/")]
        [TestCase("https://uk.finance.yahoo.com/quote/%5EN225")]
        [TestCase("https://uk.finance.yahoo.com/quote/GOOGL?p=GOOGL&.tsrc=fin-srch")]
        [TestCase("https://uk.finance.yahoo.com/quote/abdp.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/aht.l")]
        [TestCase("https://uk.finance.yahoo.com/quote/cwr.l")]
        [TestCase("https://uk.finance.yahoo.com/quote/drx.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/hwdn.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/JXN?p=JXN&.tsrc=fin-srch")]
        [TestCase("https://uk.finance.yahoo.com/quote/hgen.L")]
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
