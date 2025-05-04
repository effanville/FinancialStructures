using System.Threading.Tasks;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.Download;

/// <summary>
/// Contains download routines to update portfolio.
/// </summary>
public interface IPortfolioDataDownloader
{
    /// <summary>
    /// Updates the given portfolio.
    /// </summary>
    /// <param name="portfolio">The database storing the object</param>
    Task Download(IPortfolio portfolio);

    /// <summary>
    /// Updates the given valuelist.
    /// </summary>
    /// <param name="valueList">The database storing the object</param>
    Task Download(IValueList valueList);
}
