using System.Threading.Tasks;
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
        [TestCase("https://uk.finance.yahoo.com/quote/^FVX")]
        [TestCase("https://uk.finance.yahoo.com/quote/USDGBP=X")]
        [TestCase("https://uk.finance.yahoo.com/quote/HKDGBP=X")]
        [TestCase("https://www.morningstar.co.uk/uk/etf/snapshot/snapshot.aspx?id=0P0000WAHE")]
        [TestCase("https://www.morningstar.co.uk/uk/funds/snapshot/snapshot.aspx?id=F0GBR04S22")]
        public async Task CanDownload(string url)
        {
            decimal value = 0;
            void getValue(decimal v)
            {
                value = v;
            }

            await PortfolioDataUpdater.DownloadLatestValue(new NameData("company", "name", url: url), getValue, null);

            Assert.AreNotEqual(0m, value);
        }
    }
}
