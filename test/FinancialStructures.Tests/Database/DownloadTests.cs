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
        [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/VUKE.L", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L/history?p=VWRL.L", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/VUKE.L/options?p=VUKE.L", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/%5EFVX", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/%5ESTOXX50E", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/AW01.FGI", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/%5EN225", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/GOOGL?p=GOOGL&.tsrc=fin-srch", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/abdp.L", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/aht.l", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/cwr.l", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/drx.L", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/hwdn.L", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/JXN?p=JXN&.tsrc=fin-srch", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/hgen.L", "GBP")]
        [TestCase("https://markets.ft.com/data/funds/tearsheet/summary?s=gb00b4khn986:gbx", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/^FVX", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/USDGBP=X", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/HKDGBP=X", "GBP")]
        [TestCase("https://www.morningstar.co.uk/uk/etf/snapshot/snapshot.aspx?id=0P0000WAHE", "GBP")]
        [TestCase("https://www.morningstar.co.uk/uk/funds/snapshot/snapshot.aspx?id=F0GBR04S22", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/GRP.L", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/JXN?p=JXN&.tsrc=fin-srch", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/ship.l", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/GBPEUR%3DX", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/GBPUSD%3DX?p=GBPUSD%3DX", "GBP")]
        [TestCase("https://markets.ft.com/data/etfs/tearsheet/summary?s=2800:HKG:HKD", "HKD")]
        [TestCase("https://markets.ft.com/data/etfs/tearsheet/summary?s=Vwrl:LSE:GBP", "GBP")]
        [TestCase("https://markets.ft.com/data/etfs/tearsheet/summary?s=saaa:LSE:GBP", "GBP")]
        [TestCase("https://uk.finance.yahoo.com/quote/SAAA.L", "GBP")]
        [TestCase("https://markets.ft.com/data/equities/tearsheet/summary?s=IBKR:NSQ", "USD")]
        [TestCase("https://finance.yahoo.com/quote/IBKR", "GBP")]
        [TestCase("https://markets.ft.com/data/currencies/tearsheet/summary?s=HKDGBP", "GBP")]
        [TestCase("https://markets.ft.com/data/indices/tearsheet/summary?s=HSI:HKG", "HKD")]
        [TestCase("https://markets.ft.com/data/indices/tearsheet/summary?s=INX:IOM", "USD")]
        [TestCase("https://markets.ft.com/data/indices/tearsheet/summary?s=FTSE:FSI", "GBP")]
        [TestCase("https://www.morningstar.co.uk/uk/funds/snapshot/snapshot.aspx?id=F00000ZJEI", "EUR")]
        public async Task CanDownload(string url, string currency)
        {
            decimal value = 0;

            await PortfolioDataUpdater.DownloadLatestValue(new NameData("company", "name", url: url, currency: currency), GetValue);

            Assert.That(value, Is.Not.EqualTo(0m));
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
        public void CanGetCode(string url, string expectedCode)
        {
            string code = PriceDownloaderFactory.RetrieveCodeFromUrl(url);
            Assert.That(code, Is.EqualTo(expectedCode));
        }
    }
}
