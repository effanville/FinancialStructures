using Effanville.Common.Structure.Reporting;
using Effanville.Common.Structure.WebAccess;

namespace Effanville.FinancialStructures.Stocks.Download
{
    public sealed class StockPriceDownloaderFactory : IStockDownloaderFactory
    {
        private readonly YahooDownloader YahooDownloader;

        public StockPriceDownloaderFactory(IReportLogger logger, WebDownloader webDownloader)
            => YahooDownloader = new YahooDownloader(logger, webDownloader);

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