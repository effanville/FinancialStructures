using Common.Structure.Reporting;

namespace FinancialStructures.Stocks.Persistence
{
    public interface IExchangePersistence
    {
        /// <summary>
        /// Loads the <see cref="IStockExchange"/> from the file specified.
        /// </summary>
        IStockExchange Load(PersistenceOptions options, IReportLogger reportLogger = null);

        /// <summary>
        /// Saves the <see cref="IStockExchange"/> to the file specified.
        /// </summary>
        void Save(IStockExchange exchange, PersistenceOptions options, IReportLogger reportLogger = null);
    }
}