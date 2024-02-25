using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Persistence;
using Effanville.FinancialStructures.Stocks.HistoricalRepository;

namespace Effanville.FinancialStructures.Stocks.Persistence
{
    public interface IHistoricalMarketsPersistence
    {
        /// <summary>
        /// Loads the <see cref="HistoricalMarkets"/> from the file specified.
        /// </summary>
        HistoricalMarkets Load(PersistenceOptions options, IReportLogger reportLogger = null);

        /// <summary>
        /// Saves the <see cref="HistoricalMarkets"/> to the file specified.
        /// </summary>
        bool Save(HistoricalMarkets historicalMarkets, PersistenceOptions options, IReportLogger reportLogger = null);
    }
}