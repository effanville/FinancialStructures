using Effanville.FinancialStructures.Download.Implementation;

namespace Effanville.FinancialStructures.Download;

public sealed class PriceDownloaderFactory : IPriceDownloaderFactory
{
    private static readonly MorningstarDownloader _morningstarDownloader = new MorningstarDownloader();
    private static readonly YahooDownloader _yahooDownloader = new YahooDownloader();
    private static readonly FtDownloader _ftDownloader = new FtDownloader();
    private static readonly BloombergDownloader _bloombergDownloader = new BloombergDownloader();

    public IPriceDownloader Retrieve(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }

        if (url.Contains("morningstar"))
        {
            return _morningstarDownloader;
        }
        if (url.Contains("yahoo"))
        {
            return _yahooDownloader;
        }
        if (url.Contains("markets.ft"))
        {
            return _ftDownloader;
        }
        if (url.Contains("bloomberg"))
        {
            return _bloombergDownloader;
        }

        return null;
    }

    public string RetrieveCodeFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }

        if (url.Contains("morningstar"))
        {
            return _morningstarDownloader.GetFinancialCode(url);
        }
        if (url.Contains("yahoo"))
        {
            return _yahooDownloader.GetFinancialCode(url);
        }
        if (url.Contains("markets.ft"))
        {
            return _ftDownloader.GetFinancialCode(url);
        }

        if (url.Contains("bloomberg"))
        {
            return _bloombergDownloader.GetFinancialCode(url);
        }

        return null;
    }
}