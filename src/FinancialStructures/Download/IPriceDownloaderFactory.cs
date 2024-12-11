namespace Effanville.FinancialStructures.Download;

/// <summary>
/// Provides factory methods for creating price downloaders.
/// </summary>
public interface IPriceDownloaderFactory
{
    /// <summary>
    /// Retrieve the relevant price downloader.
    /// </summary>
    IPriceDownloader Retrieve(string url);

    /// <summary>
    /// Returns the code part from the url.
    /// </summary>
    string RetrieveCodeFromUrl(string url);
}