using Effanville.Common.Structure.Reporting;
using Effanville.Common.Structure.WebAccess;
using Effanville.FinancialStructures.Download.Implementation;

namespace Effanville.FinancialStructures.Download;

public sealed class PriceDownloaderFactory : IPriceDownloaderFactory
{
    private readonly MorningstarDownloader _morningstarDownloader;
    private readonly YahooDownloader _yahooDownloader;
    private readonly FtDownloader _ftDownloader;
    private readonly BloombergDownloader _bloombergDownloader;

    public PriceDownloaderFactory(IReportLogger logger, WebDownloader webDownloader)
    {
        _ftDownloader = new FtDownloader(logger, webDownloader);
        _yahooDownloader = new YahooDownloader(logger, webDownloader);
        _morningstarDownloader = new MorningstarDownloader(logger, webDownloader);
        _bloombergDownloader = new BloombergDownloader(logger, webDownloader);
    }

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