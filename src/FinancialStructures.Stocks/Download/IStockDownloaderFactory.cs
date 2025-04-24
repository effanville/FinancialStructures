namespace Effanville.FinancialStructures.Stocks.Download
{
    public interface IStockDownloaderFactory
    {
        IStockDownloader Retrieve(string url);
    }
}