using FinancialStructures.Download.Implementation;

namespace FinancialStructures.Download
{
    /// <summary>
    /// Provides factory methods for creating price downloaders.
    /// </summary>
    public static class PriceDownloaderFactory
    {
        private static readonly MorningstarDownloader fMorningstarDownloader = new MorningstarDownloader();
        private static readonly YahooDownloader fYahooDownloader = new YahooDownloader();
        private static readonly FtDownloader fFtDownloader = new FtDownloader();

        /// <summary>
        /// Retrieve the relevant price downloader.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IPriceDownloader Retrieve(string url)
        {
            if(string.IsNullOrEmpty(url))
            {
                return null;
            }

            if (url.Contains("morningstar"))
            {
                return fMorningstarDownloader;
            }
            else if (url.Contains("yahoo"))
            {
                return fYahooDownloader;
            }
            else if (url.Contains("markets.ft"))
            {
                return fFtDownloader;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the code part from the url.
        /// </summary>
        public static string RetrieveCodeFromUrl(string url)
        {
            if(string.IsNullOrEmpty(url))
            {
                return null;
            }
            
            if (url.Contains("morningstar"))
            {
                return MorningstarDownloader.GetFinancialCode(url);
            }
            else if (url.Contains("yahoo"))
            {
                return YahooDownloader.GetFinancialCode(url);
            }
            else if (url.Contains("markets.ft"))
            {
                return FtDownloader.GetFinancialCode(url);
            }
            else
            {
                return null;
            }
        }
    }
}
