using System.Threading.Tasks;

using Effanville.FinancialStructures.Database.Download;
using Effanville.FinancialStructures.Download;
using Effanville.FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database
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
        [TestCase("https://uk.finance.yahoo.com/quote/AW01.FGI")]
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
        //[TestCase("https://www.morningstar.co.uk/uk/etf/snapshot/snapshot.aspx?id=0P0000WAHE")]
        //[TestCase("https://www.morningstar.co.uk/uk/funds/snapshot/snapshot.aspx?id=F0GBR04S22")]
        [TestCase("https://uk.finance.yahoo.com/quote/GRP.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/JXN?p=JXN&.tsrc=fin-srch")]
        [TestCase("https://uk.finance.yahoo.com/quote/ship.l")]
        [TestCase("https://uk.finance.yahoo.com/quote/GBPEUR%3DX")]
        [TestCase("https://uk.finance.yahoo.com/quote/GBPUSD%3DX?p=GBPUSD%3DX")]
        [TestCase("https://markets.ft.com/data/etfs/tearsheet/summary?s=2800:HKG:HKD")]
        [TestCase("https://markets.ft.com/data/etfs/tearsheet/summary?s=Vwrl:LSE:GBP")]
        [TestCase("https://markets.ft.com/data/etfs/tearsheet/summary?s=saaa:LSE:GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/SAAA.L")]
        [TestCase("https://markets.ft.com/data/equities/tearsheet/summary?s=IBKR:NSQ")]
        [TestCase("https://finance.yahoo.com/quote/IBKR")]
        [TestCase("https://markets.ft.com/data/currencies/tearsheet/summary?s=HKDGBP")]
        [TestCase("https://markets.ft.com/data/indices/tearsheet/summary?s=HSI:HKG")]
        [TestCase("https://markets.ft.com/data/indices/tearsheet/summary?s=INX:IOM")]
        [TestCase("https://markets.ft.com/data/indices/tearsheet/summary?s=FTSE:FSI")]
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

        [TestCase("https://uk.finance.yahoo.com/quote/GRP.L", "GRP.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/JXN?p=JXN&.tsrc=fin-srch", "JXN")]
        [TestCase("https://uk.finance.yahoo.com/quote/GBPUSD%3DX?p=GBPUSD%3DX", "GBPUSD=X")]
        [TestCase("https://uk.finance.yahoo.com/quote/VUKE.L/options?p=VUKE.L", "VUKE.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L/history?p=VWRL.L", "VWRL.L")]
        //[TestCase("https://www.morningstar.co.uk/uk/etf/snapshot/snapshot.aspx?id=0P0000WAHE", "0P0000WAHE")]
        [TestCase("https://markets.ft.com/data/funds/tearsheet/summary?s=gb00b4khn986:gbx", "GB00B4KHN986:GBX")]
        public void CanGetCode(string url, string expectedCode)
        {
            string code = PriceDownloaderFactory.RetrieveCodeFromUrl(url);
            Assert.AreEqual(expectedCode, code);
        }
    }
}
