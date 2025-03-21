using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Download;

public sealed class PortfolioDataDownloader : IPortfolioDataDownloader
{
    private readonly IPriceDownloaderFactory _priceDownloaderFactory;

    public PortfolioDataDownloader(IPriceDownloaderFactory priceDownloaderFactory)
        => _priceDownloaderFactory = priceDownloaderFactory;

    public async Task Download(IPortfolio portfolio, IReportLogger reportLogger = null)
    {
        List<DownloadResult> results = new List<DownloadResult>();
        List<Task> downloadTasks = new List<Task>();
        Add(downloadTasks, results, portfolio.Accounts(Account.All), portfolio, reportLogger);
        Add(downloadTasks, results, portfolio.Currencies, portfolio, reportLogger);
        Add(downloadTasks, results, portfolio.BenchMarks, portfolio, reportLogger);

        await Task.WhenAll(downloadTasks);

        results.ReportResults(reportLogger);
        reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, nameof(PortfolioDataDownloader), "Downloader Completed");
    }

    public async Task Download(IValueList valueList, IReportLogger reportLogger)
    {
        List<DownloadResult> results = new List<DownloadResult>();
        await DownloadLatestValue(
            valueList.Names,
            value => valueList.UpdateAndCheck(value, reportLogger, results),
            reportLogger);
        results.ReportResults(reportLogger);
        reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, nameof(PortfolioDataDownloader), "Downloader Completed");
    }

    /// <summary>
    /// Downloads the latest value from the website stored in <paramref name="names"/> url field.
    /// </summary>
    internal async Task DownloadLatestValue(NameData names, Action<decimal> updateValue, IReportLogger reportLogger = null)
    {
        IPriceDownloader downloader = _priceDownloaderFactory.Retrieve(names.Url);
        if (downloader == null)
        {
            reportLogger?.Error(nameof(PortfolioDataDownloader), $"{names.Company}-{names.Name}: Url='{names.Url}' not of supported type");
            return;
        }

        if (!await downloader.TryGetLatestPriceFromUrl(names.Url, names.Currency, updateValue, reportLogger))
        {
            reportLogger?.Error(nameof(PortfolioDataDownloader), $"{names.Company}-{names.Name}: Couldnt get price data from {names.Url}");
        }
    }

    private void Add(
        List<Task> downloadTasks,
        List<DownloadResult> results,
        IReadOnlyList<IValueList> accounts,
        IPortfolio portfolio,
        IReportLogger reportLogger)
    {
        foreach (IValueList acc in accounts)
        {
            if (!string.IsNullOrEmpty(acc.Names.Url))
            {
                NameData nameData = acc.Names.Copy();
                if (string.IsNullOrWhiteSpace(nameData.Currency))
                {
                    nameData.Currency = portfolio.BaseCurrency ?? "GBP";
                }
                results.Add(new DownloadResult() { Name = nameData });
                downloadTasks.Add(DownloadLatestValue(
                    nameData,
                    value => acc.UpdateAndCheck(value, reportLogger, results),
                    reportLogger));
            }
            else
            {
                results.Add(new DownloadResult() { Name = acc.Names, Value = -1m });
                reportLogger?.Debug(nameof(PortfolioDataDownloader), $"No Url set for {acc.Names}");
            }
        }
    }
}
