using FinancialStructures.Download.Implementation;
using FinancialStructures.StockStructures.Download;

namespace FinancialStructures.StockStructures.Download
{
    public static class StockPriceDownloaderFactory
    {
        private static readonly YahooDownloader YahooDownloader = new YahooDownloader();

        public static IStockDownloader Retrieve(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            return url.Contains("yahoo") ? YahooDownloader : null;
        }
    }
}