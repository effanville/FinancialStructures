using System;
using System.Threading.Tasks;

using FinancialStructures.Stocks.Download;
using FinancialStructures.Stocks.Implementation;

using NUnit.Framework;

namespace FinancialStructures.Stocks.Tests;

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

    [TestCase("https://uk.finance.yahoo.com/quote/GRP.L")]
    [TestCase("https://uk.finance.yahoo.com/quote/JXN?p=JXN&.tsrc=fin-srch")]
    [TestCase("https://uk.finance.yahoo.com/quote/GBPUSD%3DX?p=GBPUSD%3DX")]
    [TestCase("https://uk.finance.yahoo.com/quote/VUKE.L/options?p=VUKE.L")]
    [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L/history?p=VWRL.L")]
    public async Task CanDownloadHistoryData(string url)
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
            new DateTime(2021, 1, 1),
            new DateTime(2021, 2, 2),
            TimeSpan.FromDays(1),
            getValue,
            null);

        Assert.IsNotNull(value);
    }
}