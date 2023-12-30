using Common.Structure.Reporting;

using FinancialStructures.Persistence;
using FinancialStructures.Stocks.HistoricalRepository;

namespace FinancialStructures.Stocks.Persistence
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