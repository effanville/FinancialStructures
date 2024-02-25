using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Persistence;

namespace Effanville.FinancialStructures.Stocks.Persistence
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
        bool Save(IStockExchange exchange, PersistenceOptions options, IReportLogger reportLogger = null);
    }
}