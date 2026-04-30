using Effanville.Common.Structure.WebAccess;
using Microsoft.Extensions.Logging;

namespace Effanville.FinancialStructures.Stocks.Download
{
    public sealed class StockPriceDownloaderFactory : IStockDownloaderFactory
    {
        private readonly YahooDownloader YahooDownloader;

        public StockPriceDownloaderFactory(ILoggerFactory loggerFactory, WebDownloader webDownloader)
            => YahooDownloader = new YahooDownloader(loggerFactory.CreateLogger<YahooDownloader>(), webDownloader);

        public IStockDownloader Retrieve(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            return url.Contains("yahoo") ? YahooDownloader : null;
        }
    }
}