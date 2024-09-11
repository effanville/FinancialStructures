using System.Threading.Tasks;

using Effanville.FinancialStructures.Database.Download;
using Effanville.FinancialStructures.Download;
using Effanville.FinancialStructures.NamingStructures;
using Nager.Date.Model;
using NUnit.Framework;
using OpenQA.Selenium.DevTools.V113.Memory;

namespace Effanville.FinancialStructures.Tests.Database
{
    [TestFixture]
    public sealed class DownloadTests
    {
        [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L", "GBP", 50,500)]
        [TestCase("https://uk.finance.yahoo.com/quote/VUKE.L", "GBP", 10,100)]
        [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L/history?p=VWRL.L", "GBP", 50,500)]
        [TestCase("https://uk.finance.yahoo.com/quote/VUKE.L/options?p=VUKE.L", "GBP", 10,100)]
        [TestCase("https://uk.finance.yahoo.com/quote/%5EFVX", "GBP", 1,10)]
        [TestCase("https://uk.finance.yahoo.com/quote/%5ESTOXX50E", "GBP", 2000,8000)]
        [TestCase("https://uk.finance.yahoo.com/quote/AW01.FGI", "USD", 200,1000)]
        [TestCase("https://markets.ft.com/data/indices/tearsheet/summary?s=AW01:FSI", "USD", 200, 1000)]
        [TestCase("https://uk.finance.yahoo.com/quote/%5EN225", "GBP", 20000,80000)]
        [TestCase("https://uk.finance.yahoo.com/quote/GOOGL?p=GOOGL&.tsrc=fin-srch", "GBP", 50,500)]
        [TestCase("https://uk.finance.yahoo.com/quote/abdp.L", "GBP", 10,50)]
        [TestCase("https://uk.finance.yahoo.com/quote/aht.l", "GBP", 20,90)]
        [TestCase("https://uk.finance.yahoo.com/quote/cwr.l", "GBP", 0.5,10)]
        [TestCase("https://uk.finance.yahoo.com/quote/drx.L", "GBP", 2,10)]
        [TestCase("https://uk.finance.yahoo.com/quote/hwdn.L", "GBP", 1,15)]
        [TestCase("https://uk.finance.yahoo.com/quote/JXN?p=JXN&.tsrc=fin-srch", "GBP", 30,300)]
        [TestCase("https://uk.finance.yahoo.com/quote/hgen.L", "GBP", 0.1,10)]
        [TestCase("https://markets.ft.com/data/funds/tearsheet/summary?s=gb00b4khn986:gbx", "GBP", 1,10)]
        [TestCase("https://uk.finance.yahoo.com/quote/^FVX", "GBP", 1,10)]
        [TestCase("https://uk.finance.yahoo.com/quote/USDGBP=X", "GBP", 0.2,10)]
        [TestCase("https://uk.finance.yahoo.com/quote/HKDGBP=X", "GBP", 0.01,10)]
        [TestCase("https://www.morningstar.co.uk/uk/etf/snapshot/snapshot.aspx?id=0P0000WAHE", "GBP", 50,500)]
        [TestCase("https://www.morningstar.co.uk/uk/funds/snapshot/snapshot.aspx?id=F0GBR04S22", "GBP", 5,100)]
        [TestCase("https://uk.finance.yahoo.com/quote/GRP.L", "EUR", 0.1,10)]
        [TestCase("https://uk.finance.yahoo.com/quote/JXN?p=JXN&.tsrc=fin-srch", "GBP", 30,300)]
        [TestCase("https://uk.finance.yahoo.com/quote/ship.l", "GBP", 0.001,10)]
        [TestCase("https://uk.finance.yahoo.com/quote/GBPEUR%3DX", "GBP", 0.5,10)]
        [TestCase("https://uk.finance.yahoo.com/quote/GBPUSD%3DX?p=GBPUSD%3DX", "GBP", 0.2,10)]
        [TestCase("https://markets.ft.com/data/etfs/tearsheet/summary?s=2800:HKG:HKD", "HKD", 10,50)]
        [TestCase("https://markets.ft.com/data/etfs/tearsheet/summary?s=Vwrl:LSE:GBP", "GBP", 50,500)]
        [TestCase("https://markets.ft.com/data/etfs/tearsheet/summary?s=saaa:LSE:GBP", "GBP", 20,200)]
        [TestCase("https://uk.finance.yahoo.com/quote/SAAA.L", "GBP", 10,100)]
        [TestCase("https://markets.ft.com/data/equities/tearsheet/summary?s=IBKR:NSQ", "USD", 50,500)]
        [TestCase("https://finance.yahoo.com/quote/IBKR", "GBP", 50,500)]
        [TestCase("https://markets.ft.com/data/currencies/tearsheet/summary?s=HKDGBP", "GBP", 0.01,1)]
        [TestCase("https://markets.ft.com/data/indices/tearsheet/summary?s=HSI:HKG", "HKD", 12000,50000)]
        [TestCase("https://markets.ft.com/data/indices/tearsheet/summary?s=INX:IOM", "USD", 2000,9000)]
        [TestCase("https://markets.ft.com/data/indices/tearsheet/summary?s=FTSE:FSI", "GBP", 2000,20000)]
        [TestCase("https://www.morningstar.co.uk/uk/funds/snapshot/snapshot.aspx?id=F00000ZJEI", "EUR", 2,100)]
        [TestCase("https://www.bloomberg.com/quote/MAMMGEE:HK", "HKD", 5, 50)]
        [TestCase("https://www.bloomberg.com/quote/MLCOREA:HK", "HKD", 2, 200)]
        [TestCase("https://uk.finance.yahoo.com/quote/XMTW.L", "GBP", 20, 100)]
        public async Task CanDownload(string url, string currency, double lower, double upper)
        {
            decimal value = 0;

            await PortfolioDataUpdater.DownloadLatestValue(new NameData("company", "name", url: url, currency: currency), GetValue);

            Assert.That(value, Is.Not.EqualTo(0m));
            Assert.That(value, Is.InRange(lower, upper));
            return;

            void GetValue(decimal v)
            {
                value = v;
            }
        }

        [TestCase("https://uk.finance.yahoo.com/quote/GRP.L", "GRP.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/JXN?p=JXN&.tsrc=fin-srch", "JXN")]
        [TestCase("https://uk.finance.yahoo.com/quote/GBPUSD%3DX?p=GBPUSD%3DX", "GBPUSD=X")]
        [TestCase("https://uk.finance.yahoo.com/quote/VUKE.L/options?p=VUKE.L", "VUKE.L")]
        [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L/history?p=VWRL.L", "VWRL.L")]
        [TestCase("https://www.morningstar.co.uk/uk/etf/snapshot/snapshot.aspx?id=0P0000WAHE", "0P0000WAHE")] //VWRL
        [TestCase("https://markets.ft.com/data/funds/tearsheet/summary?s=gb00b4khn986:gbx", "GB00B4KHN986:GBX")]
        [TestCase("https://www.bloomberg.com/quote/MAMMGEE:HK", "MAMMGEE:HK")]
        public void CanGetCode(string url, string expectedCode)
        {
            string code = PriceDownloaderFactory.RetrieveCodeFromUrl(url);
            Assert.That(code, Is.EqualTo(expectedCode));
        }
    }
}
