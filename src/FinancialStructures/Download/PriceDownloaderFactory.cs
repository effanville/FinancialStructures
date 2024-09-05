using Effanville.FinancialStructures.Download.Implementation;

namespace Effanville.FinancialStructures.Download
{
    /// <summary>
    /// Provides factory methods for creating price downloaders.
    /// </summary>
    public static class PriceDownloaderFactory
    {
        private static readonly MorningstarDownloader _morningstarDownloader = new MorningstarDownloader();
        private static readonly YahooDownloader _yahooDownloader = new YahooDownloader();
        private static readonly FtDownloader _ftDownloader = new FtDownloader();

        /// <summary>
        /// Retrieve the relevant price downloader.
        /// </summary>
        public static IPriceDownloader Retrieve(string url)
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
            
            return null;
        }

        /// <summary>
        /// Returns the code part from the url.
        /// </summary>
        public static string RetrieveCodeFromUrl(string url)
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
                
            return null;
        }
    }
}