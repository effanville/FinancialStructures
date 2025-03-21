using System;
using System.Collections.Generic;
using System.Linq;
using Effanville.Common.Structure.Reporting;

namespace Effanville.FinancialStructures.Download;

internal static class DownloadResultExtensions
{
    internal static void ReportResults(this List<DownloadResult> results, IReportLogger logger)
    {
        results.Sort((x, y) => y.Success.CompareTo(x.Success));
        foreach (DownloadResult result in results)
        {
            logger?.Debug(nameof(PortfolioDataDownloader), $"DownloadResult. Name={result.Name}, Url='{result.Name.Url}', Success={result.Success}, Value={result.Value}");
        }

        int numberSuccess = results.Count(x => x.Success);
        int numberFailure = results.Count(x => !x.Success && x.Value >= 0.0m);
        int numberNoUrl = results.Count(x => string.IsNullOrWhiteSpace(x.Name.Url));
        logger?.Info(nameof(PortfolioDataDownloader),
            $"DownloadResults. Total={results.Count} Success={numberSuccess}, Failure={numberFailure}, NoUrl={numberNoUrl}");
    }
}
