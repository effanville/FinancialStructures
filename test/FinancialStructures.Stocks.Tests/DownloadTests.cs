using System;
using System.Linq;
using System.Threading.Tasks;

using Effanville.FinancialStructures.Stocks.Download;
using Effanville.FinancialStructures.Stocks.Implementation;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Stocks.Tests;

public sealed class DownloadTests
{
    
    [TestCase("https://uk.finance.yahoo.com/quote/GRP.L")]
    [TestCase("https://uk.finance.yahoo.com/quote/JXN?p=JXN&.tsrc=fin-srch")]
    [TestCase("https://uk.finance.yahoo.com/quote/GBPUSD%3DX?p=GBPUSD%3DX")]
    [TestCase("https://uk.finance.yahoo.com/quote/VUKE.L/options?p=VUKE.L")]
    [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L/history?p=VWRL.L")]
    public async Task CanDownloadAllDayData(string url)
    {
        StockDay value = null;
        void getValue(StockDay v)
        {
            value = v;
        }
        var downloader = StockPriceDownloaderFactory.Retrieve(url);
        string code = downloader.GetFinancialCode(url);
        _ = await downloader.TryGetLatestPriceData(code, getValue, null);

        Assert.IsNotNull(value);
    }

    [TestCase("https://uk.finance.yahoo.com/quote/GRP.L",21, 1.11000001430511,1.12999999523163)]
    [TestCase("https://uk.finance.yahoo.com/quote/JXN?p=JXN&.tsrc=fin-srch",21, 42.25,42.3199996948242)]
    [TestCase("https://uk.finance.yahoo.com/quote/GBPUSD%3DX?p=GBPUSD%3DX",23, 1.35222840309143, 1.35222840309143)]
    [TestCase("https://uk.finance.yahoo.com/quote/VUKE.L/options?p=VUKE.L",21,32.6450004577637, 32.7750015258789)]
    [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L/history?p=VWRL.L",21,92.5400009155274, 91.870002746582)]
    public async Task CanDownloadHistoryData(string url, int numberEntries, decimal? open = null, decimal? close = null)
    {
        IStock value = null;
        void getValue(IStock v)
        {
            value = v;
        }
            
        var downloader = StockPriceDownloaderFactory.Retrieve(url);
        string code = downloader.GetFinancialCode(url);
        _ = await downloader.TryGetFullPriceHistory(
            code,
            new DateTime(2022, 1, 1),
            new DateTime(2022, 2, 2),
            TimeSpan.FromDays(1),
            getValue,
            null);

        Assert.IsNotNull(value);
        Assert.That(value.Valuations.Count, Is.EqualTo(numberEntries));
        StockDay first = value.Valuations.First();
        if(open.HasValue)
            Assert.That(first.Open, Is.EqualTo(open.Value));
        if(close.HasValue)
            Assert.That(first.Close, Is.EqualTo(close.Value));
    }
}